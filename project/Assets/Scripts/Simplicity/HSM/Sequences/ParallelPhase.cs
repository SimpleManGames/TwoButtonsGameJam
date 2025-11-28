namespace HSM
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using HSM.Interfaces;

    public class ParallelPhase : ISequence
    {
        public bool IsDone { get; private set; }

        private List<Task> _tasks;

        private readonly List<PhaseStep> _steps;

        private readonly CancellationToken _ct;

        public ParallelPhase(List<PhaseStep> steps, CancellationToken ct)
        {
            _steps = steps;
            _ct = ct;
        }

        public void Start()
        {
            if (_steps == null || _steps.Count == 0)
            {
                IsDone = true;
                return;
            }

            foreach (PhaseStep t in _steps)
                _tasks.Add(t(_ct));
        }

        public bool Update()
        {
            if (IsDone)
                return true;

            IsDone = _tasks == null || _tasks.TrueForAll(t => t.IsCompleted);
            return IsDone;
        }
    }
}
