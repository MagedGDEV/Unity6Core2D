using System.Collections;
using UnityEngine;

public class KnockBackAbility : BaseAbility
{

    private Coroutine currentKnockBack;
    
    private string knockBackAnimParameterName= "KnockBack";
    private int knockBackParamterInt;

    protected override void Initialization()
    {
        base.Initialization();
        knockBackParamterInt = Animator.StringToHash(knockBackAnimParameterName);
    }

    public override void EnterAbility()
    {
        currentKnockBack = null;
    }

    public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
    {
        if (currentKnockBack == null)
        {
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
        else
        {
            // Do nothing or stop current one and start new one
            StopCoroutine(currentKnockBack);
            currentKnockBack = StartCoroutine(KnockBack(duration, force, enemyObject));
        }
    }

    private IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
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

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(knockBackParamterInt, linkedStateMachine.currentState == PlayerStates.State.KnockBack);
    }
}
