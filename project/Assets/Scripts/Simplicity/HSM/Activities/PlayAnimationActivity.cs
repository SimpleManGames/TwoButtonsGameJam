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
        private readonly bool _onEnter;

        public PlayAnimationActivity(Animator animator, string stateName, bool onEnter)
        {
            _animator = animator;
            _stateName = stateName;
            _onEnter = onEnter;
        }

        public async override Task ActivateAsync(CancellationToken ct)
        {
            if (Mode != ActivityMode.Inactive || _animator == null)
                return;

            Mode = ActivityMode.Activating;
            if(_onEnter)
                _animator.Play(_stateName);
            Mode = ActivityMode.Active;
        }

        public async override Task DeactivateAsync(CancellationToken ct)
        {
            if (Mode != ActivityMode.Active || _animator == null)
                return;
            
            Mode = ActivityMode.Deactivating;
            if(!_onEnter)
                _animator.Play(_stateName);
            
            Mode = ActivityMode.Inactive;
        }
    }
}
