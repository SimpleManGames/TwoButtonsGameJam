namespace HSM
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using HSM.Enums;
    using HSM.Interfaces;

    using UnityEngine;

    public class TransitionSequencer
    {
        private readonly StateMachine _machine;

        private ISequence _sequencer; // current phase (deactivate or activate)
        private Action _nextPhase; // switch structure between phases
        private (State from, State to)? _pending; // coalesce a single pending request
        private State _lastFrom, _lastTo;

        private CancellationTokenSource _cts;
        private bool _useSequential = false; // set false to use parallel

        public TransitionSequencer(StateMachine machine)
        {
            _machine = machine;
        }

        // Request a transition from one state to another
        public void RequestTransition(State from, State to)
        {
            if (to == null || from == to) return;

            if (_sequencer != null)
            {
                _pending = (from, to);
                return;
            }
            BeginTransition(from, to);
        }

        private static List<PhaseStep> GatherPhaseSteps(List<State> chain, bool deactivate)
        {
            List<PhaseStep> steps = new List<PhaseStep>();

            for (int i = 0; i < chain.Count; i++)
            {
                State st = chain[i];
                IReadOnlyList<IActivity> acts = chain[i].Activities;

                for (int j = 0; j < acts.Count; j++)
                {
                    IActivity a = acts[j];
                    bool include = deactivate ? a.Mode == ActivityMode.Active : a.Mode == ActivityMode.Inactive;

                    if (!include)
                    {
                        continue;
                    }

                    steps.Add(ct => deactivate ? a.DeactivateAsync(ct) : a.ActivateAsync(ct));
                }
            }
            return steps;
        }

        // States to exit: from → ... up to (but excluding) lca; bottom→up order.
        private static List<State> StatesToExit(State from, State lca)
        {
            List<State> list = new List<State>();
            for (State s = from; s != null && s != lca; s = s.Parent) 
                list.Add(s);
            return list;
        }

        // States to enter: path from 'to' up to (but excluding) lca; returned in enter order (top→down).
        private static List<State> StatesToEnter(State to, State lca)
        {
            Stack<State> stack = new Stack<State>();
            for (State s = to; s != lca; s = s.Parent) stack.Push(s);
            return new List<State>(stack);
        }

        private void BeginTransition(State from, State to)
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();

            State lca = LowestCommonAncestor(from, to);
            List<State> exitChain = StatesToExit(from, lca);
            List<State> enterChain = StatesToEnter(to, lca);

            // 1. Deactivate the “old branch”
            List<PhaseStep> exitSteps = GatherPhaseSteps(exitChain, deactivate: true);

            // sequencer = new NoopPhase();
            _sequencer = _useSequential
                ? new SequentialPhase(exitSteps, _cts.Token)
                : new ParallelPhase(exitSteps, _cts.Token);
            _sequencer.Start();

            _nextPhase = () =>
            {
                // 2. ChangeState
                _machine.ChangeState(from, to);

                // 3. Activate the “new branch”
                List<PhaseStep> enterSteps = GatherPhaseSteps(enterChain, deactivate: false);

                // sequencer = new NoopPhase();
                _sequencer = _useSequential
                    ? new SequentialPhase(enterSteps, _cts.Token)
                    : new ParallelPhase(enterSteps, _cts.Token);
                _sequencer.Start();
            };
        }

        private void EndTransition()
        {
            _sequencer = null;

            if (_pending.HasValue)
            {
                (State from, State to) p = _pending.Value;
                _pending = null;
                BeginTransition(p.from, p.to);
            }
        }

        public void Tick(float deltaTime)
        {
            if (_sequencer != null)
            {
                if (_sequencer.Update())
                {
                    if (_nextPhase != null)
                    {
                        Action n = _nextPhase;
                        _nextPhase = null;
                        n();
                    }
                    else
                    {
                        EndTransition();
                    }
                }
                return; // while transitioning, we don't run normal updates
            }
            _machine.InternalTick(deltaTime);
        }

        // Compute the Lowest Common Ancestor of two states.
        public static State LowestCommonAncestor(State a, State b)
        {
            // Create a set of all parents of 'a'
            HashSet<State> ap = new HashSet<State>();
            for (State s = a; s != null; s = s.Parent) ap.Add(s);

            // Find the first parent of 'b' that is also a parent of 'a'
            for (State s = b; s != null; s = s.Parent)
                if (ap.Contains(s))
                    return s;

            // If no common ancestor found, return null
            return null;
        }
    }
}
