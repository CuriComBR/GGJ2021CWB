using UnityEngine;

public class InputWalkingState : State
{
    public InputWalkingState(IMovable context) : base(context) { }
    
    Animator animator;

    public override void Tick()
    {
        Transform contextTransform = context.GetTransform();
        contextTransform.position += Time.deltaTime * context.GetSpeed() * context.GetTransformDirection().normalized;
        int facingDirection = (int) context.GetFacingDirection();
        if (animator.GetInteger("direction") != facingDirection) animator.SetInteger("direction", facingDirection);
    }

    public override void OnEnter()
    {
        animator = context.GetAnimator();
        animator.SetBool("isWalking", true);
    }

    public override void OnExit()
    {
        animator.SetBool("isWalking", false);
    }
}