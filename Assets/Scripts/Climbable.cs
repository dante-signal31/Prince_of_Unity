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
    [Tooltip("Climbing and hanging animations state machine.")]
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

    private bool _climbingAbortable;
    
    /// <summary>
    /// Whether we can abort a climbing.
    /// </summary>
    public bool ClimbingAbortable { 
        get => _climbingAbortable;
        set
        {
            _climbingAbortable = value;
            stateMachine.SetBool("ClimbingAbortable", value);
        } 
    }
    
    // /// <summary>
    // /// Wheter this brick is playing character climbing animations.
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
    
    private void Awake()
    {
        UpdateFlags();
    }

    /// <summary>
    /// Called from Prince to hang from this ledge.
    /// </summary>
    /// <param name="fromRight">True if Prince is trying right ledge, false if is trying left ledge.</param>
    public IEnumerator Hang(HangableLedges HangingLedge)
    {
        // PlayingAnimations = true;
        this.Log($"(Climbable - {transform.root.name}) Prince is hanging from me.", showLogs);
        climbableStatus.LookingRightWards = (HangingLedge == HangableLedges.Left);
        stateMachine.SetTrigger("Hang");
        JumpPushed(true);
        // This yield return null is needed to give time to climbableStatus to change its state from Inactive
        // to Hanging.
        yield return null;
        yield return new WaitUntil(() => climbableStatus.CurrentState == ClimbableStatus.States.Inactive);
        JumpPushed(false);
        this.Log($"(Climbable - {transform.root.name}) Climbing animation finished.", showLogs);
        // PlayingAnimations = false;
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

    
}
