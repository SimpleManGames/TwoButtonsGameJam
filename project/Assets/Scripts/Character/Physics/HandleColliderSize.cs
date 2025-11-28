namespace Character
{
    using Character.Settings;

    using UnityEngine;

    using VContainer.Unity;

    public sealed class HandleColliderSize : IStartable
    {
        private readonly CapsuleCollider2D _collider2D;
        private readonly CharacterSettings _settings;

        public HandleColliderSize(CapsuleCollider2D collider2D, CharacterSettings settings)
        {
            _collider2D = collider2D;
            _settings = settings;
        }
        
        public void Start()
        {
            if (_collider2D == null) 
                return;

            _collider2D.size = new Vector2(_settings.Thickness, _settings.ColliderHeight * (1f - _settings.StepHeightRatio));
            _collider2D.offset = _settings.AdditionalOffset * _settings.ColliderHeight + new  Vector2(0f,  _settings.StepHeightRatio * _settings.ColliderHeight / 2f);
        }
    }
}
