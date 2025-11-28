namespace Character
{
    using Character.Settings;

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
        private readonly CharacterSettings _settings;

        public CheckGrounded(Transform transform, CapsuleCollider2D collider, CharacterContext context, CharacterSettings settings)
        {
            _transform = transform;
            _collider = collider;
            _context = context;
            _settings = settings;
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
            RaycastHit2D hit = Physics2D.CircleCast(raycastPosition, _settings.GroundCheckRadius, Vector2.down, _settings.GroundDistanceCheckDistance);
            Debug.DrawRay(raycastPosition, Vector2.down, Color.blue);
            
            return new GatheredGroundInfo(hit.transform != null, Vector2.Angle(hit.normal, Vector2.up), hit.transform);
        }
    }
}
