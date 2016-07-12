using System;

namespace BehaviorLibrary
{
    public class BAction : BehaviorComponent
    {
        public override BehaviorComponent[] Behaviors
        {
            get { return null; }
        }

        public Func<Status> Action;

        public BAction(string name = "BehaviorAction")
        {
            Name = name;
        }

        public BAction(Func<Status> action)
        {
            Action = action;
            Name = "BehaviorAction";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            switch (Action.Invoke())
            {
                case Status.Success:
                    Status = Status.Success;
                    return Status;
                case Status.Failure:
                    Status = Status.Failure;
                    return Status;
                case Status.Running:
                    Status = Status.Running;
                    return Status;
                default:
                    Status = Status.Failure;
                    return Status;
            }
        }

        public BAction SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
