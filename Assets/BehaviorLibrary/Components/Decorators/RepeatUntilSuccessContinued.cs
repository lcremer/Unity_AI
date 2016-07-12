namespace BehaviorLibrary
{
    /// <summary>
    /// Repeats behavior till Success or max iteration is met
    /// </summary>
    public class RepeaterUntilSuccessContinued : BehaviorComponent
    {
        private int maxIterations;
        private int iteration = 0;

        public RepeaterUntilSuccessContinued(int maxIterations, BehaviorComponent behavior)
        {
            this.maxIterations = maxIterations;
            AssignBehaviors(new[] { behavior });
            Name = "RepeaterContinued";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            if (iteration <= maxIterations)
            {
                iteration++;
                if (behaviors[0].Execute() == Status.Success)
                {
                    Status = Status.Success;
                    return Status;
                }
                Status = Status.Running;
                return Status;
            }
            iteration = 0;
            Status = Status.Failure;
            return Status;
        }

        public RepeaterUntilSuccessContinued SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
