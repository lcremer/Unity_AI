using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Assets.Project.Source.UI;
using UnityEngine;
using Object = System.Object;

namespace BehaviorLibrary.Components.Conditionals
{
    public class ConditionalLambda : BehaviorComponent
    {
        #if UNITY_EDITOR
        public Expression<Func<bool>> Expression; 
        public Action<ConditionalLambda> TestAction;
        public List<Object> ExpressionObjects = new List<object>();
        public string ConditionalDescription;
        #endif

        public Func<bool> TestFunc;       
        public bool Result;


        public override BehaviorComponent[] Behaviors
        {
            get { return null; }
        }

        #if !UNITY_EDITOR
        public ConditionalLambda(Func<bool> test)
        {
            TestFunc = test;
            Name = "ConditionalLambda";
        }
        #endif

        #if UNITY_EDITOR
        public ConditionalLambda(Action<ConditionalLambda> test)
        {
            TestAction = test;
            Name = "ConditionalLambda";
        }

        public ConditionalLambda(Expression<Func<bool>> test)
        {
            Expression = test;
            Name = "ConditionalLambda";
        }
        #endif

        public override Status Execute()
        {
            AddToHistory(this);

            #if !UNITY_EDITOR
            if (TestFunc != null)
            {
                Result = TestFunc.Invoke();
            }
            #endif
            
            #if UNITY_EDITOR
            if (Expression != null)
            {
                Expression.Compile().Invoke();
            }
            if (TestAction != null)
            {
                ExpressionObjects.Clear();
                TestAction.Invoke(this);
            }
            #endif

            switch (Result)
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

        public ConditionalLambda SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
