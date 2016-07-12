using System;

namespace BehaviorLibrary
{
    public class ConditionalGeneric<T> : BehaviorComponent where T : IComparable<T>
    {
        public T ValueA;
        public T ValueB;
        public ConditionType Condition;

        public override BehaviorComponent[] Behaviors
        {
            get { return null; }
        }

        public ConditionalGeneric(T a, T b, ConditionType condition)
        {
            ValueA = a;
            ValueB = b;
            Condition = condition;
            Name = "Conditional Generic";
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
            switch (Condition)
            {
                case ConditionType.Greater:
                    return ValueA.CompareTo(ValueB) > 0;
                case ConditionType.GreaterOrEqual:
                    return ValueA.CompareTo(ValueB) >= 0;
                case ConditionType.Lesser:
                    return ValueA.CompareTo(ValueB) < 0;
                case ConditionType.LesserOrEqual:
                    return ValueA.CompareTo(ValueB) <= 0;
                case ConditionType.Equals:
                    return ValueA.CompareTo(ValueB) == 0;
                case ConditionType.NotEqual:
                    return ValueA.CompareTo(ValueB) != 0;
            }
            return false;
        }

        public ConditionalGeneric<T> SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
