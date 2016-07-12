namespace BehaviorLibrary
{
    public class StatefulSequence : BehaviorComponent
    {
        private int lastBehavior = 0;

        public StatefulSequence(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            Name = "StatefulSequence";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            //start from last remembered position
            for (; lastBehavior < behaviors.Length; lastBehavior++)
            {
                switch (behaviors[lastBehavior].Execute())
                {
                    case Status.Failure:
                        lastBehavior = 0;
                        Status = Status.Failure;
                        return Status;
                    case Status.Success:
                        Status = Status.Success;
                        continue;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                    default:
                        lastBehavior = 0;
                        Status = Status.Success;
                        return Status;
                }
            }

            lastBehavior = 0;
            Status = Status.Success;
            return Status;
        }

        public StatefulSequence SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}

