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
        if (target != null) {
            Animator animator = target.GetAnimator();
            animator.SetInteger("direction", (int)target.GetFacingDirection());
            animator.SetBool("isWalking", target.IsWalking());
        }  
    }
}