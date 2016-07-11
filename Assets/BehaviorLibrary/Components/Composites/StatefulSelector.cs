using System;
using BehaviorLibrary.Components;
using UnityEngine;

namespace BehaviorLibrary
{
    public class StatefulSelector : BehaviorComponent
    {
        private int lastBehavior = 0;

        public StatefulSelector(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            Name = "StatefulSelector";
        }

        public override Status Execute()
        {
            AddToHistory(this);

            for (; lastBehavior < behaviors.Length; lastBehavior++)
            {
                switch (behaviors[lastBehavior].Execute())
                {
                    case Status.Failure:
                        Status = Status.Failure;
                        continue;
                    case Status.Success:
                        lastBehavior = 0;
                        Status = Status.Success;
                        return Status;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                    default:
                        continue;
                }
            }

            lastBehavior = 0;
            Status = Status.Failure;
            return Status;
        }

        public StatefulSelector SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
