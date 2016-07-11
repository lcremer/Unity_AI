using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BehaviorLibrary.Components.Decorators
{
    public class Alterable : BehaviorComponent
    {

        public string ID;
        public BehaviorTree BehaviorTree;

        public Alterable(BehaviorComponent behavior, string id)
        {
            Name = "Atlerable";
            ID = id;
            if (behavior is Alterable)
            {
                Debug.LogError("Cannot have Alterable BehaviorComponent child of Alterable");
                return;
            }
            AssignBehaviors(new[] { behavior });
        }

        ~Alterable()
        {
            BehaviorTree.AlterableComponents.Remove(ID);
        }

        public override Status Execute()
        {
            AddToHistory(this);
            return Status = behaviors[0].Execute();
        }

        public Alterable SetName(string name)
        {
            Name = name;
            return this;
        }

        public override string Name
        {
            get { return name + ":" + ID; }
            set { name = value; }
        }

        public BehaviorComponent UpdateBehaviors(BehaviorComponent behavior)
        {
            if (behavior is Alterable)
            {
                Debug.LogError("Cannot have Alterable Decorator as Child of Alterable");
                return this;
            }
#if UNITY_EDITOR
            behaviors = new[] {(BehaviorComponent) behavior.Clone()};
#else
            behaviors = new[] {behavior};
#endif
            //BehaviorTree.RefreshBehaviorTree();
            return this;
        }
    }
}
