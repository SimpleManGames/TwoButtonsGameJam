namespace HSM.Interfaces
{
    using System.Threading;
    using System.Threading.Tasks;

    using HSM.Enums;

    public interface IActivity
    {
        ActivityMode Mode { get; }

        Task ActivateAsync(CancellationToken ct);

        Task DeactivateAsync(CancellationToken ct);
    }

}
