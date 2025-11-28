using HSM;

using UnityEngine;

using VContainer.Unity;

namespace Character
{
    public sealed class CharacterStateMachineHandler : IStartable, ITickable, IFixedTickable
    {
        private readonly StateMachine _machine;

        public CharacterStateMachineHandler(StateMachine machine)
        {
            _machine = machine;
        }
        
        public void Start()
        {
            _machine.Start();
        }

        public void Tick()
        {
            _machine.Tick(Time.deltaTime);
        }

        public void FixedTick()
        {
            _machine.FixedTick(Time.fixedDeltaTime);
        }
    }
}
