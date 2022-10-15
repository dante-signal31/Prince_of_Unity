using System;
using System.Collections;
using Prince;
using UnityEngine;

/// <summary>
/// <p>Any prefab that has ledges which can be climbed should have this behaviour to interact with
/// its grabbing ledges.</p>
/// 
/// <p>Some bricks can be climbed, so they have to points (right and left) where they can be climbed.
/// This component allow main character get those climbing points.</p>
/// </summary>
public class Climbable: MonoBehaviour
{
    [Header("WIRING:")]
    [Tooltip("Needed to signal character actions to animations state machine.")]
    [SerializeField] private Animator stateMachine;
    [Tooltip("Needed to know current state machine status.")]
    [SerializeField] private ClimbableStatus climbableStatus;

    [Header("CONFIGURATION:")]
    [Tooltip("Anchor for climbing and hanging animations at right side.")]
    [SerializeField] private Transform grabbingPointRight;
    [Tooltip("Anchor for climbing and hanging animations at left side.")]
    [SerializeField] private Transform grabbingPointLeft;
    [Tooltip("Point to place character after he descends by right hand side.")]
    [SerializeField] private Transform descendingPointRight;
    [Tooltip("Point to place character after he descends by left hand side.")]
    [SerializeField] private Transform descendingPointLeft;
    [Tooltip("Whether current brick is hollow below hanging ground allowing balancing movements.")]
    [SerializeField] private bool HollowBrick;
    
    [Header("DEBUG:")]
    [Tooltip("Show this component logs on console window.")]
    [SerializeField] private bool showLogs;

    public enum HangableLedges
    {
        Right,
        Left,
    }

    // private bool _climbingAbortable;
    //
    // /// <summary>
    // /// Whether we can abort a climbing.
    // /// </summary>
    // public bool ClimbingAbortable { 
    //     get => _climbingAbortable;
    //     set
    //     {
    //         _climbingAbortable = value;
    //         stateMachine.SetBool("ClimbingAbortable", value);
    //     } 
    // }

    /// <summary>
    /// Whether we can abort a climbing.
    /// </summary>
    public bool ClimbingAbortable => climbableStatus.ClimbingAbortable;

    /// <summary>
    /// <p>Result of the last climbing interaction from the point of view of the climbable.</p>
    ///
    /// <p>Possible results of a climbing operation:</p>
    /// <ul>
    /// <li> <b>Climbed</b>: Character climbed to a ledge grabbing point.</li>
    /// <li> <b>Descended</b>: Character descended to a descending point.</li>
    /// <li> <b>Uncertain</b>: Climbing interaction has not ended yet.</li>
    /// </ul>
    /// </summary>
    public ClimbableStatus.ClimbingResult ClimbingResult => climbableStatus.LastClimbingResult;
    
    // /// <summary>
    // /// Whether this brick is playing character climbing animations.
    // /// </summary>
    // public bool PlayingAnimations { get; private set; }

    /// <summary>
    /// Used to place character on ledge after climbing by right side.
    /// </summary>
    public Transform RightLedgeTransform => grabbingPointRight;
    
    /// <summary>
    /// Used to place character on ledge after climbing by left side.
    /// </summary>
    public Transform LeftLedgeTransform => grabbingPointLeft;
    
    /// <summary>
    /// Used to place character after descending by right side.
    /// </summary>
    public Transform RightDescendingTransform => descendingPointRight;
    
    /// <summary>
    /// Used to place character after descending by left side.
    /// </summary>
    public Transform LeftDescendingTransform => descendingPointLeft;
    
    private EventBus _eventBus;
    private bool _princeClimbingEndedAlreadyTriggered = false;
    private bool _princeHangedAlreadyTriggered = false;
    
    private void Awake()
    {
        UpdateFlags();
        _eventBus = GameObject.Find("GameManagers").GetComponentInChildren<EventBus>();
    }

    /// <summary>
    /// Called from Prince to hang from this ledge.
    /// </summary>
    /// <param name="hangingLedge">Hanging ledge used by character to climb.</param>
    /// <param name="actionAlreadyPushed">Whether action was previously pushed and hold before starting the climbing.</param>
    public IEnumerator Hang(HangableLedges hangingLedge, bool actionAlreadyPushed, bool jumpStillPushed)
    {
        // PlayingAnimations = true;
        this.Log($"(Climbable - {transform.root.name}) Prince is hanging from me.", showLogs);
        climbableStatus.LookingRightWards = (hangingLedge == HangableLedges.Left);
        JumpPushed(jumpStillPushed);
        ActionPushed(actionAlreadyPushed);
        stateMachine.SetTrigger("Hang");
        // First yield return is needed to give time to climbableStatus to change its state from Inactive
        // to Hanging.
        yield return new WaitUntil(() => climbableStatus.CurrentState != ClimbableStatus.States.Inactive);
        // Second yield return is needed to play every needed climbing hanging animation sequence.
        yield return new WaitUntil(() => climbableStatus.CurrentState == ClimbableStatus.States.Inactive);
        JumpPushed(false);
        ActionPushed(false);
        this.Log($"(Climbable - {transform.root.name}) Climbing animation finished.", showLogs);
        // PlayingAnimations = false;
    }

