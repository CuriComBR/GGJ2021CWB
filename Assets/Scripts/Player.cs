using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private Animator anim;

    private Direction actualDirection;
    private bool isWalking = false;
    
    public enum Direction
    {
        DOWN = 0, RIGHT = 1, UP = 2, LEFT = 3
    }

    void Update()
    {
        var direction = GetDirection();
        Move(direction);
        SetIsWalking(!direction.Equals(Vector3.zero));
    }

    private void Move(Vector3 direction)
    {
        transform.position += Time.deltaTime * Speed * direction;
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
        {
            SetDirection(Direction.UP);
            direction += Vector3.up;
        }

        if (Input.GetKey(KeyCode.S))
        {
            SetDirection(Direction.DOWN);
            direction += Vector3.down;
        }

        if (Input.GetKey(KeyCode.A))
        {
            SetDirection(Direction.LEFT);
            direction += Vector3.left;
        }

        if (Input.GetKey(KeyCode.D))
        {
            SetDirection(Direction.RIGHT);
            direction += Vector3.right;
        }

        return direction;
    }
    
    public void SetDirection(Direction direction)
    {
        actualDirection = direction;
        anim.SetInteger("direction", (int)actualDirection);
    }
    
    public void SetIsWalking(bool isWalking)
    {
        this.isWalking = isWalking;
        anim.SetBool("walking", isWalking);
    }
}