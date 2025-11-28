namespace Character
{
    using UnityEngine;

    using VContainer;
    using VContainer.Unity;

    public struct GatheredGroundInfo
    {
        public readonly bool isGrounded;
        public readonly float groundedAngle;
        public readonly Transform groundTransform;

        public GatheredGroundInfo(bool isGrounded, float groundedAngle, Transform groundTransform)
        {
            this.isGrounded = isGrounded;
            this.groundedAngle = groundedAngle;
            this.groundTransform = groundTransform;
        }
    }

    public class CheckGrounded : IFixedTickable
    {
        private readonly Transform _transform;
        private readonly CircleCollider2D _collider;
        private readonly CharacterContext _context;

        private float length = 1f;
        
        public CheckGrounded(Transform transform, CircleCollider2D collider, CharacterContext context)
        {
            _transform = transform;
            _collider = collider;
            _context = context;
        }

        public void FixedTick()
        {
            GatheredGroundInfo info = RaycastForCheckGroundedInfo();
            
            _context.IsGrounded = info.isGrounded;
            _context.Angle = info.groundedAngle;
            _context.GroundTransform = info.groundTransform;
        }

        private GatheredGroundInfo RaycastForCheckGroundedInfo()
        {
            Vector2 raycastPosition = new Vector2 (_transform.position.x, _transform.position.y) + _collider.offset;
            RaycastHit2D hit = Physics2D.CircleCast(raycastPosition, _collider.radius, Vector2.down, length);
            
            return new GatheredGroundInfo(hit.transform != null, Vector2.Angle(hit.normal, Vector2.up), hit.transform);
        }
    }
}
