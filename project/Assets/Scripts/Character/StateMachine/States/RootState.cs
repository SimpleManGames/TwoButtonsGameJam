namespace Character
{
    using Character.Settings;

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

            _context.JumpInputElapsed = float.MaxValue / 2f;
        }

        protected override State GetInitialState() => GroundedState;
        
        protected override void OnUpdate(float deltaTime)
        {
            _context.JumpInputElapsed += deltaTime;
        }
        
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

            bool canJump = _context.IsGrounded || (_context.ElapsedAirtime <= _settings.CoyoteTime) &&
                _context.GroundedAngle <= _settings.MaxJumpAngle;

            if (attemptingJump && canJump)
            {
                Debug.Log($"here {_context.IsGrounded} {_context.ElapsedAirtime <= _settings.CoyoteTime}");
                
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

    public sealed class IdleState : State
    {
        private readonly Animator _animator;
        private readonly CharacterContext _context;

        public IdleState(Animator animator, CharacterContext context)
        {
            _animator = animator;
            _context = context;
            // Add(new PlayAnimationActivity(characterAnimator, "Idle"));
        }

        protected override void OnEnter()
        {
            _animator.Play("Idle");
        }

        protected override State GetTransition()
        {
            return _context.MoveDirection != Vector2.zero ? Ancestor<GroundedState>().MoveState : null;
        }
    }

    public sealed class MoveState : State
    {
        private readonly Animator _animator;
        private readonly CharacterContext _context;

        public MoveState(Animator animator, CharacterContext context)
        {
            _animator = animator;
            _context = context;
            // Add(new PlayAnimationActivity(characterAnimator, "Walk"));
        }

        protected override void OnEnter()
        {
            _animator.Play("Walk");
        }

        protected override State GetTransition()
        {
            return _context.MoveDirection == Vector2.zero ? Ancestor<GroundedState>().IdleState : null;
        }
    }

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

    public sealed class JumpState : State
    {
        private readonly CharacterContext _context;
        private readonly Rigidbody2D _rigidbody;
        private readonly ApplyJumpForce _jumpForce;
        private readonly Animator _animator;

        public JumpState(CharacterContext context, Rigidbody2D rigidbody, ApplyJumpForce jumpForce, Animator animator)
        {
            _context = context;
            _rigidbody = rigidbody;
            _jumpForce = jumpForce;
            _animator = animator;
        }
        
        protected override State GetTransition()
        {
            if (!_context.JumpPressed || _rigidbody.linearVelocityY < 0)
                return Ancestor<AirborneState>().FallingState;

            return null;
        }

        protected override void OnEnter()
        {
            _jumpForce.Jump();
            _animator.Play("Jump Squat");
        }
    }

    public sealed class FallingState : State
    {
        private readonly CharacterSettings _settings;
        private readonly Rigidbody2D _rigidbody;
        private readonly Animator _animator;
        private float _currentFallTime;

        public FallingState(CharacterSettings settings, Rigidbody2D rigidbody, Animator animator)
        {
            _settings = settings;
            _rigidbody = rigidbody;
            _animator = animator;
        }
        
        protected override void OnEnter()
        {
            _currentFallTime = 0f;
            _animator.Play("Jump Land Prediction");
        }

        protected override void OnUpdate(float deltaTime)
        {
            _currentFallTime += deltaTime;
        }

        protected override void OnFixedUpdate()
        {
            float adjustedFallSpeedForFallTime = _settings.BaseFallSpeed * _settings.FallAirtimeSpeedMultiplierCurve.Evaluate(_currentFallTime);
            _rigidbody.AddForce(Vector3.down * Mathf.Abs(Physics2D.gravity.y) * adjustedFallSpeedForFallTime, ForceMode2D.Force);
        }   
    }
}
