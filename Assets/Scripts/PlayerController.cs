using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]

public class PlayerController : MonoBehaviour
{
    Vector2 moveInput;
    public float walkSpeed = 5f;
    public float runSpeed = 8f;
    public float airWalkSpeed = 3f;
    public float jumpImpulse = 4f;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = -40f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    public float currentMoveSpeed
    {
        get
        {
            if (CanMove)
            {
                if (isMoving && !TouchingDirections.IsOnWall)
                {
                    if (TouchingDirections.IsGrounded)
                    {
                        if (isRunning)
                        {
                            return runSpeed;
                        }
                        else
                        {
                            return walkSpeed;
                        }
                    }
                    else
                    {
                        return airWalkSpeed;
                    }
                }

                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }

        }
    }

    [SerializeField]
    private bool _isMoving = false;
    public bool isMoving 
    { 
        get 
        {
            return _isMoving;
        } 
        private set
        {
            _isMoving = value;
            animator.SetBool(AnimationStrings.isMoving, value);
        }
    }
    [SerializeField]
    private bool _isRunning = false;
    public bool isRunning
    {
        get
        {
            return _isRunning;
        }
        private set
        {
            _isRunning = value;
            animator.SetBool(AnimationStrings.isRunning, value);
        }
    }

    public bool _isFacingRight = true;
    public bool isFacingRight
    {
        get
        {
            return _isFacingRight;
        }
        private set
        {
            if(_isFacingRight != value)
            {
                transform.localScale *= new Vector2(-1, 1);
            }
            _isFacingRight = value;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    Rigidbody2D rb;
    Animator animator;
    TouchingDirections TouchingDirections;
    Damageable damageable;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        TouchingDirections = GetComponent<TouchingDirections>();
        damageable = GetComponent<Damageable>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.Q) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (!damageable.LockVelocity)
            rb.velocity = new Vector2(moveInput.x * currentMoveSpeed, rb.velocity.y);

        animator.SetFloat(AnimationStrings.yVelocity, rb.velocity.y);
    }
    public void onMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();

        if (IsAlive)
        {
            isMoving = moveInput != Vector2.zero;
            setFacingDirection(moveInput);
        }
        else
        {
            isMoving = false;
        }
    }

    private void setFacingDirection(Vector2 moveInput)
    {
        if(moveInput.x > 0 && !isFacingRight)
        {
            isFacingRight = true;
        }
        else if(moveInput.x < 0 && isFacingRight)
        {
            isFacingRight = false;
        }
    }
    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
    }

    public bool IsAlive
    {
        get
        {
            return animator.GetBool(AnimationStrings.isAlive);
        }
    }

    public bool lockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
    }

    public bool LockVelocity
    {
        get
        {
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    public void onRun(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isRunning = true;
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void onJump(InputAction.CallbackContext context)
    {
        if (context.started && TouchingDirections.IsGrounded && CanMove)
        {
            animator.SetTrigger(AnimationStrings.jumpTrigger);
            rb.velocity = new Vector2(rb.velocity.x, jumpImpulse);
        }
        else if (context.canceled)
        {
            isRunning = false;
        }
    }

    public void onAttack(InputAction.CallbackContext context)
    {
        if (context.started) 
        {            
                animator.SetTrigger(AnimationStrings.attackTrigger);     
        }
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }
}
