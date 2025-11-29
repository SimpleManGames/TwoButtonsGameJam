namespace Character
{
    using Character.Settings;

    using UnityEngine;

    public sealed class FloatRigidbody
    {
        private readonly Transform _transform;
        private readonly Rigidbody2D _rigidbody2D;
        private readonly CharacterSettings _settings;

        public FloatRigidbody(Transform transform, Rigidbody2D rigidbody2D, CharacterSettings settings)
        {
            _transform = transform;
            _rigidbody2D = rigidbody2D;
            _settings = settings;
        }
        
        public void ApplyForceToFloat()
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(_transform.position, Vector2.down, _settings.FloatColliderRaycastLength);
            
            if (hitInfo.collider == null)
                return;
                
            Vector2 velocity = _rigidbody2D.linearVelocity;
            Vector2 direction = Vector2.down;

            Vector2 otherVelocity = Vector2.zero;
            Rigidbody2D otherRigidbody2D = hitInfo.rigidbody;
            
            if(otherRigidbody2D != null)
                otherVelocity = otherRigidbody2D.linearVelocity;
            
            float rayDirectionVelocity = Vector2.Dot(direction, velocity);
            float otherDirectionVelocity = Vector2.Dot(direction, otherVelocity);
            
            float relativeVelocity = rayDirectionVelocity - otherDirectionVelocity;
            float x = hitInfo.distance - _settings.RideHeight;

            float springForce = (x * _settings.SpringForce) - (relativeVelocity * _settings.SpringDamper);
            _rigidbody2D.AddForce(direction * springForce, ForceMode2D.Force);
            
            if(otherRigidbody2D != null)
                otherRigidbody2D.AddForceAtPosition(otherVelocity * -springForce, hitInfo.point, ForceMode2D.Force);
        }
    }
}
