using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    // Create an event that other scripts can subscribe to
    public event EventHandler OnInteractAction;
    [SerializeField] private InputActionReference input, interact;
    private void Awake()
    {
        input.action.Enable();
        interact.action.Enable();
        //Subscribe an InputSystem event named performed for E key pressed
        interact.action.performed += Interact_action_performed;
        
    }

    private void Interact_action_performed(InputAction.CallbackContext context)
    {
        //E key is pressed, do something?
        //Normally we would call a method in other scripts to handle the interaction, 
        // but following the Observer pattern, 
        // we will publish an event that other scripts can subscribe to,
        //  so they can handle the interaction by themselves when the E key is pressed (by subscribe event is published by GameInput).
    
        //Publish event for E key interaction
        OnInteractAction?.Invoke(this, EventArgs.Empty);

        // ?. null-conditional operator, equivalent for if(OnInteractAction != null) { OnInteractAction(this, EventArgs.Empty); }
    }
    
    public Vector2 GetMovementVectorNormalized()
    {
        //use input action reference to get input vector instead legacy input system
        Vector2 inputVector = input.action.ReadValue<Vector2>().normalized;
        // for movement use isPressed(for continuously movement) 
        // for jump,.. use wasPressedThisFrame (return true for single frame when key is pressed -  single frame action)
        // if (Keyboard.current.wKey.isPressed)
        // {
        //     inputVector.y += 1;
        // }
        // if (Keyboard.current.sKey.isPressed)
        // {
        //     inputVector.y -= 1;
        // }
        // if (Keyboard.current.aKey.isPressed)
        // {
        //     inputVector.x -= 1;
        // }
        // if (Keyboard.current.dKey.isPressed)
        // {
        //     inputVector.x += 1;
        // }

        
        // normalize input vector to avoid faster diagonal movement
        // inputVector = inputVector.normalized;
        return inputVector;
    }
}
