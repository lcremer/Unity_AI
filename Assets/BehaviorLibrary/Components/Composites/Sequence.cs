using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class Sequence : BehaviorComponent
    {
        public Sequence(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            Name = "Sequence";
        }

        public override Status Execute()
        {
            AddToHistory(this);
			//add watch for any running behaviors
			bool anyRunning = false;

            for(int i = 0; i < behaviors.Length;i++)
            {
                switch (behaviors[i].Execute())
                {
                    case Status.Failure:
                        Status = Status.Failure;
                        return Status;
                    case Status.Success:
                        Status = Status.Success;
                        continue;
                    case Status.Running:
						anyRunning = true;
                        Status = Status.Running;
                        continue;
                    default:
                        Status = Status.Success;
                        return Status;
                }
            }

            //TODO: Review this
			//if none running, return success, otherwise return running
            Status = !anyRunning ? Status.Success : Status.Running;
            return Status;
        }

        public Sequence SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
