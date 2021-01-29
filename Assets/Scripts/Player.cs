using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float Speed = 1.0f;
    
    void Update()
    {
        Move(GetDirection());
    }

    private void Move(Vector3 direction)
    {
        transform.position += Time.deltaTime * Speed * direction;
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(KeyCode.W)) direction += Vector3.up;
        if (Input.GetKey(KeyCode.S)) direction += Vector3.down;
        if (Input.GetKey(KeyCode.A)) direction += Vector3.left;
        if (Input.GetKey(KeyCode.D)) direction += Vector3.right;

        return direction;
    }
}