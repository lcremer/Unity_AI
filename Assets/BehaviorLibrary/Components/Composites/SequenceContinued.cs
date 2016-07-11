using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Composites
{
    public class SequenceContinued : BehaviorComponent
    {
        private short sequence = 0;

        private short seqLength = 0;

        public SequenceContinued(params BehaviorComponent[] behaviors)
        {
            AssignBehaviors(behaviors);
            seqLength = (short) Behaviors.Length;
            Name = "PartialSequence";
        }

        public override Status Execute()
        {
            AddToHistory(this);

            while (sequence < seqLength)
            {
                switch (Behaviors[sequence].Execute())
                {
                    case Status.Failure:
                        sequence = 0;
                        Status = Status.Failure;
                        return Status;
                    case Status.Success:
                        sequence++;
                        Status = Status.Running;
                        return Status;
                    case Status.Running:
                        Status = Status.Running;
                        return Status;
                }

            }

            sequence = 0;
            Status = Status.Success;
            return Status;
        }

        public SequenceContinued SetName(string name)
        {
            Name = name;
            return this;
        }

    }
}
