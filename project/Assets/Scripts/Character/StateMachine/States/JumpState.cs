namespace Character
{
    using HSM;

    using UnityEngine;

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
}
