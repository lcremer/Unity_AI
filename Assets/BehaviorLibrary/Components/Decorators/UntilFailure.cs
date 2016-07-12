namespace BehaviorLibrary
{
    /// <summary>
    /// Repeats behavior till return status is Failure
    /// </summary>
    public class UntilFailure : BehaviorComponent
    {
        public UntilFailure(BehaviorComponent behavior)
        {
            AssignBehaviors(new[] { behavior });
            Name = "UntilFail";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            switch (behaviors[0].Execute())
            {
                case Status.Success:
                case Status.Running:
                    Status = Status.Running;
                    return Status;
                case Status.Failure:
                    Status = Status.Failure;
                    return Status;
            }
            return Status.Running;
        }

        public UntilFailure SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
