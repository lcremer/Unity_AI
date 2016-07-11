using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class Inverter : BehaviorComponent
    {

        public Inverter(BehaviorComponent behavior) 
        {
            AssignBehaviors(new[] { behavior });
            Name = "Inverter";
        }

        public override Status Execute()
        {
            AddToHistory(this);

            switch (behaviors[0].Execute())
            {
                case Status.Failure:
                    Status = Status.Success;
                    return Status;
                case Status.Success:
                    Status = Status.Failure;
                    return Status;
                case Status.Running:
                    Status = Status.Running;
                    return Status;
            }

            Status = Status.Success;
            return Status;
        }

        public Inverter SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
