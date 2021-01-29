using System;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private IMovable target;

    public void SetTarget(IMovable target)
    {
        this.target = target;
    }

    private void Update()
    {
        Animator animator = target.GetAnimator();
        animator.SetInteger("direction", (int) target.GetFacingDirection());
        animator.SetBool("walking", target.IsWalking());
    }
}