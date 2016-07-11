using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Assets.Project.Source.Common.Utility;
using UnityEngine;

namespace BehaviorLibrary.Components
{
    public abstract class  BehaviorComponent: ICloneable
    {
        #if UNITY_EDITOR
        public BehaviorNode Node;
        public bool NotifyOnExecute;
        #endif

        public Action<BehaviorComponent> AddToHistory;

        public virtual BehaviorComponent[] Behaviors
        {
            get { return behaviors; }
        }
        protected BehaviorComponent[] behaviors;

        protected BehaviorComponent AssignBehaviors(BehaviorComponent[] behaviorComponents)
        {
            // This is done to prevent previewer errors
            #if UNITY_EDITOR
            behaviors = new BehaviorComponent[behaviorComponents.Length];
            for (int i = 0; i < behaviorComponents.Length; i++)
            {
                if (behaviorComponents[i] == null)
                {
                    Debug.Log("Something fucked up on iteration: " + i);
                }
                behaviors[i] = (BehaviorComponent)behaviorComponents[i].Clone();
            }
            #else
            behaviors = behaviors;
            #endif
            return this;
        }

        public virtual string Name
        {
            get { return name; }
            set { name = value; }
        }
        protected string name;

        public Status Status
        {
            get { return status; }
            set
            {
                status = value;
#if UNITY_EDITOR
                if (NotifyOnExecute)
                {
                    Debug.Log(Name + "Was Executed at " + Time.time + "Status was " + Status);
                }
#endif
            }
        }
        private Status status;

        public BehaviorComponent(){}

        public abstract Status Execute();

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}
