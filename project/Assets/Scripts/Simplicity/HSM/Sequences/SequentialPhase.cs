namespace HSM
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using HSM.Interfaces;

    public class SequentialPhase : ISequence
    {
        public bool IsDone { get; private set; }

        private Task _current;

        private int _index = -1;

        private readonly List<PhaseStep> _steps;

        private readonly CancellationToken _ct;

        public SequentialPhase(List<PhaseStep> steps, CancellationToken ct)
        {
            _steps = steps;
            _ct = ct;
        }

        public void Start()
        {
            Next();
        }

        public bool Update()
        {
            if (IsDone)
                return true;

            if (_current == null || _current.IsCompleted)
                Next();

            return IsDone;
        }
        
        private void Next()
        {
            _index++;

            if (_index >= _steps.Count)
            {
                IsDone = true;
                return;
            }

            _current = _steps[_index](_ct);
        }
    }
}
