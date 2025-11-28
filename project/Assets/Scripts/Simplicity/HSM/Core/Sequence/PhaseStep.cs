namespace HSM
{
    using System.Threading;
    using System.Threading.Tasks;

    public delegate Task PhaseStep(CancellationToken ct);
}
