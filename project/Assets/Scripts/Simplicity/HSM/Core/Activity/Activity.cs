namespace HSM
{
    using System.Threading;
    using System.Threading.Tasks;

    using HSM.Enums;
    using HSM.Interfaces;

    public abstract class Activity : IActivity
    {
        public ActivityMode Mode { get; protected set; } = ActivityMode.Inactive;

        public async virtual Task ActivateAsync(CancellationToken ct)
        {
            if (Mode != ActivityMode.Inactive)
                return;
            
            Mode = ActivityMode.Activating;
            await Task.CompletedTask;
            Mode = ActivityMode.Active;
        }

        public async virtual Task DeactivateAsync(CancellationToken ct)
        {
            if (Mode != ActivityMode.Active)
                return;

            Mode = ActivityMode.Deactivating;
            await Task.CompletedTask;
            Mode = ActivityMode.Inactive;
        }
    }
}
