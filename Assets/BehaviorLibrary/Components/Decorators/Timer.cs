using UnityEngine;

namespace BehaviorLibrary
{
    public class Timer : BehaviorComponent
    {
        public override string Name
        {
            get { return name + " " + (waitTime - timeElapsed); }
            set { name = value; }
        }

        private float lastTime = 0;
        private float timeElapsed = 0;

        private float waitTime;

        public Timer(float timeToWait, BehaviorComponent behavior)
        {
            AssignBehaviors(new[] { behavior });
            waitTime = timeToWait;
            Name = "Timer";
        }

        public override Status Execute()
        {
            AddToHistory(this);
            timeElapsed = Time.time - lastTime;
            if (timeElapsed >= waitTime)
            {
                timeElapsed = 0;
                lastTime = Time.time;
                Status = behaviors[0].Execute();
                return Status;
            }
            else
            {
                Status = Status.Running;
                return Status;
            }
        }

        public Timer SetName(string name)
        {
            Name = name;
            return this;
        }
    }
}
