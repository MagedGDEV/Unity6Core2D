using System.Collections;
using UnityEngine;

public class KnockBackAbility : BaseAbility
{

    public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
        linkedPhysicsControl.ResetVelocity();
        linkedPhysicsControl.rb.linearVelocity = (transform.position.x >= enemyObject.transform.position.x) ? force : new Vector2(-force.x, force.y);
        
        yield return new WaitForSeconds(duration);
        
        // return to other states
        if (player.playerStats.GetCurrentHealth() > 0)
        {
            if (linkedPhysicsControl.grounded)
                linkedStateMachine.ChangeState(linkedInput.horizontalInput != 0
                    ? PlayerStates.State.Run
                    : PlayerStates.State.Idle);
            else
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
        }
        else
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Death);
        }
    }
}
