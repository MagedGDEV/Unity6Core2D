using UnityEngine;

public class DeathAbility : BaseAbility
{
    private string deathAnimParameterName= "Death";
    private int deathParameterInt;


    public override void EnterAbility()
    {
        player.gatherInput.DisablePlayerInput();
        linkedPhysicsControl.ResetVelocity();
    }

    protected override void Initialization()
    {
        base.Initialization();
        deathParameterInt = Animator.StringToHash(deathAnimParameterName);
    }

    public override void UpdateAnimator()
    {
        if(linkedPhysicsControl.grounded)
            linkedAnimator.SetBool(deathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        else
        {
            // TOD0:: Add air death animation
            linkedAnimator.SetBool(deathParameterInt, linkedStateMachine.currentState == PlayerStates.State.Death);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Reset Game");
    }
}
