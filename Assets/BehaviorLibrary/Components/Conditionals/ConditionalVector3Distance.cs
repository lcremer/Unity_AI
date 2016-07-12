using System;
using UnityEngine;

namespace BehaviorLibrary
{
    public class ConditionalVector3Distance : BehaviorComponent
    {

        public Vector3 ValueA;
        public Vector3 ValueB;
        public float Distance;
        public ConditionType Condition;

        public override BehaviorComponent[] Behaviors
        {
            get { return null; }
        }

        public ConditionalVector3Distance(Vector3 a, Vector3 b, float d, ConditionType condition)
        {
            ValueA = a;
            ValueB = b;
            Distance = d;
            Condition = condition;
            Name = "Conditional Vector3 Distance";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            switch (Test())
            {
                case true:
                    Status = Status.Success;
                    return Status;
                case false:
                    Status = Status.Failure;
                    return Status;
                default:
                    Status = Status.Failure;
                    return Status;
            }
        }

        public bool Test()
        {
            if (ValueA == null)
            {
                throw new Exception("ValueA is null");
            }
            if (ValueB == null)
            {
                throw new Exception("ValueB is null");
            }
            switch (Condition)
            {
                case ConditionType.Greater:
                    return Vector3.Distance(ValueA, ValueB) > Distance;
                case ConditionType.GreaterOrEqual:
                    return Vector3.Distance(ValueA, ValueB) >= Distance;
                case ConditionType.Lesser:
                    return Vector3.Distance(ValueA, ValueB) < Distance;
                case ConditionType.LesserOrEqual:
                    return Vector3.Distance(ValueA, ValueB) <= Distance;
                case ConditionType.Equals:
                    return Vector3.Distance(ValueA, ValueB) == Distance;
                case ConditionType.NotEqual:
                    return Vector3.Distance(ValueA, ValueB) != Distance;
            }
            return false;
        }

        public ConditionalVector3Distance SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
