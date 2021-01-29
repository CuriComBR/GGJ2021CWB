using System;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private IMovable target;

    public void SetTarget(IMovable target)
    {
        this.target = target;
    }

    private void Update()
    {
        Transform targetTransform = target.GetTransform();
        targetTransform.position += Time.deltaTime * target.GetSpeed() * target.GetTransformDirection();
    }
}