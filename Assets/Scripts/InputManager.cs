using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance {  get; private set; }

    public static event Action<Vector2> OnMoveEvent;
    public static event Action OnJumpEvent;
    public static event Action OnAttackEvent;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveInput = context.ReadValue<Vector2>();
        OnMoveEvent?.Invoke(moveInput);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpEvent?.Invoke();
        }
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackEvent?.Invoke();
        }
    }

     
}
