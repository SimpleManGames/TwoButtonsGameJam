namespace Character
{
    using Character.Settings;

    using HSM;

    using UnityEngine;

    public sealed class GroundedState : State
    {
        private readonly FloatRigidbody _floatRigidbody;
        private readonly CharacterContext _context;
        private readonly ApplyMovementForce _movement;
        private readonly CharacterSettings _settings;

        public IdleState IdleState { get; private set; }
        
        public MoveState MoveState { get; private set; }

        private bool _enabledFloat = false;
        
        public GroundedState(IdleState idleState, MoveState moveState, FloatRigidbody floatRigidbody, CharacterContext context, ApplyMovementForce movement, CharacterSettings settings)
        {
            _floatRigidbody = floatRigidbody;
            _context = context;
            _movement = movement;
            _settings = settings;
            IdleState = idleState;
            MoveState = moveState;
        }
        
        protected override State GetInitialState() => IdleState;

        protected override State GetTransition()
        {
            bool attemptingJump = _context.JumpInputElapsed <= _settings.BufferTime;

            bool canJump = (_context.IsGrounded || _context.ElapsedAirtime <= _settings.CoyoteTime) &&
                _context.GroundedAngle <= _settings.MaxJumpAngle;

            if (attemptingJump && canJump)
            {
                _enabledFloat = false;
                return Ancestor<RootState>().AirborneState.JumpState;
            }
            
            if (_context.MoveDirection != Vector2.zero)
                return MoveState;
            
            return null;
        }

        protected override void OnEnter()
        {
            _enabledFloat = true;
        }

        protected override void OnFixedUpdate()
        {
            if(_enabledFloat)
                _floatRigidbody.ApplyForceToFloat();
            
            _movement.Move(_context.MoveDirection, _settings.WalkSettings);
        }
    }
}
