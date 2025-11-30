namespace Character
{
    using UnityEngine;

    public class CharacterContext
    {
        public bool IsGrounded { get; set; }
        
        public float GroundedAngle { get; set; }

        public Transform GroundTransform { get; set; }
        
        public Vector2 LookDirection { get; set; } = Vector2.right;
        
        public Vector2 MoveDirection { get; set; } = Vector2.zero;

        public float ElapsedAirtime { get; set; }

        public bool JumpPressed { get; set; }

        public float JumpInputElapsed { get; set; }

        public Vector2 GroundWorldPoint { get; set; }
    }
}
