namespace Character
{
    using VContainer;
    using VContainer.Unity;

    using HSM;

    using UnityEngine;
    
    public class CharacterLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private new Rigidbody2D rigidbody2D;

        [SerializeField]
        private new CircleCollider2D collider2D;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<CharacterContext>(Lifetime.Scoped);
            
            builder.RegisterInstance(transform).As<Transform>();
            builder.RegisterInstance(rigidbody2D).As<Rigidbody2D>();
            builder.RegisterInstance(collider2D).As<CircleCollider2D>();

            builder.UseEntryPoints(config =>
            {
                config.Add<CheckGrounded>();
                config.Add<CharacterStateMachineHandler>();
            });

            builder.Register<RootState>(Lifetime.Scoped);
            builder.Register<GroundedState>(Lifetime.Scoped);
            builder.Register<IdleState>(Lifetime.Scoped);
            builder.Register<MoveState>(Lifetime.Scoped);

            builder.Register<AirborneState>(Lifetime.Scoped);
            builder.Register<JumpState>(Lifetime.Scoped);
            builder.Register<FallingState>(Lifetime.Scoped);

            builder.Register(resolver =>
            {
                RootState root = resolver.Resolve<RootState>();
                StateMachineBuilder stateMachineBuilder = new StateMachineBuilder(root);
                StateMachine stateMachine = stateMachineBuilder.Build();
                resolver.Inject(stateMachine);
                return stateMachine;
            }, Lifetime.Scoped);
        }
    }
}
