using System;
using UnityEditor.Animations;
using UnityEngine;

public class Player : MonoBehaviour, IMovable
{
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private Animator Animator;

    private Vector3 transformDirection;
    private Direction facingDirection = Direction.Down;

    private AnimationController animationController;
    private MovementController movementController;

    private void Awake()
    {
        animationController = gameObject.AddComponent<AnimationController>();
        animationController.SetTarget(this);

        movementController = gameObject.AddComponent<MovementController>();
        movementController.SetTarget(this);
    }

    private void Update()
    {
        SetDirectionsFromInput();
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
        transformDirection = Vector3.zero;
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