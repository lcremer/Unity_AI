using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class ChanceRunning : BehaviorComponent
    {

        private float probability;

        private Func<float> randomFunction;

        public ChanceRunning(float probability, Func<float> randomFunction, BehaviorComponent behavior)
        {
            this.probability = probability;
            this.randomFunction = randomFunction;
            AssignBehaviors(new[] { behavior });
            Name = "ChanceRunning";
        }


        public override Status Execute()
        {
            AddToHistory(this);

            if (randomFunction.Invoke() <= probability)
            {
                Status = behaviors[0].Execute();
                return Status;
            }
            else
            {
                Status = Status.Running;
                return Status.Running;
            }
        }

        public ChanceRunning SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
