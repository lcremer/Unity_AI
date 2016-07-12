using System;
using UnityEngine;

namespace BehaviorLibrary
{
    public class ConditionalGameObjectDistance : BehaviorComponent
    {

        public GameObject ValueA;
        public GameObject ValueB;
        public float Distance;
        public ConditionType Condition;

        public override BehaviorComponent[] Behaviors
        {
            get { return null; }
        }

        public ConditionalGameObjectDistance(GameObject a, GameObject b, float d, ConditionType condition)
        {
            ValueA = a;
            ValueB = b;
            Distance = d;
            Condition = condition;
            Name = "Conditional GameObject Distance";
        }

        public ConditionalGameObjectDistance(Transform a, Transform b, float d, ConditionType condition)
        {
            ValueA = a.gameObject;
            ValueB = b.gameObject;
            Distance = d;
            Condition = condition;
            Name = "Conditional GameObject Distance";
        }

        public ConditionalGameObjectDistance(GameObject a, Transform b, float d, ConditionType condition)
        {
            ValueA = a;
            ValueB = b.gameObject;
            Distance = d;
            Condition = condition;
            Name = "Conditional GameObject Distance";
        }

        public ConditionalGameObjectDistance(Transform a, GameObject b, float d, ConditionType condition)
        {
            ValueA = a.gameObject;
            ValueB = b;
            Distance = d;
            Condition = condition;
            Name = "Conditional GameObject Distance";
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
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) > Distance;
                case ConditionType.GreaterOrEqual:
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) >= Distance;
                case ConditionType.Lesser:
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) < Distance;
                case ConditionType.LesserOrEqual:
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) <= Distance;
                case ConditionType.Equals:
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) == Distance;
                case ConditionType.NotEqual:
                    return Vector3.Distance(ValueA.transform.position, ValueB.transform.position) != Distance;
            }
            return false;
        }

        public ConditionalGameObjectDistance SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
