using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager _instance { get; private set; }

    [HideInInspector] public InputActions _inputActions;

    private void Awake()
    {
        _instance = this;

        _inputActions = new InputActions();
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }
}