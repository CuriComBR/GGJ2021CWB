using UnityEngine;

public class Player : MonoBehaviour, IMovable
{
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private Animator Animator;

    public static Player Instance;

    private Vector3 transformDirection;
    private Direction facingDirection = Direction.Down;

    private AnimationController animationController;
    private MovementController movementController;

    private bool isFrozen;
    private float releaseTime;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
        
        animationController = gameObject.AddComponent<AnimationController>();
        animationController.SetTarget(this);

        movementController = gameObject.AddComponent<MovementController>();
        movementController.SetTarget(this);
    }

    public void Freeze(float freezeTime)
    {
        releaseTime = Time.time + freezeTime;
        isFrozen = true;
        Animator.SetBool("freezed", true);
    }

    private void Update()
    {
        if (!isFrozen)
        {
            SetDirectionsFromInput();
        }
        else
        {
            if (Time.time >= releaseTime)
            {
                isFrozen = false;
                Animator.SetBool("freezed", false);
            }
            else
            {
                transformDirection = Vector2.zero;
            }
        }
        
    }
    
    public bool IsWalking()
    {
        return !transformDirection.Equals(Vector3.zero);
    }

    public Direction GetFacingDirection()
    {
        return facingDirection;
    }

    public Vector3 GetTransformDirection()
    {
        return transformDirection;
    }

    public Animator GetAnimator()
    {
        return Animator;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public float GetSpeed()
    {
        return Speed;
    }

    private void SetDirectionsFromInput()
    {
        transformDirection = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            ConfigureDirections(Direction.Up, Vector3.up);
        }

        if (Input.GetKey(KeyCode.S))
        {
            ConfigureDirections(Direction.Down, Vector3.down);
        }

        if (Input.GetKey(KeyCode.A))
        {
            ConfigureDirections(Direction.Left, Vector3.left);
        }

        if (Input.GetKey(KeyCode.D))
        {
            ConfigureDirections(Direction.Right, Vector3.right);
        }
    }

    private void ConfigureDirections(Direction facingDirection, Vector3 transformDirection)
    {
        this.facingDirection = facingDirection;
        this.transformDirection += transformDirection;
    }
}