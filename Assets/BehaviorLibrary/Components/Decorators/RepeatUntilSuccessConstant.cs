namespace BehaviorLibrary
{
    /// <summary>
    /// Repeats Behavior in single execution till Success or max iteration is met
    /// </summary>
    public class RepeatUntilSuccessConstant : BehaviorComponent
    {
        private int maxIterations;
        private int iteration = 0;

        public RepeatUntilSuccessConstant(int maxIterations, BehaviorComponent behavior)
        {
            this.maxIterations = maxIterations;
            AssignBehaviors(new[] { behavior });
            Name = "Counter";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            for (int i = 0; i < maxIterations; i++)
            {
                if (behaviors[0].Execute() == Status.Success)
                {
                    Status = Status.Success;
                    return Status;
                }
            }
            Status = Status.Failure;
            return Status;
        }

        public RepeatUntilSuccessConstant SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
