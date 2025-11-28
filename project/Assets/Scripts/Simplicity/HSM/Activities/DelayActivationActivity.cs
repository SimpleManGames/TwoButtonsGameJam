namespace HSM
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class DelayActivationActivity : Activity
    {
        private readonly float seconds;

        public DelayActivationActivity(float seconds)
        {
            this.seconds = seconds;
        }

        public async override Task ActivateAsync(CancellationToken ct)
        {
            await Task.Delay(TimeSpan.FromSeconds(seconds), ct);
            await base.ActivateAsync(ct);
        }
    }
}
