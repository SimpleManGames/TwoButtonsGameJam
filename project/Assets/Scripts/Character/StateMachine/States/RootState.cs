namespace Character
{

    using HSM;

    using UnityEngine;

    public sealed class RootState : State
    {
        private readonly CharacterContext _context;
        private readonly SpriteRenderer _spriteRenderer;

        public GroundedState GroundedState { get; private set; }
        
        public AirborneState AirborneState { get; private set; }
        
        public RootState(GroundedState groundedState, AirborneState airborneState, CharacterContext context, SpriteRenderer spriteRenderer)
        {
            _context = context;
            _spriteRenderer = spriteRenderer;
            AirborneState = airborneState;
            GroundedState = groundedState;

            _context.JumpInputElapsed = float.MaxValue / 2f;
        }

        protected override State GetInitialState() => GroundedState;
        
        protected override void OnUpdate(float deltaTime)
        {
            _context.JumpInputElapsed += deltaTime;
            if(_context.MoveDirection != Vector2.zero)
                _spriteRenderer.flipX = _context.MoveDirection.x < 0;
        }
        
        protected override State GetTransition()
        {
            if (!_context.IsGrounded)
                return AirborneState.FallingState;

            return null;
        }
    }

}
