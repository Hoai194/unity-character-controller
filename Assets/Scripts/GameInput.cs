using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    [SerializeField] private InputActionReference input, interact;
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
