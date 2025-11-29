namespace Character
{
    using Character.Settings;

    using HSM;

    public sealed class AirborneState : State
    {
        public JumpState JumpState { get; private set; }
        
        public FallingState FallingState { get; private set; }
        
        private float _currentReacquireGroundLockoutTime;
        
        private readonly CharacterContext _context;
        private readonly ApplyMovementForce _movement;
        private readonly CharacterSettings _settings;

        private const float GROUNDED_LOCKOUT_TIME = 0.1f;
        
        public AirborneState(JumpState jumpState, FallingState fallingState, CharacterContext context, ApplyMovementForce movement, CharacterSettings settings)
        {
            _context = context;
            _movement = movement;
            _settings = settings;
            JumpState = jumpState;
            FallingState = fallingState;
        }

        protected override State GetTransition()
        {
            if(!_context.IsGrounded)
                return FallingState;

            bool isLockedOutFromGrounded = _currentReacquireGroundLockoutTime < GROUNDED_LOCKOUT_TIME;

            if (_context.IsGrounded && !isLockedOutFromGrounded)
                return Ancestor<RootState>().GroundedState;
            
            return null;
        }
        
        protected override void OnEnter()
        {
            _context.IsGrounded = false;
        }
        
        protected override void OnUpdate(float deltaTime)
        {
            _context.ElapsedAirtime += deltaTime;
            _currentReacquireGroundLockoutTime += deltaTime;
        }

        protected override void OnFixedUpdate()
        {
            _movement.Move(_context.MoveDirection, _settings.AirborneSettings);
        }

        protected override void OnExit()
        {
            _context.ElapsedAirtime = 0f;
            _currentReacquireGroundLockoutTime = 0f;
        }
    }
}
