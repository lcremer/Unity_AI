using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    /// <summary>
    /// Repeats Behavior in single execution till failure or max iteration is met
    /// </summary>
    public class RepeatUntilFailureConstant : BehaviorComponent
    {
        private int maxIterations;
        private int iteration = 0;

        public RepeatUntilFailureConstant(int maxIterations, BehaviorComponent behavior)
        {
            this.maxIterations = maxIterations;
            AssignBehaviors(new[] { behavior });
            Name = "Counter";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            for (int i = 0; i < maxIterations; i++)
            {
                if (behaviors[0].Execute() == Status.Failure)
                {
                    Status = Status.Success;
                    return Status.Success;
                }
            }
            Status = Status.Failure;
            return Status.Failure;
        }

        public RepeatUntilFailureConstant SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
