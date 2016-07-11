using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{   
    /// <summary>
    /// Will not Execute behavior till max iterations are met
    /// </summary>
    public class Counter : BehaviorComponent
    {
        private int maxIterations;
        private int iteration = 0;

        public Counter(int maxIterations, BehaviorComponent behavior)
        {
            this.maxIterations = maxIterations;
            AssignBehaviors(new[] { behavior });
            Name = "Counter";
        }

        public override Status Execute()
        {
            AddToHistory(this);

            if (iteration < maxIterations)
            {
                iteration++;
                Status = Status.Running;
                return Status.Running;
            }
            else
            {
                iteration = 0;
                Status = behaviors[0].Execute();
                return Status;
            }
        }

        public Counter SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
