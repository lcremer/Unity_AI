namespace BehaviorLibrary
{
    /// <summary>
    /// Repeats behavior till failure or max iteration is met
    /// </summary>
    public class RepeaterUntilFailureContinued : BehaviorComponent
    {
        private int maxIterations;
        private int iteration = 0;

        public RepeaterUntilFailureContinued(int maxIterations, BehaviorComponent behavior)
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
                if (behaviors[0].Execute() == Status.Failure)
                {
                    Status = Status.Success;
                    return Status.Success;
                }
                Status = Status.Running;
                return Status.Running;
            }
            iteration = 0;
            Status = Status.Failure;
            return Status.Failure;
        }

        public RepeaterUntilFailureContinued SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
