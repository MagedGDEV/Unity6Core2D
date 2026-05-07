using UnityEngine;

public class Player : MonoBehaviour
{
    public GatherInput gatherInput;
    public StateMachine stateMachine;
    public PhysicsControl physicsControl;
    public Animator anim;

    private BaseAbility[] playerAbilities;
    public bool facingRight = true;

    private void Awake()
    {
        stateMachine = new StateMachine();
        playerAbilities = GetComponents<BaseAbility>();
        stateMachine.arrayOfAbilities = playerAbilities;
    }

    private void Update()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if(ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessAbility();
            }
            ability.UpdateAnimator();
        }
        Debug.Log("Current State is " + stateMachine.currentState);
    }

    private void FixedUpdate()
    {
        foreach(BaseAbility ability in playerAbilities)
        {
            if (ability.thisAbilityState == stateMachine.currentState)
            {
                ability.ProcessFixedAbility();
            }
        }
    }

    public void Flip()
    {
        if (facingRight && gatherInput.horizontalInput < 0)
        {
            transform.Rotate(0,180,0);
            facingRight = !facingRight;
        }
        else if (!facingRight && gatherInput.horizontalInput > 0)
        {
            transform.Rotate(0,180,0);
            facingRight = !facingRight;
        }
    }
}
