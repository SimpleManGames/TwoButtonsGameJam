namespace HSM
{
    using System.Collections.Generic;

    using UnityEngine;

    public class StateMachine
    {
        public readonly State root;

        public readonly TransitionSequencer sequencer;

        private bool _started = false;
        
        public StateMachine(State root)
        {
            this.root = root;
            sequencer = new TransitionSequencer(this);
        }
        
        public void Start()
        {
            if (_started)
                return;

            _started = true;
            root.Enter();
        }

        public void Tick(float deltaTime)
        {
            if (!_started)
                Start();

            sequencer.Tick(deltaTime);
        }

        public void FixedTick(float fixedDeltaTime)
        {
            if(!_started)
                Start();
            
            InternalFixedTick(fixedDeltaTime);
        }

        public void ChangeState(State from, State to)
        {
            if (from == to || from == null || to == null)
                return;

            State lca = TransitionSequencer.LowestCommonAncestor(from, to);

            for (State s = from; s != lca; s = s.parent)
            {
                // Debug.Log($"Exiting {s.GetType().Name}");
                s.Exit();
            }

            Stack<State> stack = new Stack<State>();

            for (State s = to; s != lca; s = s.parent)
            {
                stack.Push(s);
            }

            while (stack.Count > 0)
            {
                State entering = stack.Pop();
                // Debug.Log($"Entering {entering.GetType().Name}");
                entering.Enter();
            }
        }

        internal void InternalTick(float deltaTime)
        {
            root.Update(deltaTime);
        }

        internal void InternalFixedTick(float fixedDeltaTime)
        {
            root.FixedUpdate(fixedDeltaTime);
        }
    }
}
