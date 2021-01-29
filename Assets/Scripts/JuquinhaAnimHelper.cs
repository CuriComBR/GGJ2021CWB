using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuquinhaAnimHelper
{   
    Animator anim;

    Direction actualDirection;
    bool isWalking = false;

    public enum Direction
    {
        DOWN = 0, RIGHT = 1, UP = 2, LEFT = 3
    }

    public JuquinhaAnimHelper(Direction startDirection, Animator anim)
    {
        SetDirection(startDirection);
        this.anim = anim;
    }

    public void SetDirection(Direction direction)
    {
        actualDirection = direction;
        anim.SetInteger("direction", (int)actualDirection);
    }
    
    public void SetIsWalking(bool isWalking)
    {
        this.isWalking = isWalking;
        anim.SetBool("walking", isWalking);
    }

    public bool IsWalking()
    {
        return isWalking;
    }
}
