using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    private void Update()
    {
        Vector2 inputVector = Vector2.zero;
        // for movement use isPressed(for continuously movement) 
        // for jump,.. use wasPressedThisFrame (return true for single frame when key is pressed -  single frame action)
        if (Keyboard.current.wKey.isPressed)
        {
            inputVector.y += 1;
        }
        if (Keyboard.current.sKey.isPressed)
        {
            inputVector.y -= 1;
        }
        if (Keyboard.current.aKey.isPressed)
        {
            inputVector.x -= 1;
        }
        if (Keyboard.current.dKey.isPressed)
        {
            inputVector.x += 1;
        }
        // normalize input vector to avoid faster diagonal movement
        inputVector = inputVector.normalized;
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);
        //deltaTime = 1/frames per second
        transform.position += moveDir * moveSpeed * Time.deltaTime ;
    }
}
