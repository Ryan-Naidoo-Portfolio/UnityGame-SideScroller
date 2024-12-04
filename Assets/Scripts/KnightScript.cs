using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections), typeof(Damageable))]
public class KnightScript : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    public DetectionZone attackZone;
    Animator animator;
    Damageable damageable;

    public enum walkDirection { right, left }

    private walkDirection _walkDirection;
    private Vector2 walkDirectionVector = Vector2.right;

    public walkDirection WalkDirection
    {
        get
        {
            return _walkDirection;
        }
        set
        {
            if (_walkDirection != value)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
                if (value == walkDirection.right)
                {
                    walkDirectionVector = Vector2.right;
                }
                else if (value == walkDirection.left)
                {
                    walkDirectionVector = Vector2.left;
                }
            }
            _walkDirection = value;
        }
    }

    public bool _hasTarget = false;

    public bool HasTarget
    {
        get
        {
            return _hasTarget;
        }
        private set
        {
            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
        set
        {
            animator.SetBool(AnimationStrings.canMove, value);
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

    public float AttackCooldown
    {
        get
        {
            return animator.GetFloat(AnimationStrings.attackCooldown);
        }
        private set
        {
            animator.SetFloat(AnimationStrings.attackCooldown, Mathf.Max(value, 0));
        }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
        damageable = GetComponent<Damageable>();

        if (rb == null) Debug.LogError("Rigidbody2D is missing!");
        if (touchingDirections == null) Debug.LogError("TouchingDirections is missing!");
        if (animator == null) Debug.LogError("Animator is missing!");
        if (damageable == null) Debug.LogError("Damageable is missing!");
        if (attackZone == null) Debug.LogError("AttackZone is not assigned!");
    }

    private void Update()
    {
        HasTarget = attackZone.detectedColliders.Count > 0;

        if (AttackCooldown > 0)
        {
            AttackCooldown -= Time.deltaTime;
        }
    }

    private float flipCooldown = 0.5f; // 0.5 seconds cooldown
    private float lastFlipTime = 0;

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && Time.time > lastFlipTime + flipCooldown)
        {
            FlipDirection();
            lastFlipTime = Time.time;
        }

        if (!damageable.LockVelocity)
        {
            if (CanMove)
                rb.velocity = new Vector2(walkSpeed * walkDirectionVector.x, rb.velocity.y);
            else
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, walkStopRate), rb.velocity.y);
        }
    }

    private void FlipDirection()
    {
        if (WalkDirection == walkDirection.right)
        {
            WalkDirection = walkDirection.left;
        }
        else if (WalkDirection == walkDirection.left)
        {
            WalkDirection = walkDirection.right;
        }
        else
        {
            Debug.LogError("Current walk direction not set to legal values of left or right");
        }
    }

    public void onHit(int damage, Vector2 knockback)
    {
        rb.velocity = new Vector2(knockback.x, rb.velocity.y + knockback.y);
    }

    public void OnAttackAnimationEnd()
    {
        CanMove = true;
    }
}
