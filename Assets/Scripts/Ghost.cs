using UnityEngine;


public class Ghost : MonoBehaviour, IMovable
{
    [Header("References")]
    [SerializeField] private Animator Animator;
    [SerializeField] private GameObject vanishEffect;

    [Header("General")]
    [SerializeField] private float Speed = 1.0f;
    
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius;
    [SerializeField] private float minRangeChase;
    [SerializeField] private float maxRangeChase;
    [SerializeField] private bool isPatroling;
    [SerializeField] private Transform[] patrolPoints;

    [SerializeField] private float freezeTime = 5f;

    [SerializeField] private AudioClip groomingSound;

    private AudioSource audioSource;

    private int patrolIndex = 0;
    private bool reversePatrol = false;

    private StateMachine stateMachine;

    private Vector3 transformDirection;
    private Direction facingDirection = Direction.Down;

    private AnimationController animationController;
    private MovementController movementController;

    #region LifeCycle

    private void Awake()
    {
        animationController = gameObject.AddComponent<AnimationController>();
        animationController.SetTarget(this);

        movementController = gameObject.AddComponent<MovementController>();
        movementController.SetTarget(this);

        stateMachine = new StateMachine();

        audioSource = GetComponent<AudioSource>();

        InvokeRepeating("PlayGroomingSound", Random.Range(0f, 2f), Random.Range(1, 3f));
    }

    private void Update()
    {
        if (isPatroling)
        {
            Patrol();
        }
        else
        {
            Chase();
        }
    }
    
    #endregion

    #region Collision

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.instance.Freeze(freezeTime);
            animationController.SetTarget(null);
            movementController.SetTarget(null);
            Destroy(gameObject);
            Instantiate(vanishEffect, transform.position, Quaternion.identity);
        }
    }

    #endregion
    
    #region InterfaceImplementation

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

    #endregion

    private void Chase()
    {
        float distance = Vector2.Distance(transform.position, Player.instance.transform.position);

        if (isPatroling && distance > maxRangeChase)
        {
            Patrol();
        }
        else
        {
            Vector2 chaseDirection = Player.instance.transform.position - transform.position;
            ConfigureDirections(GetFacingDirectionByTargetPosition(chaseDirection), chaseDirection.normalized);
        }
    }

    private void PlayGroomingSound()
    {
        audioSource.clip = groomingSound;
        audioSource.Play();
    }

    private void Patrol()
    {
        if (Vector2.Distance(transform.position, Player.instance.transform.position) <= minRangeChase)
        {
            Chase();
        }
        else
        {
            Transform patrolPoint = patrolPoints[patrolIndex];
            if (Vector2.Distance(patrolPoint.position, transform.position) < patrolRadius)
            {
                if (patrolIndex == patrolPoints.Length - 1)
                {
                    reversePatrol = true;
                }
                else if (patrolIndex == 0)
                {
                    reversePatrol = false;
                }

                if (reversePatrol) patrolIndex--;
                else patrolIndex++;
                patrolPoint = patrolPoints[patrolIndex];
            }

            Vector2 patrolDirection = patrolPoint.position - transform.position;
            ConfigureDirections(GetFacingDirectionByTargetPosition(patrolDirection), patrolDirection.normalized);
        }
    }
    
    private Direction GetFacingDirectionByTargetPosition(Vector2 chaseDirection)
    {
        Direction fDirection = Direction.Down;

        float angle = Vector2.SignedAngle(transform.right, chaseDirection);

        if (angle > -50 && angle < 50)
        {
            fDirection = Direction.Right;
        }
        else if (angle >= 50 && angle <= 130)
        {
            fDirection = Direction.Up;
        }
        else if (angle > 130 || angle < -130)
        {
            fDirection = Direction.Left;
        }
        else if (angle <= -50 && angle >= -130)
        {
            fDirection = Direction.Down;
        }

        return fDirection;
    }
    
    private void ConfigureDirections(Direction facingDirection, Vector3 transformDirection)
    {
        this.facingDirection = facingDirection;
        this.transformDirection = transformDirection;
    }
}