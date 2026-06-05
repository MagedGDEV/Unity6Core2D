using UnityEngine;
using UnityEngine.InputSystem;

public class CrouchAbility: BaseAbility
{
    public InputActionReference crouchActionReference;
    public float crouchSpeed;
    private bool wantToStop;
    
    private string crouchAnimParameterName = "Crouch";
    private string xSpeedAnimParameterName = "xSpeed";
    private int crouchAnimParameterInt;
    private int xSpeedAnimParameterInt;

    public override void EnterAbility()
    {
        linkedPhysicsControl.CrouchColliders();
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.StandColliders();
        wantToStop = false;
    }

    protected override void Initialization()
    {
        base.Initialization();
        crouchAnimParameterInt = Animator.StringToHash(crouchAnimParameterName);
        xSpeedAnimParameterInt = Animator.StringToHash(xSpeedAnimParameterName);
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
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if (!linkedPhysicsControl.grounded ||
            linkedStateMachine.currentState == PlayerStates.State.Crouch ||
            linkedStateMachine.currentState == PlayerStates.State.Dash ||
            linkedStateMachine.currentState == PlayerStates.State.Ladders)
            return;
        
        wantToStop = false;
        linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
    }

    private void StopCrouch(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;

        if (linkedStateMachine.currentState != PlayerStates.State.Crouch)
            return;

        if (linkedPhysicsControl.ceilingDetected)
        {
            wantToStop = true;
            return;
        }

        linkedStateMachine.ChangeState(linkedInput.horizontalInput == 0
            ? PlayerStates.State.Idle
            : PlayerStates.State.Run);
    }

    public override void ProcessAbility()
    {
        player.Flip();
        
        if(wantToStop && !linkedPhysicsControl.ceilingDetected)
            linkedStateMachine.ChangeState(linkedInput.horizontalInput == 0
                ? PlayerStates.State.Idle
                : PlayerStates.State.Run);
        
        if(!linkedPhysicsControl.grounded)
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
    }

    public override void ProcessFixedAbility()
    {
        if (linkedPhysicsControl.grounded)
            linkedPhysicsControl.rb.linearVelocity =
                new Vector2(linkedInput.horizontalInput * crouchSpeed, linkedPhysicsControl.rb.linearVelocityY);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(crouchAnimParameterInt, linkedStateMachine.currentState == PlayerStates.State.Crouch);
        linkedAnimator.SetFloat(xSpeedAnimParameterInt, Mathf.Abs(linkedPhysicsControl.rb.linearVelocityX));
    }
}
