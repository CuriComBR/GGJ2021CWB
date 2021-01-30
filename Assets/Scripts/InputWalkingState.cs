using UnityEngine;

public class InputWalkingState : State
{
    public InputWalkingState(IMovable context) : base(context) { }
    
    Animator animator;

    public override void Tick()
    {
        Transform contextTransform = context.GetTransform();
        contextTransform.position += Time.deltaTime * context.GetSpeed() * context.GetTransformDirection().normalized;
    }

    public override void OnEnter()
    {
        animator = context.GetAnimator();
        int facingDirection = (int) context.GetFacingDirection();
        animator.SetBool("isWalking", true);
        animator.SetInteger("direction", facingDirection);
    }

    public override void OnExit()
    {
        animator.SetBool("isWalking", false);
    }
}