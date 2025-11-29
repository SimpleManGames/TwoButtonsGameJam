namespace Character
{
    using Character.Settings;

    using HSM;

    using UnityEngine;

    public sealed class AirborneState : State
    {
        public JumpState JumpState { get; private set; }
        
        public FallingState FallingState { get; private set; }
        
        private readonly CharacterContext _context;
        private readonly ApplyMovementForce _movement;
        private readonly CharacterSettings _settings;
        private readonly Rigidbody2D _rigidbody;

        public AirborneState(JumpState jumpState, FallingState fallingState, CharacterContext context, ApplyMovementForce movement, CharacterSettings settings, Rigidbody2D rigidbody)
        {
            _context = context;
            _movement = movement;
            _settings = settings;
            _rigidbody = rigidbody;
            JumpState = jumpState;
            FallingState = fallingState;
        }

        protected override State GetTransition()
        {
            if(!_context.IsGrounded && _rigidbody.linearVelocityY < 0f)
                return FallingState;
            
            return null;
        }
        
        protected override void OnEnter()
        {
            _context.IsGrounded = false;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _context.ElapsedAirtime += deltaTime;
        }

        protected override void OnFixedUpdate()
        {
            _movement.Move(_context.MoveDirection, _settings.AirborneSettings);
        }

        protected override void OnExit()
        {
            _context.ElapsedAirtime = 0f;
        }
    }
}
