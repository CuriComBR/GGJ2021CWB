using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IMovable
{
    [SerializeField] private float Speed = 1.0f;
    [SerializeField] private Animator Animator;

    [SerializeField] private GameObject interactionPopup;

    [SerializeField] private AudioClip interactionSound;

    [SerializeField] private AudioSource sourceSteps;
    [SerializeField] private AudioSource sourceOtherSounds;



    public static Player instance;

    private StateMachine stateMachine;

    private Dictionary<KeyCode, Tuple<Vector3, Direction>> keys;

    private Vector3 transformDirection;

    private bool isLocked;

    private Direction facingDirection = Direction.Down;

    private bool interacted = false;

    private Coroutine releaseCoroutine;

    #region LifeCycle
    
    private void Awake()
    {
        if (shouldInstantiate())
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeInput();

        stateMachine = new StateMachine();
    }
    
    private void Update()
    {
        if (!IsFrozen() && !isLocked)
        {
            SetDirectionsFromInput();
            CheckInteractbles();
        }

        if(transformDirection != Vector3.zero)
        {
            if (!sourceSteps.isPlaying)
            {
                sourceSteps.Play();
            }
        }
        else
        {
            sourceSteps.Stop();
        }

        stateMachine.Tick();
    }

    private void PlaySound(AudioClip audioClip)
    {
        sourceOtherSounds.clip = audioClip;
        sourceOtherSounds.Play();
    }

    private void CheckInteractbles()
    {
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position, GetDirectionRayFromFacingDirection(), 0.6f);

        if (hit)
        {
            if (hit.collider.CompareTag("Interactable"))
            {
                if (!interacted)
                {
                    interactionPopup.transform.position = Camera.main.WorldToScreenPoint(transform.position);
                    interactionPopup.SetActive(true);
                }

                if (Input.GetKeyDown(KeyCode.F))
                {
                    PlaySound(interactionSound);
                    interacted = true;
                    Interactable interacable = hit.collider.GetComponent<Interactable>();
                    interacable.Interact();
                    interactionPopup.SetActive(false);
                }
            }
            else
            {
                interacted = false;
                interactionPopup.SetActive(false);
            }
        }
        else
        {
            interacted = false;
            interactionPopup.SetActive(false);
        }
       
    }

    internal void Lock()
    {
        isLocked = true;
    }

    internal void Unlock()
    {
        isLocked = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, GetDirectionRayFromFacingDirection() * 0.6f);
    }

    private Vector2 GetDirectionRayFromFacingDirection()
    {
        Vector2 directionRay = Vector2.zero;
        switch (facingDirection)
        {
            case Direction.Down:
                directionRay = Vector2.down;
                break;
            case Direction.Right:
                directionRay = Vector2.right;
                break;
            case Direction.Up:
                directionRay = Vector2.up;
                break;
            case Direction.Left:
                directionRay = Vector2.left;
                break;
        }

        return directionRay;
    }

    #endregion

    #region InterfaceImplementation

    public bool IsWalking()
    {
        return !transformDirection.Equals(Vector3.zero);
    }

    public Direction GetFacingDirection()
    {
        return facingDirection;
    }

    public Vector3 GetTransformDirection()
    {
        return transformDirection;
    }

    public Animator GetAnimator()
    {
        return Animator;
    }

    public Transform GetTransform()
    {
        return transform;
    }

    public float GetSpeed()
    {
        return Speed;
    }

    #endregion

    #region Initialization

    private bool shouldInstantiate()
    {
        return instance == null;
    }

    private void InitializeInput()
    {
        keys = new Dictionary<KeyCode, Tuple<Vector3, Direction>>();
        keys.Add(KeyCode.W, Tuple.Create(Vector3.up, Direction.Up));
        keys.Add(KeyCode.S, Tuple.Create(Vector3.down, Direction.Down));
        keys.Add(KeyCode.A, Tuple.Create(Vector3.left, Direction.Left));
        keys.Add(KeyCode.D, Tuple.Create(Vector3.right, Direction.Right));
    }

    #endregion

    #region Freezing

    public void Freeze(float freezeTime)
    {
        transformDirection = Vector3.zero;
        if (releaseCoroutine != null)
        {
            freezeTime *= 0.85f;
            StopCoroutine(releaseCoroutine);
        }
        releaseCoroutine = StartCoroutine(Release(freezeTime));
        stateMachine.TransitionTo(new FrozenState(this));
    }
    
    private IEnumerator Release(float timeInSeconds)
    {
        yield return new WaitForSeconds(timeInSeconds);
        stateMachine.TransitionTo(new IdleState(this));
    }

    private bool IsFrozen()
    {
        return stateMachine.IsCurrentState(typeof(FrozenState));
    }

    #endregion

    #region Moving

    private void SetDirectionsFromInput()
    {
        transformDirection = Vector3.zero;

        foreach (var keyCode in keys)
        {
            if (Input.GetKey(keyCode.Key))
            {
                ConfigureDirections(keyCode.Value.Item1, keyCode.Value.Item2);
            }
        }

        if (transformDirection.Equals(Vector3.zero))
        {
            stateMachine.TransitionTo(new IdleState(this));
        }
        else
        {
            stateMachine.TransitionTo(new InputWalkingState(this));
        }
    }

    private void ConfigureDirections(Vector3 transformDirection, Direction facingDirection)
    {
        this.facingDirection = facingDirection;
        this.transformDirection += transformDirection;
    }

    #endregion
}