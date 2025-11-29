namespace HSM
{
    using System.Threading;
    using System.Threading.Tasks;

    using HSM.Enums;

    using UnityEngine;

    public class PlayAnimationActivity : Activity
    {
        private readonly Animator _animator;
        private readonly string _stateName;

        public PlayAnimationActivity(Animator animator, string stateName)
        {
            _animator = animator;
            _stateName = stateName;
        }

        public async override Task ActivateAsync(CancellationToken ct)
        {
            if (Mode != ActivityMode.Inactive || _animator == null)
                return;

            Mode = ActivityMode.Activating;
            _animator.Play(_stateName);
            Mode = ActivityMode.Active;
        }

        public async override Task DeactivateAsync(CancellationToken ct)
        {
            Mode = ActivityMode.Inactive;
        }
    }
}
