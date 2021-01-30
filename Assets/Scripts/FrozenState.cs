using UnityEngine;

public class FrozenState : State
{

    private Animator animator;
    
    public FrozenState(IMovable context) : base(context) { }

    public override void Tick()
    {
        Debug.Log("Frozen!");
    }

    public override void OnEnter()
    {
        animator = context.GetAnimator();
        animator.SetBool("isWalking", false);
        animator.SetBool("frozen", true);
    }

    public override void OnExit()
    {
        animator.SetBool("frozen", false);
    }
}