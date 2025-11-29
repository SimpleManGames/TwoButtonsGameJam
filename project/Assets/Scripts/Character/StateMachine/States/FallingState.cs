namespace Character
{
    using Character.Settings;

    using HSM;

    using UnityEngine;

    public sealed class FallingState : State
    {
        private readonly CharacterSettings _settings;
        private readonly Rigidbody2D _rigidbody;
        private readonly Animator _animator;
        private readonly CharacterContext _context;
        private float _currentFallTime;

        public FallingState(CharacterSettings settings, Rigidbody2D rigidbody, Animator animator, CharacterContext context)
        {
            _settings = settings;
            _rigidbody = rigidbody;
            _animator = animator;
            _context = context;
        }

        protected override State GetTransition()
        {
            if(_context.IsGrounded)
                return Ancestor<RootState>().GroundedState;
            
            return null;
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
