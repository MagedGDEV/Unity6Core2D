using UnityEngine;
using UnityEngine.InputSystem;

public class JumpAbility : BaseAbility
{

    [SerializeField] private InputActionReference jumpActionRef;
    [SerializeField] private float jumpForce;
    [SerializeField] private float airSpeed;
    [SerializeField] private float minimumAirTime;
    private float startMinimumAirTime;

    private string jumpAnimParamterName = "Jump";
    private string ySpeedAnimParameterName = "ySpeed";
    private int jumpParamterInt;
    private int ySpeedParameterInt;

    private void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
        jumpActionRef.action.canceled += StopJump;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;
    }

    private void TryToJump(InputAction.CallbackContext value)
    {
        if(!isPermitted)
            return;

        if (linkedPhysicsControl.grounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocity = 
                new Vector2
                (
                    airSpeed * linkedInput.horizontalInput,
                    jumpForce
                );
            minimumAirTime = startMinimumAirTime;
        }
    }

    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("STOP JUMP");
    }

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
        jumpParamterInt = Animator.StringToHash(jumpAnimParamterName);
        ySpeedParameterInt = Animator.StringToHash(ySpeedAnimParameterName);
    }

    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTime -=Time.deltaTime;
        if (linkedPhysicsControl.grounded && minimumAirTime < 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
    }

    public override void ProcessFixedAbility()
    {
        if (!linkedPhysicsControl.grounded)
            linkedPhysicsControl.rb.linearVelocity =
                new Vector2
                (
                    airSpeed * linkedInput.horizontalInput,
                    linkedPhysicsControl.rb.linearVelocityY
                );
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParamterInt,
            linkedStateMachine.currentState == PlayerStates.State.Jump ||
            linkedStateMachine.currentState == PlayerStates.State.WallJump
            );
        linkedAnimator.SetFloat(ySpeedParameterInt, linkedPhysicsControl.rb.linearVelocityY);
    }
}
