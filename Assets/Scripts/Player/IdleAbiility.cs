using UnityEngine;

public class IdleAbiility : BaseAbility
{
    private string idleAnimParameterName= "Idle";
    private int idleParamterInt;

    protected override void Initialization()
    {
        base.Initialization();
        idleParamterInt = Animator.StringToHash(idleAnimParameterName);
        // add more things
    }

    public override void EnterAbility()
    {
        linkedPhysicsControl.rb.linearVelocityX = 0;
    }

    public override void ProcessAbility()
    {
        if (linkedInput.horizontalInput != 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Run);
        }
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamterInt, linkedStateMachine.currentState == PlayerStates.State.Idle);
    }
}
