using UnityEngine;

public class FrozenState : State
{
    private Animator animator;

    public FrozenState(IMovable context) : base(context) { }

    public override void Tick() { }

    public override void OnEnter()
    {
        animator = context.GetAnimator();
        animator.SetBool("frozen", true);
    }

    public override void OnExit()
    {
        animator.SetBool("frozen", false);
    }
}