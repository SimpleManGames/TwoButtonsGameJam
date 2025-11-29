namespace Character
{
    using Character.Settings;

    using VContainer;
    using VContainer.Unity;

    using HSM;

    using UnityEngine;
    
    public class CharacterLifetimeScope : LifetimeScope
    {
        [SerializeField]
        private new Rigidbody2D rigidbody2D;

        [SerializeField]
        private new CapsuleCollider2D collider2D;

        [SerializeField]
        private Animator animator;
        
        [SerializeField]
        private CharacterSettings settings;
        
        [SerializeField]
        private SpriteRenderer spriteRenderer;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(settings).As<CharacterSettings>();
            builder.Register<CharacterContext>(Lifetime.Singleton);
            
            builder.RegisterInstance(transform).As<Transform>();
            builder.RegisterInstance(rigidbody2D).As<Rigidbody2D>();
            builder.RegisterInstance(collider2D).As<CapsuleCollider2D>();
            builder.RegisterInstance(animator).As<Animator>();
            builder.RegisterInstance(spriteRenderer).As<SpriteRenderer>();
                
            builder.UseEntryPoints(config =>
            {
                config.Add<HandleColliderSize>();
                config.Add<CheckGrounded>();
                // config.Add<MaintainUpright>(); // This behaves weirdly in 2D
                config.Add<CharacterStateMachineHandler>();
            });
            
            builder.Register<FloatRigidbody>(Lifetime.Scoped);
            builder.Register<ApplyMovementForce>(Lifetime.Scoped);
            builder.Register<ApplyJumpForce>(Lifetime.Scoped);

            builder.Register<RootState>(Lifetime.Singleton);
            builder.Register<GroundedState>(Lifetime.Singleton);
            builder.Register<IdleState>(Lifetime.Singleton);
            builder.Register<MoveState>(Lifetime.Singleton);

            builder.Register<AirborneState>(Lifetime.Singleton);
            builder.Register<JumpState>(Lifetime.Singleton);
            builder.Register<FallingState>(Lifetime.Singleton);

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
