using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class Selector : BehaviorComponent
    {
        public Selector(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            Name = "Selector";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            for (int i = 0; i < behaviors.Length; i++)
            {
                switch (behaviors[i].Execute())
                {
                    case Status.Failure:
                        Status = Status.Failure;;
                        continue;
                    case Status.Success:
                        Status = Status.Success;
                        return Status;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                    default:
                        continue;
                }
            }

            Status = Status.Failure;
            return Status;
        }

        public Selector SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
