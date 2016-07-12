using System;

namespace BehaviorLibrary
{
    public class SelectorBranch : BehaviorComponent
    {
        private Func<int> Index;

        public SelectorBranch(Func<int> index, params BehaviorComponent[] behaviors)
        {
            Index = index;
            AssignBehaviors(behaviors);
            Name = "Branch Selector";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            switch (Behaviors[Index.Invoke()].Execute())
            {
                case Status.Failure:
                    Status = Status.Failure;
                    return Status;
                case Status.Success:
                    Status = Status.Success;
                    return Status;
                case Status.Running:
                    Status = Status.Running;
                    return Status;
                default:
                    Status = Status.Running;
                    return Status;
            }
        }
    }
}
