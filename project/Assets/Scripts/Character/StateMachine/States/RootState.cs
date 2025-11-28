namespace Character
{
    using HSM;

    public sealed class RootState : State
    {
        public GroundedState GroundedState { get; private set; }
        
        public RootState(GroundedState groundedState)
        {
            GroundedState = groundedState;
        }

        protected override State GetInitialState() => GroundedState;
    }

    public sealed class GroundedState : State
    {
        public IdleState IdleState { get; private set; }
        
        public MoveState MoveState { get; private set; }

        public GroundedState(IdleState idleState, MoveState moveState)
        {
            IdleState = idleState;
            MoveState = moveState;
        }
    }

    public sealed class IdleState : State
    {
        
    }

    public sealed class MoveState : State
    {
        
    }

    public sealed class AirborneState : State
    {
        public JumpState JumpState { get; private set; }
        
        public FallingState FallingState { get; private set; }
        
        public AirborneState(JumpState jumpState, FallingState fallingState)
        {
            JumpState = jumpState;
            FallingState = fallingState;
        }
    }

    public sealed class JumpState : State
    {
        
    }

    public sealed class FallingState : State
    {
        
    }
}
