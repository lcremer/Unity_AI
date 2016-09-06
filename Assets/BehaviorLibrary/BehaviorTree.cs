using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace BehaviorLibrary
{
    public enum Status
    {
        Failure,
        Success,
        Running,
        Dormant // TODO: fully implement usage
    }

    public delegate Status BehaviorReturn();

    [Serializable]
    public class BehaviorTree : ScriptableObject
    {
        public Action<BehaviorTree> BehaviorTreeUpdated;

        public Func<bool> CheckIfUpdateNeeded;

        public List<BehaviorComponent> History;
        public Dictionary<string, Alterable> AlterableComponents;

        public BehaviorComponent Root;

        public Status CurrentStatus { get; set; }

        public BehaviorTree(BehaviorComponent root)
        {
            Init(root);
        }

        public void Init(BehaviorComponent root)
        {
            Root = root;
            History = new List<BehaviorComponent>();
            AlterableComponents = new Dictionary<string, Alterable>();
            SetupHistoryCallBackAndAlterable(Root);
        }

        public Status Behave()
        {
            History.Clear();
            if (CheckIfUpdateNeeded != null && CheckIfUpdateNeeded())
            {
                RefreshBehaviorTree();
            }
            return CurrentStatus = Root.Execute();
        }

        // TODO: consider moving these into a Behavior.Editor history object
        private void AddToHistory(BehaviorComponent behaviorComponent)
        {
            History.Add(behaviorComponent);
        }

        public void SetupHistoryCallBackAndAlterable(BehaviorComponent behaviorComponent)
        {
            behaviorComponent.AddToHistory = AddToHistory;
            if (behaviorComponent is Alterable)
            {
                Alterable alterable = (Alterable) behaviorComponent;
                alterable.BehaviorTree = this;
                AlterableComponents.Add(alterable.ID, alterable);
            }
            if (behaviorComponent.Behaviors != null)
            {
                for (int i = 0; i < behaviorComponent.Behaviors.Length; i++)
                {
                    BehaviorComponent behavior = behaviorComponent.Behaviors[i];
                    behavior.AddToHistory = AddToHistory;
                    if (behavior.Behaviors != null)
                    {
                        SetupHistoryCallBackAndAlterable(behavior);
                    }
                }
            }
        }

        public void RefreshBehaviorTree()
        {
            AlterableComponents.Clear();
            SetupHistoryCallBackAndAlterable(Root);
#if UNITY_EDITOR
            if (BehaviorTreeUpdated != null)
            {
                Debug.Log("Refreshing Tree");
                BehaviorTreeUpdated(this);
            }
#endif
        }
    }
}
