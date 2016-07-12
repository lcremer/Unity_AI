using System;

namespace BehaviorLibrary
{
    public class SelectorRandom : BehaviorComponent
    {
        private System.Random random = new System.Random(DateTime.Now.Millisecond);

        public SelectorRandom(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            Name = "RandomSelector";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            random = new System.Random(DateTime.Now.Millisecond);

            switch (behaviors[random.Next(0, behaviors.Length - 1)].Execute())
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
                    Status = Status.Failure;
                    return Status;
            }
        }

        public SelectorRandom SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
