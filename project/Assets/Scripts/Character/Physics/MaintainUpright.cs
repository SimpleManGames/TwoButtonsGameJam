namespace Character
{
    using UnityEngine;

    using VContainer.Unity;

    public sealed class MaintainUpright : IFixedTickable
    {
        private readonly Rigidbody2D _rigidbody;
        private readonly Transform _transform;

        public MaintainUpright(Rigidbody2D rigidbody, Transform transform)
        {
            _rigidbody = rigidbody;
            _transform = transform;
        }
        
        public void FixedTick()
        {
            if (_transform == null || _rigidbody == null) return;
            
            Quaternion currentRotation = _transform.rotation;
            
        }
    }
}
