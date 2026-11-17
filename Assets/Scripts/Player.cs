using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Instance is defined as a property by this way
    public static Player Instance { get; private set; }

    //Instance is defined as a field by this way
    // public static Player instance;
    //public static GetInstance() { return instance; }
    //public static SetInstance(Player player) { instance = player; }

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("There is more than one Player instance");
        }
        Instance = this;
    }

    public event EventHandler<OnSelectedAnimalChangedEventArgs> OnSelectedAnimalChanged;
    public class OnSelectedAnimalChangedEventArgs : EventArgs
    {
        public Animal selectedAnimal;
    }
    
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    // [SerializeField] private Animator animator;

    [SerializeField] private LayerMask animalsLayerMask;

    private Animal selectedAnimal;

    private Vector3 lastMoveDir;
    private bool isMoving;

    
    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if(selectedAnimal != null)
        {
            selectedAnimal.Interact();
        }

    }

    private void Update()
    {
        
        HandleMovement();
        HandleInteractions();
        
    }

    public bool IsMoving(){
        return isMoving;
    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        if(moveDir != Vector3.zero)
        {
            lastMoveDir = moveDir;
        }
       
        float interactDistance = 2f;
        
        // Raycast: shoot a ray from the player's position in the last move direction.
        // - interactDistance: max length of the ray.
        // - animalsLayerMask: the ray only detects colliders on the "Animals" layer,
        //   so it ignores walls, floor and other objects we don't want to interact with.
        if (Physics.Raycast(transform.position, lastMoveDir, out RaycastHit raycastHit, interactDistance, animalsLayerMask))
        {
            // TryGetComponent does 2 things at once:
            // 1. checks whether the hit object has an Animal component
            // 2. outputs that component so we can call its methods
            if (raycastHit.transform.TryGetComponent(out Animal animal))
            {
                // The object has an Animal component
                //Set animal as selected animal and publish event for other scripts to subscribe
                if(animal != selectedAnimal)
                {
                    // selectedAnimal = animal;
                    //publish and also send more data (selectedAnimal)
                    // OnSelectedAnimalChanged?.Invoke(this, new OnSelectedAnimalChangedEventArgs { selectedAnimal = selectedAnimal });
                    //refactor to use a method to set selected animal
                    SetSelectedAnimal(animal);
                }
                
            }
            else
            {
                // selectedAnimal = null;
                SetSelectedAnimal(null);
            }

            // Equivalent, longer version:
            // Animal animal = raycastHit.transform.GetComponent<Animal>();
            // if (animal != null)
            // {
            //     animal.Interact();
            // }
        }
        else
        {
            // The ray hit nothing on the Animals layer within interactDistance
            // selectedAnimal = null;
            SetSelectedAnimal(null);
            
        }

    }
    private void SetSelectedAnimal(Animal selectedAnimal)
    {
        this.selectedAnimal = selectedAnimal;
        OnSelectedAnimalChanged?.Invoke(this, new OnSelectedAnimalChangedEventArgs
        {
            selectedAnimal = selectedAnimal
        });
    }
    private void HandleMovement()
    {
        //input system refactored to GameInput.cs script
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if(!canMove)
        {
            //cannot move towards moveDir
            //attempt only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //cannot move towards moveDirX
                //attempt only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
                else
                {
                    //cannot move in any direction
                }

            }
            
        }
        //deltaTime = 1/frames per second
        if (canMove)
        {
            transform.position += moveDir * moveDistance;
        }

        isMoving = moveDir != Vector3.zero;
        if( isMoving)
        {
            float rotateSpeed = 10f;
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        }

        // animator.SetBool("isMoving", isMoving);

    }
}
