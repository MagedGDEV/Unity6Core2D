using UnityEngine;
using UnityEngine.InputSystem;

public class DashAbility: BaseAbility
{
    public InputActionReference dashActionRef;
    [SerializeField] private float dashForce;
    [SerializeField] private float maxDashDuration;
    private float dashTimer;
    
    private string dashAnimParameterName= "Dash";
    private int dashParameterInt;

    private void OnEnable()
    {
        dashActionRef.action.performed += TryToDash;
    }

    private void OnDisable()
    {
        dashActionRef.action.performed -= TryToDash;
    }

    protected override void Initialization()
    {
        base.Initialization();
        dashParameterInt = Animator.StringToHash(dashAnimParameterName);
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        player.playerStats.EnableDamage();
    }

    public override void EnterAbility()
    {
        player.playerStats.DisableDamage();
    }

    private void TryToDash(InputAction.CallbackContext value)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            return;

        if (linkedStateMachine.currentState == PlayerStates.State.Dash ||
            linkedStateMachine.currentState == PlayerStates.State.Crouch ||
            linkedPhysicsControl.wallDetected)
            return;
        
        linkedStateMachine.ChangeState(PlayerStates.State.Dash);
        linkedPhysicsControl.DisableGravity();
        linkedPhysicsControl.ResetVelocity();
        if (player.facingRight)
            linkedPhysicsControl.rb.linearVelocityX = dashForce;
        else
            linkedPhysicsControl.rb.linearVelocityX = -dashForce;

        dashTimer = maxDashDuration;
    }

    public override void ProcessAbility()
    {
        dashTimer -= Time.deltaTime;
        if (linkedPhysicsControl.wallDetected)
            dashTimer = -1;
        if (dashTimer <= 0)
        {
            linkedStateMachine.ChangeState(linkedPhysicsControl.grounded
                ? PlayerStates.State.Idle
                : PlayerStates.State.Jump);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(dashParameterInt, linkedStateMachine.currentState == PlayerStates.State.Dash);
    }
}
