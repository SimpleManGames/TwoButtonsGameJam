namespace Character
{
    using UnityEngine;

    public class CharacterContext
    {
        public bool IsGrounded { get; set; }
        
        public float Angle { get; set; }

        public Transform GroundTransform { get; set; }
        
        public Vector2 LookDirection { get; set; } = Vector2.right;
    }
}
