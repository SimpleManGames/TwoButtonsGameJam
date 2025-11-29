namespace Character.Settings
{
    using System;

    using UnityEngine;

    [CreateAssetMenu(fileName = "Character Settings", menuName = "Game/Character/Settings")]
    public class CharacterSettings : ScriptableObject
    {
        [Serializable]
        public class MovementSettings
        {
            [field: SerializeField]
            public float MaxSpeed { get; private set; } = 10f;

            [field: SerializeField]
            public float Acceleration { get; private set; } = 20f;
        
            [field: SerializeField]
            public AnimationCurve AccelerationFactorFromDot { get; private set; }

            [field: SerializeField]
            public float MaxAccelerationForce { get; private set; } = 15f;
        
            [field: SerializeField]
            public AnimationCurve MaxAccelerationForceFactorFromDot { get; private set; }
        }
        
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
        
        [field: Header("Jump Settings")]
        [field: SerializeField]
        public float JumpHeight { get; private set; }
        
        [field: SerializeField]
        public float BufferTime { get; set; } = 0.2f;

        [field: SerializeField]
        public float CoyoteTime { get; set; } = 0.1f;

        [field: SerializeField]
        public float MaxJumpAngle { get; set; } = 45f;
        
        [field: Header("Walk Settings")]
        [field: SerializeField]
        public MovementSettings WalkSettings { get; private set; } = new MovementSettings();
        
        [field: Header("Airborne Settings")]
        [field: SerializeField]
        public MovementSettings AirborneSettings { get; private set; } = new MovementSettings();
        
        [field: SerializeField]
        public float BaseFallSpeed { get; set; }

        [field: SerializeField]
        public AnimationCurve FallAirtimeSpeedMultiplierCurve { get; set; }

        [field: SerializeField]
        public float AcquireGroundLockoutTime { get; set; }
    }
}
