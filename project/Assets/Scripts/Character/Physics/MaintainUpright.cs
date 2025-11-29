namespace Character
{
    using Character.Settings;

    using UnityEngine;

    using VContainer.Unity;

    public sealed class MaintainUpright : IFixedTickable
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;
        private readonly CharacterContext _context;
        private readonly CharacterSettings _settings;

        public MaintainUpright(Rigidbody2D rigidbody, Transform transform, CharacterContext context, CharacterSettings settings)
        {
            _rigidbody = rigidbody;
            _transform = transform;
            _context = context;
            _settings = settings;
        }
        
        public void FixedTick()
        {
            if (_transform == null || _rigidbody == null) return;
            
            Quaternion currentRotation = _transform.rotation;
            Vector2 targetDirection = _context.LookDirection.normalized;

            if (targetDirection.sqrMagnitude < 0.00001f)
                return;
            
            Quaternion desiredRotation = Quaternion.LookRotation(targetDirection);
            Quaternion deltaRotation = desiredRotation * Quaternion.Inverse(currentRotation);

            if (deltaRotation.w < 0.1f)
            {
                deltaRotation.x = -deltaRotation.x;
                deltaRotation.y = -deltaRotation.y;
                deltaRotation.z = -deltaRotation.z;
                deltaRotation.w = -deltaRotation.w;
            }
            
            deltaRotation.ToAngleAxis(out float angleDeg, out Vector3 axis);
            
            if (float.IsNaN(axis.x) || float.IsNaN(axis.y) || float.IsNaN(axis.z))
                return;
            
            axis.Normalize();

            float angleRad = angleDeg * Mathf.Deg2Rad;
            Vector3 torque = axis * (angleRad * _settings.UprightSpringStrength) - new Vector3(_rigidbody.angularVelocity,0.0f, 0.0f) * _settings.UprightSpringDamping;

            _rigidbody.AddTorque(torque.z);
        }
    }
}
