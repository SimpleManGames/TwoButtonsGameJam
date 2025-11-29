namespace Character
{
    using Character.Settings;

    using UnityEngine;

    public sealed class ApplyJumpForce
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly CharacterSettings _settings;
        
        private Vector2 _goalVelocity;

        public ApplyJumpForce(Rigidbody2D rigidbody, CharacterSettings settings)
        {
            _rigidbody = rigidbody;
            _settings = settings;
        }

        public void Jump()
        {
            _rigidbody.linearVelocityY = 0f;

            float gravity = Mathf.Abs(Physics2D.gravity.y);
            float jumpVelocity = Mathf.Sqrt(2 * gravity * _settings.JumpHeight);
            float jumpForce = _rigidbody.mass * jumpVelocity;
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
