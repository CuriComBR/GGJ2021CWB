using System;
using UnityEngine;


public class Ghost : MonoBehaviour, IMovable
{
    [Header("References")]
    [SerializeField] private Animator Animator;
    private Player player;

    [Header("General")]
    [SerializeField] private float Speed = 1.0f;
    
    [Header("Patrol Settings")]
    [SerializeField] private float patrolRadius;
    [SerializeField] private float minRangeChase;
    [SerializeField] private float maxRangeChase;
    [SerializeField] private bool isPatroling;
    [SerializeField] private Transform[] patrolPoints;

    private int patrolIndex = 0;
    private bool reversePatrol = false;

    private Vector3 transformDirection;
    private Direction facingDirection = Direction.Down;

    private AnimationController animationController;
    private MovementController movementController;

    private void Awake()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
        if (gameObject)
            player = gameObject.GetComponent<Player>();

        animationController = gameObject.AddComponent<AnimationController>();
        animationController.SetTarget(this);

        movementController = gameObject.AddComponent<MovementController>();
        movementController.SetTarget(this);
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

    private void Chase()
    {
        if (isPatroling && Vector2.Distance(transform.position, player.transform.position) > maxRangeChase)
        {
            Patrol();
        }
        else
        {
            Vector2 chaseDirection = player.transform.position - transform.position;
            ConfigureDirections(GetFacingDirectionByTargetPosition(chaseDirection), chaseDirection.normalized);
        }
    }

    private void Patrol()
    {
        if(Vector2.Distance(transform.position, player.transform.position) <= minRangeChase)
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

        if(angle > -50 && angle < 50)
        {
            fDirection = Direction.Right;
        }
        else if(angle >= 50 && angle <= 130)
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

    private void OnDrawGizmos()
    {
    
    }

   
    private void SetDirectionsFromInput()
    {
        
        ConfigureDirections(Direction.Left, Vector3.left);
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

    private void ConfigureDirections(Direction facingDirection, Vector3 transformDirection)
    {
        this.facingDirection = facingDirection;
        this.transformDirection = transformDirection;
    }
}