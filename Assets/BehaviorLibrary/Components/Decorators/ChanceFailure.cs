using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class ChanceFailure : BehaviorComponent
    {

        private float probability;

        private Func<float> randomFunction;

        public ChanceFailure(float probability, Func<float> randomFunction, BehaviorComponent behavior)
        {
            this.probability = probability;
            this.randomFunction = randomFunction;
            AssignBehaviors(new[] {behavior});
            Name = "ChanceFailure";
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
                Status = Status.Failure;
                return Status;
            }
        }

        public ChanceFailure SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
