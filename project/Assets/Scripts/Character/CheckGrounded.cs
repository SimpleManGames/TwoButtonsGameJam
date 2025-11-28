namespace Character
{
    using UnityEngine;

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
        private readonly CapsuleCollider2D _collider;
        private readonly CharacterContext _context;

        // TODO: Move these to be customizable
        private const float LENGTH = 1f;
        private const float RADIUS = 0.1f;

        public CheckGrounded(Transform transform, CapsuleCollider2D collider, CharacterContext context)
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
            RaycastHit2D hit = Physics2D.CircleCast(raycastPosition, RADIUS, Vector2.down, LENGTH);
            
            return new GatheredGroundInfo(hit.transform != null, Vector2.Angle(hit.normal, Vector2.up), hit.transform);
        }
    }
}
