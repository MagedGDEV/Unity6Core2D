using UnityEngine;
using UnityEngine.InputSystem;

public class MultipleJumpAbility : BaseAbility
{
    [SerializeField] private InputActionReference jumpActionRef;
    
    [Header("Multiple Jumping")]
    [SerializeField] private int maxNumberOfJumps;
    private int numberOfJumps;
    private bool canActivateAdditionalJumps;
    
    [Header("Variable Jumping")]
    [SerializeField] private float setMaxJumpTime;
    [SerializeField] private float gravityDivider;
    private float jumpTimer;
    private bool jumping;
    
    [Header("Jumping")]
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

        if (linkedStateMachine.currentState == PlayerStates.State.Ladders)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocity = 
                new Vector2
                (
                    airSpeed * linkedInput.horizontalInput,
                    0
                );
            
            minimumAirTime = startMinimumAirTime;
            jumpTimer = setMaxJumpTime;
            jumping = true;
            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps--;
        }

        else if (linkedPhysicsControl.coyoteTimer > 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            linkedPhysicsControl.rb.linearVelocity = 
                new Vector2
                (
                    airSpeed * linkedInput.horizontalInput,
                    jumpForce
                );
            linkedPhysicsControl.coyoteTimer = -1;
            
            minimumAirTime = startMinimumAirTime;
            jumpTimer = setMaxJumpTime;
            jumping = true;
            numberOfJumps = maxNumberOfJumps;
            canActivateAdditionalJumps = true;
            numberOfJumps--;
        }
        
        else if (numberOfJumps > 0 && canActivateAdditionalJumps)
        {
           linkedPhysicsControl.EnableGravity();
            linkedPhysicsControl.rb.linearVelocity = 
                new Vector2
                (
                    airSpeed * linkedInput.horizontalInput,
                    jumpForce
                );
            linkedPhysicsControl.coyoteTimer = -1;
            
            minimumAirTime = startMinimumAirTime;
            jumpTimer = setMaxJumpTime;
            jumping = true;
            numberOfJumps--;
        }
        else
        {
            canActivateAdditionalJumps = false;
        }
    }

    private void StopJump(InputAction.CallbackContext value)
    {
        jumping = false;
    }

    protected override void Initialization()
    {
        base.Initialization();
        startMinimumAirTime = minimumAirTime;
        jumpParamterInt = Animator.StringToHash(jumpAnimParamterName);
        ySpeedParameterInt = Animator.StringToHash(ySpeedAnimParameterName);
        numberOfJumps = maxNumberOfJumps;
    }

    public override void ProcessAbility()
    {
        player.Flip();
        minimumAirTime -=Time.deltaTime;

        if (jumping)
        {
            jumpTimer -= Time.deltaTime;
            if (jumpTimer <= 0)
                jumping = false;
        }
        
        if (linkedPhysicsControl.grounded && minimumAirTime < 0)
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        else if (!linkedPhysicsControl.grounded && linkedPhysicsControl.wallDetected)
            if (linkedPhysicsControl.rb.linearVelocity.y < 0)
                linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
    }

    public override void ProcessFixedAbility()
    {
        if (!linkedPhysicsControl.grounded)
        {
            if (jumping)
                linkedPhysicsControl.rb.linearVelocity =
                    new Vector2(
                        airSpeed * linkedInput.horizontalInput,
                        jumpForce);
            else
                linkedPhysicsControl.rb.linearVelocity =
                    new Vector2(
                        airSpeed * linkedInput.horizontalInput,
                        linkedPhysicsControl.rb.linearVelocityY);
        }

        if (linkedPhysicsControl.rb.linearVelocity.y < 0)
        {
            linkedPhysicsControl.rb.gravityScale = linkedPhysicsControl.GetGravity() / gravityDivider;
        }
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.EnableGravity();
        canActivateAdditionalJumps = false;
    }

    public override void UpdateAnimator()
    {
        linkedAnimator.SetBool(jumpParamterInt,
            linkedStateMachine.currentState == PlayerStates.State.Jump ||
            linkedStateMachine.currentState == PlayerStates.State.WallJump
            );
        linkedAnimator.SetFloat(ySpeedParameterInt, linkedPhysicsControl.rb.linearVelocityY);
    }
    
    public void SetMaxJumpTime(int maxJumps)
    {
        maxNumberOfJumps = maxJumps;
    }
}
