using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility : BaseAbility
{
    public InputActionReference crouchActionReference;
    
    private string crouchAnimParameterName = "Crouch";
    private int crouchAnimParameterInt;

    public override void EnterAbility()
    {
        linkedPhysicsControl.CrouchColliders();
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.StandColliders();
    }

    protected override void Initialization()
    {
        base.Initialization();
        crouchAnimParameterInt = Animator.StringToHash(crouchAnimParameterName);
    }

    private void OnEnable()
    {
        crouchActionReference.action.performed += TryToCrouch;
        crouchActionReference.action.canceled += StopCrouch;
    }

    private void OnDisable()
    {
        crouchActionReference.action.performed -= TryToCrouch;
        crouchActionReference.action.canceled -= StopCrouch;
    }

    private void TryToCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;

        if (!linkedPhysicsControl.grounded ||
            linkedStateMachine.currentState == PlayerStates.State.Crouch ||
            linkedStateMachine.currentState == PlayerStates.State.Dash ||
            linkedStateMachine.currentState == PlayerStates.State.Ladders)
            return;
        
        linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
    }

    private void StopCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;

        if (linkedStateMachine.currentState != PlayerStates.State.Crouch)
            return;

        linkedStateMachine.ChangeState(linkedInput.horizontalInput == 0
            ? PlayerStates.State.Idle
            : PlayerStates.State.Run);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchAnimParameterInt, linkedStateMachine.currentState == PlayerStates.State.Crouch);
    }
}
