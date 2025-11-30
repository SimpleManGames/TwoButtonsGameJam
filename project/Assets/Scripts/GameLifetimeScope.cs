namespace Game
{
    using UnityEngine;

    using VContainer;
    using VContainer.Unity;

    public class GameLifetimeScope : LifetimeScope
    {
        private Interactable[] _interactableArray;
        
        protected override void Awake()
        {
            _interactableArray = FindObjectsByType<Interactable>( FindObjectsInactive.Include, FindObjectsSortMode.None);
            base.Awake();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_interactableArray);
            builder.Register<TwoButtonsInputAction>(Lifetime.Singleton);
        }
    }
}