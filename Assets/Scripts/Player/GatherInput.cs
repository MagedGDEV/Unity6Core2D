using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    public InputActionReference jumpActionRef;
    public InputActionReference moveActionRef;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    [HideInInspector]
    public float horizontalInput;

    private void OnEnable()
    {
        jumpActionRef.action.performed += TryToJump;
        jumpActionRef.action.canceled += StopJump;
    }

    private void OnDisable()
    {
        jumpActionRef.action.performed -= TryToJump;
        jumpActionRef.action.canceled -= StopJump;
        playerMap.Disable();
    }

    private void TryToJump(InputAction.CallbackContext value)
    {
        Debug.Log("JUMP");
    }

    private void StopJump(InputAction.CallbackContext value)
    {
        Debug.Log("STOP JUMP");
    }

    void Start()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");
        playerMap.Enable();
    }

    void Update()
    {
        horizontalInput = moveActionRef.action.ReadValue<float>();
        Debug.Log("Horizontal Input = " + horizontalInput);
    }
}
