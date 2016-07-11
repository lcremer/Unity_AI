using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class SelectorContinued : BehaviorComponent
    {
        private short selections = 0;

        private short selLength = 0;

        public SelectorContinued(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            selLength = (short)Behaviors.Length;
            Name = "ContinuedSelector";
        }

        public SelectorContinued SetName(string name)
        {
            Name = name;
            return this;
        }

        public override Status Execute()
        {
            AddToHistory(this);
            while (selections < selLength)
            {
                switch (Behaviors[selections].Execute())
                {
                    case Status.Failure:
                        selections++;
                        Status = Status.Running;
                        return Status;
                    case Status.Success:
                        selections = 0;
                        Status = Status.Success;
                        return Status;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                    default:
                        selections++;
                        Status = Status.Failure;
                        return Status;
                }
            }

            selections = 0;
            Status = Status.Failure;
            return Status;
        }
    }
}
