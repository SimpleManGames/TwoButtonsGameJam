namespace HSM
{
    using HSM.Interfaces;

    public class NoopPhase : ISequence
    {
        public bool IsDone { get; private set; }

        public void Start()
        {
            IsDone = true;
        }

        public bool Update()
        {
            return IsDone;
        }
    }
}
