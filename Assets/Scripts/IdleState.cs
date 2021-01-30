using UnityEngine;

public class IdleState : State
{
    public IdleState(IMovable context) : base(context) { }

    public override void Tick() { }

    public override void OnEnter()
    {
        Animator animator = context.GetAnimator();
        int facingDirection = (int) context.GetFacingDirection();
        animator.SetBool("isWalking", false);
        animator.SetInteger("direction", facingDirection);
    }

    public override void OnExit() { }
}