namespace Character
{

    using HSM;

    public sealed class RootState : State
    {
        private readonly CharacterContext _context;

        public GroundedState GroundedState { get; private set; }
        
        public AirborneState AirborneState { get; private set; }
        
        public RootState(GroundedState groundedState, AirborneState airborneState, CharacterContext context)
        {
            _context = context;
            AirborneState = airborneState;
            GroundedState = groundedState;

            _context.JumpInputElapsed = float.MaxValue / 2f;
        }

        protected override State GetInitialState() => GroundedState;
        
        protected override void OnUpdate(float deltaTime)
        {
            _context.JumpInputElapsed += deltaTime;
        }
        
        protected override State GetTransition()
        {
            if (!_context.IsGrounded)
                return AirborneState.FallingState;

            return null;
        }
    }

}
