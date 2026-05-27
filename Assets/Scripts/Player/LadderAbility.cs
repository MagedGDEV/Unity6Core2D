using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LadderAbility : BaseAbility
{
    [SerializeField] private InputActionReference ladderActionRef;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float setMinLadderTime;
    private float minimumLadderTime;
    private bool climb;
    public bool canGoOnLadder;

    private string ladderAnimParameterName = "Ladder";
    private int ladderParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        minimumLadderTime = setMinLadderTime;
        ladderParameterInt = Animator.StringToHash(ladderAnimParameterName);
    }

    private void OnEnable()
    {
        ladderActionRef.action.performed += TryToClimb;
        ladderActionRef.action.canceled += StopClimb;
    }

    private void OnDisable()
    {
        ladderActionRef.action.performed -= TryToClimb;
        ladderActionRef.action.canceled -= StopClimb;
    }

    private void TryToClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;

        linkedAnimator.enabled = true;
        
        if (linkedStateMachine.currentState == PlayerStates.State.Ladders ||
            linkedStateMachine.currentState == PlayerStates.State.Dash ||
            !canGoOnLadder)
            return;
        
        linkedStateMachine.ChangeState(PlayerStates.State.Ladders);
        linkedPhysicsControl.DisableGravity();
        linkedPhysicsControl.ResetVelocity();
        climb = true;
        minimumLadderTime = setMinLadderTime;
    }

    private void StopClimb(InputAction.CallbackContext value)
    {
        if (!isPermitted ||
            linkedStateMachine.currentState != PlayerStates.State.Ladders)
            return;
        
        linkedPhysicsControl.ResetVelocity();
        linkedAnimator.enabled = false;
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        linkedAnimator.enabled = true;
    }

    public override void ProcessAbility()
    {
        if (climb)
            minimumLadderTime -= Time.deltaTime;

        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }

        if (!canGoOnLadder && !linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            return;
        }

        if (linkedPhysicsControl.grounded && minimumLadderTime <= 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        
    }

    public override void ProcessFixedAbility()
    {
        if (climb)
            linkedPhysicsControl.rb.linearVelocity =
                new Vector2(0, linkedInput.verticalInput * climbSpeed);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(ladderParameterInt, linkedStateMachine.currentState == PlayerStates.State.Ladders);
    }
}
