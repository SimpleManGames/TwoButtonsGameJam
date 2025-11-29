namespace Game
{
    using VContainer;
    using VContainer.Unity;
    
    using Character;

    public class PlayerLifetimeScope : LifetimeScope
    {
        protected override LifetimeScope FindParent() => GetComponent<CharacterLifetimeScope>();

        protected override void Configure(IContainerBuilder builder)
        {
            builder.UseEntryPoints(config =>
            {
                config.Add<PlayerCharacterInput>();
            });
        }
    }
}
