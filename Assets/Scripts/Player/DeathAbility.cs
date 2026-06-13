using UnityEngine;

public class DeathAbility : BaseAbility
{
    private string DeathAnimParameterName= "Death";
    private int DeathParameterInt;


    public override void EnterAbility()
    {
        player.gatherInput.DisablePlayerInput();
        linkedPhysicsControl.ResetVelocity();
    }

    protected override void Initialization()
    {
        base.Initialization();
        DeathParameterInt = Animator.StringToHash(DeathAnimParameterName);
    }

    public override void UpdateAnimator()
    {
        if(linkedPhysicsControl.grounded)
            linkedAnimator.SetBool(DeathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        else
        {
            // TOD0:: Add air death animation
            linkedAnimator.SetBool(DeathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Reset Game");
    }
}
