using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbility : BaseAbility
{
    public InputActionReference wallJumpActionRef;
    [SerializeField] private Vector2 wallJumpForce;
    [SerializeField] private float wallJumpMaxTime;
    private float wallJumpMinimumTime;
    private float wallJumpTimer;

    private void OnEnable()
    {
        wallJumpActionRef.action.performed += TryToWallJump;
    }

    private void OnDisable()
    {
        wallJumpActionRef.action.performed -= TryToWallJump;
    }

    protected override void Initialization()
    {
        base.Initialization();
        wallJumpTimer = wallJumpMaxTime;
    }

    private void TryToWallJump(InputAction.CallbackContext value)
    {
        if (!isPermitted)
            return;
        
        if (EvaluateWallJumpCondition())
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallJump);
            wallJumpTimer = wallJumpMaxTime;
            wallJumpMinimumTime = 0.15f;
            player.ForceFlip();
            if (player.facingRight)
                linkedPhysicsControl.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            else 
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
        }
    }

    public override void ProcessAbility()
    {
        wallJumpTimer -= Time.deltaTime;
        wallJumpMinimumTime -= Time.deltaTime;

        if (wallJumpTimer <= 0)
        {
            linkedStateMachine.ChangeState(linkedPhysicsControl.grounded
                ? PlayerStates.State.Idle
                : PlayerStates.State.Jump);
            return;
        }
        if (wallJumpMinimumTime <= 0 && linkedPhysicsControl.wallDetected)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            wallJumpTimer = -1;
        }
    }

    private bool EvaluateWallJumpCondition()
    {
        if (linkedPhysicsControl.grounded || !linkedPhysicsControl.wallDetected)
            return false;
        return true;
    }
}