    /// <summary>
    /// Called from Prince to hang from this ledge to descend.
    /// </summary>
    /// <param name="hangingLedge">Hanging ledge used by character to descend.</param>
    /// <param name="actionAlreadyPushed">Whether action was previously pushed and hold before starting the descending.</param>
    public IEnumerator Descend(HangableLedges hangingLedge, bool actionAlreadyPushed)
    {
        this.Log($"(Climbable - {transform.root.name}) Prince is hanging from me to descend.", showLogs);
        climbableStatus.LookingRightWards = (hangingLedge == HangableLedges.Left);
        JumpPushed(false);
        ActionPushed(actionAlreadyPushed);
        stateMachine.SetTrigger("Descend");
        // First yield return is needed to give time to climbableStatus to change its state from Inactive
        // to Descending.
        yield return new WaitUntil(() => climbableStatus.CurrentState != ClimbableStatus.States.Inactive);
        // Second yield return is needed to play every needed climbing hanging animation sequence.
        yield return new WaitUntil(() => climbableStatus.CurrentState == ClimbableStatus.States.Inactive);
        this.Log($"(Climbable - {transform.root.name}) Descend animation finished.", showLogs);
    }

    /// <summary>
    /// Used by Prince to signal climbable that action button has been (or not) pushed.
    /// </summary>
    /// <param name="isPushed">Whether button is pushed or not.</param>
    public void ActionPushed(bool isPushed)
    {
        stateMachine.SetBool("ActionPushed", isPushed);
    }
    
    /// <summary>
    /// Used by Prince to signal climbable that jump button has been (or not) pushed.
    /// </summary>
    /// <param name="isPushed">Whether button is pushed or not.</param>
    public void JumpPushed(bool isPushed)
    {
        stateMachine.SetBool("JumpPushed", isPushed);
    }

    /// <summary>
    /// Updates climbable state machine flags depending on set parameters.
    /// </summary>
    private void UpdateFlags()
    {
        stateMachine.SetBool("HollowBrick", HollowBrick);
    }

    /// <summary>
    /// Used when climbed brick falls.
    /// </summary>
    public void AbortClimbing()
    {
        stateMachine.SetTrigger("Abort");
    }

    private void Update()
    {
        Vector3 climbingPosition;
        Vector3 hangingPosition;
        
        switch (climbableStatus.CurrentState)
        {
            // Move camera if we have climbed or descended.
            case ClimbableStatus.States.Inactive when climbableStatus.PreviousState != ClimbableStatus.States.Inactive:
                if (_princeClimbingEndedAlreadyTriggered) break;
                if (climbableStatus.PreviousState == ClimbableStatus.States.Climbing)
                {
                    climbingPosition = climbableStatus.LookingRightWards
                        ? grabbingPointLeft.position
                        : grabbingPointRight.position; 
                    _eventBus.TriggerEvent(new GameEvents.PrinceClimbingEnded(climbingPosition), this); 
                }
                else
                {
                    hangingPosition = climbableStatus.LookingRightWards
                        ? descendingPointLeft.position
                        : descendingPointRight.position;
                    _eventBus.TriggerEvent(new GameEvents.PrinceHanged(hangingPosition), this);
                }
                _princeClimbingEndedAlreadyTriggered = true;
                break;
            // Move camera if we are hanging.
            case ClimbableStatus.States.Hanging when climbableStatus.PreviousState != ClimbableStatus.States.Hanging:
                if (_princeHangedAlreadyTriggered) break;
                hangingPosition = climbableStatus.LookingRightWards
                    ? descendingPointLeft.position
                    : descendingPointRight.position;
                _eventBus.TriggerEvent(new GameEvents.PrinceHanged(hangingPosition), this);
                _princeHangedAlreadyTriggered = true;
                break;
            default:
                _princeClimbingEndedAlreadyTriggered = false;
                _princeHangedAlreadyTriggered = false;
                break;
        }
    }

#if UNITY_EDITOR
    private void DrawPoint(Transform pointToDraw, Color color)
    {
        float gizmoRadius = 0.05f;
        Vector3 gizmoSize = new Vector3(gizmoRadius, gizmoRadius, gizmoRadius);
        Gizmos.color = color;
        Gizmos.DrawCube(pointToDraw.position, gizmoSize);
    }


    private void OnDrawGizmosSelected()
    {
        DrawPoint(LeftLedgeTransform, Color.magenta);
        DrawPoint(RightLedgeTransform, Color.cyan);
        DrawPoint(RightDescendingTransform, Color.blue);
        DrawPoint(LeftDescendingTransform, Color.red);
    }
#endif
    
}
