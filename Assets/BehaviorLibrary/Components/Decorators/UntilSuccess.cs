namespace BehaviorLibrary
{
    /// <summary>
    /// Repeats behavior till return status is Success
    /// </summary>
    public class UntilSuccess : BehaviorComponent
    {
        public UntilSuccess(BehaviorComponent behavior)
        {
            AssignBehaviors(new[] { behavior });
            Name = "RepeaterContinued";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            switch (behaviors[0].Execute())
            {
                case Status.Failure:
                case Status.Running:
                    Status = Status.Running;
                    return Status;
                case Status.Success:
                    Status = Status.Success;
                    return Status;
            }
            return Status.Running;
        }

        public UntilSuccess SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
