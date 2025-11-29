namespace Character
{
    using HSM;

    using UnityEngine;

    public sealed class MoveState : State
    {
        private readonly Animator _animator;
        private readonly CharacterContext _context;

        public MoveState(Animator animator, CharacterContext context)
        {
            _animator = animator;
            _context = context;
            // Add(new PlayAnimationActivity(characterAnimator, "Walk"));
        }

        protected override void OnEnter()
        {
            _animator.Play("Walk");
        }

        protected override State GetTransition()
        {
            return _context.MoveDirection == Vector2.zero ? Ancestor<GroundedState>().IdleState : null;
        }
    }
}
