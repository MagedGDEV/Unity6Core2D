using System;
using UnityEngine;

public class MoveAbility : BaseAbility
{
    [SerializeField] private float speed;

    private string runAnimParameterName= "Run";
    private int runParamterInt;

    protected override void Initialization()
    {
        base.Initialization();
        runParamterInt = Animator.StringToHash(runAnimParameterName);
    }

    public override void ProcessAbility()
    {
        if (linkedInput.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }

    public override void ProcessFixedAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = 
            new Vector2
            (
                speed * linkedInput.horizontalInput,
                linkedPhysicsControl.rb.linearVelocityY
            );
    }

    public override void UpdateAnimator()
    {
        Debug.Log("Hello world");
        linkedAnimator.SetBool(runParamterInt, linkedStateMachine.currentState == PlayerStates.State.Run);
    }
}
