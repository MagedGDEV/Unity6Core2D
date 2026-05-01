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

    public override void ProcessAbility()
    {
        Debug.Log("This is IDLE ability");
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(idleParamterInt, linkedStateMachine.currentState == PlayerStates.State.Idle);
    }
}
