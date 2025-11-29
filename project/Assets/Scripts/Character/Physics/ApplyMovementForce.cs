namespace Character
{
    using Character.Settings;

    using UnityEngine;

    public sealed class ApplyMovementForce
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly CharacterSettings _settings;
        private Vector2 _goalVelocity;

        public ApplyMovementForce(Rigidbody2D rigidbody, CharacterSettings settings)
        {
            _rigidbody = rigidbody;
            _settings = settings;
        }

        public void Move(Vector2 moveAmount)
        {
            if (_rigidbody == null)
                return;
            
            Vector2 unitVelocity = _goalVelocity.normalized;
            float velocityDot = Vector2.Dot(moveAmount,unitVelocity);
            float acceleration = _settings.Acceleration * _settings.AccelerationFactorFromDot.Evaluate(velocityDot);
            
            Vector2 goal = moveAmount * _settings.MaxSpeed;
            _goalVelocity = Vector2.MoveTowards(_goalVelocity, goal, acceleration * Time.fixedDeltaTime);
            Vector2 neededAcceleration = (_goalVelocity - _rigidbody.linearVelocity) / Time.fixedDeltaTime;
            float maxAcceleration = _settings.MaxAccelerationForce * _settings.MaxAccelerationForceFactorFromDot.Evaluate(velocityDot);
            
            neededAcceleration = Vector2.ClampMagnitude(neededAcceleration, maxAcceleration);
            _rigidbody.AddForce(Vector2.Scale(neededAcceleration * _rigidbody.mass, Vector2.one), ForceMode2D.Force);
        }
    }
}
