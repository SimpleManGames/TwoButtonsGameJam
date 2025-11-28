namespace HSM.Interfaces
{
    public interface ISequence
    {
        bool IsDone { get; }

        void Start();

        bool Update();
    }

}
