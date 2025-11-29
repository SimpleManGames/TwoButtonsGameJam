namespace Character.Settings
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "Character Settings", menuName = "Game/Character/Settings")]
    public class CharacterSettings : ScriptableObject
    {
        [field: Header("Float Settings")]
        [field: SerializeField]
        public float FloatColliderRaycastLength { get; private set; } = 1f;

        [field: SerializeField]
        public float RideHeight { get; private set; } = 0.75f;

        [field: SerializeField]
        public float SpringForce { get; private set; } = 100f;

        [field: SerializeField]
        public float SpringDamper { get; private set; } = 100f;

        [field: Header("Ground Check Settings")]
        [field: SerializeField]
        public float GroundDistanceCheckDistance { get; private set; } = 1f;

        [field: SerializeField]
        public float GroundCheckRadius { get; private set; } = 0.1f;

        [field: Header("Collider Settings")]
        [field: SerializeField]
        public float ColliderHeight { get; private set; } = 2f;

        [field: SerializeField]
        public float StepHeightRatio { get; private set; } = 0.2f;

        [field: SerializeField]
        public float Thickness { get; private set; } = 1f;

        [field: SerializeField]
        public Vector2 AdditionalOffset { get; private set; } = Vector2.zero;
        
        
        [field: Header("Collider Settings")]
        [field: SerializeField]
        public float UprightSpringStrength { get; private set; }
        
        [field: SerializeField]
        public float UprightSpringDamping { get; private set; }
    }
}
