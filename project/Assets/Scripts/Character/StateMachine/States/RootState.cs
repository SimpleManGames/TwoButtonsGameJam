namespace Character
{
    using HSM;

    using UnityEngine;

    public sealed class RootState : State
    {
        private readonly CharacterContext _context;

        public GroundedState GroundedState { get; private set; }
        
        public AirborneState AirborneState { get; private set; }
        
        public RootState(GroundedState groundedState, AirborneState airborneState, CharacterContext context)
        {
            _context = context;
            AirborneState = airborneState;
            GroundedState = groundedState;
        }

        protected override State GetInitialState() => GroundedState;

        protected override State GetTransition()
        {
            if (!_context.IsGrounded)
                return AirborneState.FallingState;

            return null;
        }
    }

    public sealed class GroundedState : State
    {
        private readonly FloatRigidbody _floatRigidbody;
        private readonly CharacterContext _context;
        private readonly ApplyMovementForce _movement;

        public IdleState IdleState { get; private set; }
        
        public MoveState MoveState { get; private set; }

        private bool _enabledFloat = false;
        
        public GroundedState(IdleState idleState, MoveState moveState, FloatRigidbody floatRigidbody, CharacterContext context, ApplyMovementForce movement)
        {
            _floatRigidbody = floatRigidbody;
            _context = context;
            _movement = movement;
            IdleState = idleState;
            MoveState = moveState;
        }

        protected override State GetTransition()
        {
            if (_context.MoveDirection != Vector2.zero)
                return MoveState;

            return IdleState;
        }

        protected override void OnEnter()
        {
            _enabledFloat = true;
        }

        protected override void OnFixedUpdate()
        {
            if(_enabledFloat)
                _floatRigidbody.ApplyForceToFloat();
            
            _movement.Move(_context.MoveDirection);
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
        private readonly CharacterContext _context;

        public JumpState JumpState { get; private set; }
        
        public FallingState FallingState { get; private set; }
        
        public AirborneState(JumpState jumpState, FallingState fallingState, CharacterContext context)
        {
            _context = context;
            JumpState = jumpState;
            FallingState = fallingState;
        }

        protected override State GetTransition()
        {
            if(_context.IsGrounded)
                return Ancestor<RootState>().GroundedState;

            return null;
        }
    }

    public sealed class JumpState : State
    {
        
    }

    public sealed class FallingState : State
    {
        
    }
}
