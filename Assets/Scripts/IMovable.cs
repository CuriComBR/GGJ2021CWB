using UnityEngine;

public interface IMovable
{
    public bool IsWalking();
    public Direction GetFacingDirection();
    public Vector3 GetTransformDirection();
    public Animator GetAnimator();
    public Transform GetTransform();
    public float GetSpeed();
}