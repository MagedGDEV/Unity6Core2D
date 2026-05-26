using UnityEngine;
using UnityEngine.PlayerLoop;

public class WallSlideAbility : BaseAbility
{
    [SerializeField] private float maxWallSlideSpeed;
    
    private string wallSlideAnimParameterName = "WallSlide";
    private int wallSlideParameterInt;

    protected override void Initialization()
    {
        base.Initialization();
        wallSlideParameterInt = Animator.StringToHash(wallSlideAnimParameterName);
    }

    public override void ProcessAbility()
    {
        if (linkedPhysicsControl.grounded)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        else if (player.facingRight && linkedInput.horizontalInput < 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        else if (!player.facingRight && linkedInput.horizontalInput > 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        else if (!linkedPhysicsControl.wallDetected)
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
    }

    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocityY = Mathf.Clamp(
            linkedPhysicsControl.rb.linearVelocityY,
            -maxWallSlideSpeed,
            1);
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(wallSlideParameterInt,
            linkedStateMachine.currentState == PlayerStates.State.WallSlide);
    }
}
