namespace Character
{
    using HSM;

    using UnityEngine;

    public sealed class IdleState : State
    {
        private readonly Animator _animator;
        private readonly CharacterContext _context;

        public IdleState(Animator animator, CharacterContext context)
        {
            _animator = animator;
            _context = context;
            Add(new PlayAnimationActivity(animator, "Idle", true));
        }

        protected override void OnEnter()
        {
            // _animator.Play("Idle");
        }

        protected override State GetTransition()
        {
            return _context.MoveDirection != Vector2.zero ? Ancestor<GroundedState>().MoveState : null;
        }
    }
}
