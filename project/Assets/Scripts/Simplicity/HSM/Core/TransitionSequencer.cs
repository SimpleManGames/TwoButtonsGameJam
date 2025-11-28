namespace HSM
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using HSM.Enums;
    using HSM.Interfaces;

    public class TransitionSequencer
    {
        public readonly bool useSequential = true;

        private readonly StateMachine _machine;
        
        private State _lastFrom, _lastTo;

        private Action _nextPhase;

        private (State from, State to)? _pending;

        private ISequence _sequencer;

        private CancellationTokenSource _cts;

        public TransitionSequencer(StateMachine machine)
        {
            _machine = machine;
            _cts = new CancellationTokenSource();
        }

        public void RequestTransition(State from, State to)
        {
            if (to == null || from == to)
                return;

            if (_sequencer != null)
            {
                _pending = (from, to);
                return;
            }

            BeginTransition(from, to);
        }

        public void Tick(float deltaTime)
        {
            if (_sequencer != null)
            {
                if (!_sequencer.Update())
                    return;

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
                return;
            }

            _machine.InternalTick(deltaTime);
        }

        public static State LowestCommonAncestor(State a, State b)
        {
            HashSet<State> ap = new HashSet<State>();

            for (State s = a; s != null; s = s.parent)
            {
                ap.Add(s);
            }

            for (State s = b; s != null; s = s.parent)
            {
                if (ap.Contains(s))
                    return s;
            }

            return null;
        }

        private List<PhaseStep> GatherPhaseSteps(List<State> chain, bool deactivate)
        {
            List<PhaseStep> steps = new List<PhaseStep>();

            foreach (State t in chain)
            {
                IReadOnlyList<IActivity> acts = t.Activities;

                foreach (IActivity a in acts)
                {
                    if (deactivate)
                    {
                        if (a.Mode == ActivityMode.Active)
                            steps.Add(ct => a.DeactivateAsync(ct));
                    }
                    else
                    {
                        if (a.Mode == ActivityMode.Inactive)
                            steps.Add(ct => a.ActivateAsync(ct));
                    }
                }
            }

            return steps;
        }

        private void BeginTransition(State from, State to)
        {
            State lca = LowestCommonAncestor(from, to);
            List<State> exitChain = StatesToExit(from, lca);
            List<State> enterChain = StatesToEnter(to, lca);

            List<PhaseStep> exitSteps = GatherPhaseSteps(exitChain, deactivate: true);

            _sequencer = useSequential ? new SequentialPhase(exitSteps, _cts.Token) : new ParallelPhase(exitSteps, _cts.Token);
            _sequencer.Start();

            _nextPhase = () =>
            {
                _machine.ChangeState(from, to);
                List<PhaseStep> enterSteps = GatherPhaseSteps(enterChain, deactivate: false);
                _sequencer = useSequential ? new SequentialPhase(enterSteps, _cts.Token) : new ParallelPhase(enterSteps, _cts.Token);
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

        private static List<State> StatesToEnter(State to, State lca)
        {
            Stack<State> stack = new Stack<State>();

            for (State s = to; s != lca; s = s.parent)
                stack.Push(s);

            return new List<State>(stack);
        }

        private static List<State> StatesToExit(State from, State lca)
        {
            List<State> list = new List<State>();

            for (State s = from; s != null && s != lca; s = s.parent)
                list.Add(s);

            return list;
        }
    }
}
