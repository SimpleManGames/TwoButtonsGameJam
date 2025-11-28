namespace HSM
{
    using System.Collections.Generic;

    using HSM.Interfaces;

    public abstract class State
    {
        public IReadOnlyList<IActivity> Activities => _activities;

        public StateMachine Machine => machine;

        public State Parent => parent;

        public State activeChild;

        public readonly StateMachine machine;

        public readonly State parent;

        private readonly List<IActivity> _activities = new List<IActivity>();

        protected State()
        {
            
        }
        
        protected State(StateMachine machine, State parent = null)
        {
            this.machine = machine;
            this.parent = parent;
        }

        protected virtual State GetInitialState()
        {
            return null;
        }

        protected virtual State GetTransition()
        {
            return null;
        }

        protected virtual void OnEnter() { }

        protected virtual void OnExit() { }

        protected virtual void OnUpdate(float deltaTime) { }
        
        protected virtual void OnFixedUpdate() { }

        public void Add(IActivity activity)
        {
            if (activity != null)
            {
                _activities.Add(activity);
            }
        }

        public State Leaf()
        {
            State s = this;

            while (s.activeChild != null)
            {
                s = s.activeChild;
            }

            return s;
        }

        public IEnumerable<State> PathToRoot()
        {
            for (State s = this; s != null; s = s.parent)
            {
                yield return s;
            }
        }

        protected T Ancestor<T>() where T : State
        {
            foreach (State s in PathToRoot())
            {
                if (s is not T ancestor)
                {
                    continue;
                }
                
                return ancestor;
            }

            return null;
        }

        internal void Enter()
        {
            if (parent != null)
            {
                parent.activeChild = this;
            }

            OnEnter();

            State init = GetInitialState();
            init?.Enter();
        }

        internal void Exit()
        {
            activeChild?.Exit();
            activeChild = null;
            OnExit();
        }

        internal void Update(float deltaTime)
        {
            State t = GetTransition();

            if (t != null)
            {
                machine.sequencer.RequestTransition(this, t);
            }

            activeChild?.Update(deltaTime);
            OnUpdate(deltaTime);
        }

        internal void FixedUpdate(float fixedDeltaTime)
        {
            activeChild?.FixedUpdate(fixedDeltaTime);
            OnFixedUpdate();
        }
    }
}
