using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class InputAdapter : MonoBehaviour
{
    [SerializeField] private PlayerInput _input;
    private InputAction _pointAction;
    private InputAction _pressAction;


    // 外部に通知するイベント
    public event Action<Vector2> OnPointerMove;
    public event Action<bool> OnPointerPress;
    public event Action OnPointerRelease;

    private void Awake()
    {
        _pointAction = _input.actions["Point"];
        _pressAction = _input.actions["Press"];
    }

    private void OnEnable()
    {
        _input.actions.Enable();
        _pointAction.performed += Pointer;
        _pressAction.performed += Press;
        _pressAction.canceled += Release;
    }

    private void OnDisable()
    {
        _input.actions.Disable();
        _pointAction.performed -= Pointer;
        _pressAction.performed -= Press;
        _pressAction.canceled -= Release;
    }

    private void Pointer(InputAction.CallbackContext ctx)
    {
        OnPointerMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    private void Press(InputAction.CallbackContext ctx)
    {
        OnPointerPress?.Invoke(true);
    }

    private void Release(InputAction.CallbackContext ctx)
    {
        OnPointerPress?.Invoke(false);
        OnPointerRelease?.Invoke();
    }
}