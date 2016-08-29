using BehaviorLibrary;
using UnityEngine;

public class BehaviorTest : MonoBehaviour
{
    public GameObject Target;
    public float SightDistance;
    public float FollowDistance;
    public Transform PatrolPoint;
    public float PatrolRadius;
    public bool CanBehave;

    public BehaviorTree behavior;

    private Vector3 patrolPoint;
    private float updateTick;

    //Note: this could be used to actively check states and switch behaviors or listen for tasks and switch
    public int switchBehaviors()
    {
        return 0;
    }

    #region Conditions
    private bool CanSeeTarget()
    {
        //if (Vector3.Distance(transform.position, Target.transform.position) <= SightDistance)
        //{
        //    Debug.Log("Can see target");
        //}
        return Vector3.Distance(transform.position, Target.transform.position) <= SightDistance;
    }

    private bool TooCloseToTarget()
    {
        //if (Vector3.Distance(transform.position, Target.transform.position) <= FollowDistance)
        //{
        //    Debug.Log("too close to target");
        //}
        return Vector3.Distance(transform.position, Target.transform.position) <= FollowDistance;
    }
    #endregion

    #region Actions
    private Status FollowTarget()
    {
        //Debug.Log("Following Target");
        transform.position = Vector3.Lerp(transform.position, Target.transform.position, .1f);
        return Status.Success;
    }

    private Status CircleTarget()
    {
        //Debug.Log("Circling Target");
        Vector3 targetPos = Target.transform.position;
        Vector3 pos = targetPos + (new Vector3(Mathf.Cos(Time.time), 0, Mathf.Sin(Time.time))*FollowDistance);
        transform.position = Vector3.Lerp(transform.position, pos, .1f);
        return Status.Success;
    }

    private Status GetRandomPatrolPoint()
    {
        if (Time.time > updateTick)
        {
            updateTick = Time.time + 2.5f;
            patrolPoint = (UnityEngine.Random.insideUnitSphere*PatrolRadius) + PatrolPoint.position;
            patrolPoint.y = 0;
        }
        return Status.Success;
    }

    private Status GoToPatrolPoint()
    {
        //Debug.Log("Going to Patrol Point");
        transform.position = Vector3.Lerp(transform.position, patrolPoint, .1f);
        return Status.Success;
    }
    #endregion

    //Run this on awake, enable, or start
    public void DefineBehavior()
    {
        //Conditions
        ConditionalLambda canSeeTarget = new ConditionalLambda(() => Vector3.Distance(transform.position, Target.transform.position) <= SightDistance);
        ConditionalLambda tooCloseToTarget = new ConditionalLambda(() => Vector3.Distance(transform.position, Target.transform.position) <= FollowDistance);

        //Actions
        BAction followTarget = new BAction(FollowTarget);
        BAction getPatrolPoint = new BAction(GetRandomPatrolPoint);
        BAction goToPatrolPoint = new BAction(GoToPatrolPoint);
        BAction circleTarget = new BAction(CircleTarget);

        SequenceContinued checkIfShouldCircle = new SequenceContinued(tooCloseToTarget, circleTarget);
        //If too circle target, if not follow Target
        SelectorContinued following = new SelectorContinued(checkIfShouldCircle, followTarget);
        SequenceContinued checkIfCanFollow = new SequenceContinued(canSeeTarget, following);
        SequenceContinued patrol = new SequenceContinued(getPatrolPoint, goToPatrolPoint);

        SelectorContinued lookForTarget = new SelectorContinued(checkIfCanFollow, patrol);

        //NOTE: RootSelector allows for StateMachine type usage in behavior tree
        //setup root node, choose initialization phase or pathing/movement phase
        SelectorBranch root = new SelectorBranch(switchBehaviors, lookForTarget);

        //set a reference to the root
        behavior = ScriptableObject.CreateInstance<BehaviorTree>();
        behavior.Init( root );
        CanBehave = true;
    }

    public void Start()
    {
        DefineBehavior();
    }

    public void Update()
    {
        if (CanBehave && behavior != null)
        {
            behavior.Behave();
        }
    }
}