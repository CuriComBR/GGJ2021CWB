using UnityEngine;

public interface IMovable
{
    public bool IsWalking();
    public Direction GetDirection();
    public Animator GetAnimator();
    public Transform GetTransform();
}