using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] private Transform ball;
    [SerializeField] private Transform ballSpawnPoint;
    public void Interact()
    {
        Debug.Log("Interacting with animal");
        Transform ballTransform = Instantiate(ball, ballSpawnPoint );
        ballTransform.localPosition = Vector3.zero;

    }
}
