using UnityEngine;
using UnityEngine.InputSystem;

public class GatherInput : MonoBehaviour
{
    public PlayerInput playerInput;

    public InputActionReference moveActionRef;

    private InputActionMap playerMap;
    private InputActionMap uiMap;

    [HideInInspector]
    public float horizontalInput;

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        playerMap.Disable();
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
