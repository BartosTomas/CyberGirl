using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyberGirlScript : MonoBehaviour
{
    private float horizontalInput;
    private bool faceRight;
    private bool isJumping;
    private bool isDashing;
    private bool ableToSlam;
    private bool ableToDash = true;
    [SerializeField] private float maxVel;
    [SerializeField] private float accelStrength;
    [SerializeField] private float breakStrength;
    [SerializeField] private float jumpStrength;
    [SerializeField] private float slamStrength;
    [SerializeField] private float jumpTime;
    [SerializeField] private float dashStrength;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float hangTime;
    private float dashCooldownCounter;
    private float jumpTimeCounter;

    private Rigidbody2D rb;
    private BoxCollider2D boxColider;
    [SerializeField] private LayerMask ground;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxColider = GetComponent<BoxCollider2D>();
        dashCooldownCounter = dashCooldown;

    }
    void FixedUpdate()
    {
        if (!isDashing)
        {
            #region Acceleration
            if (Mathf.Abs(rb.velocity.x) < maxVel)
            {
                rb.AddForce(new Vector2(horizontalInput * accelStrength, 0));
                if (horizontalInput == -1)
                {
                    faceRight = false;
                }
                else if (horizontalInput == 1)
                {
                    faceRight = true;
                }
            }
            #endregion
            #region Deceleration
            if (horizontalInput == 0)
            {
                if (faceRight && rb.velocity.x > 0)
                {
                    rb.AddForce(new Vector2(-breakStrength, 0));
                }
                else if (faceRight == false && rb.velocity.x < 0)
                {
                    rb.AddForce(new Vector2(breakStrength, 0));
                }
                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
            }
            #endregion
        }

    }
    private void Update()
    {
        if (!isDashing)
        {
            #region Dash
            if (Input.GetKeyDown(KeyCode.LeftShift) && ableToDash == true)
            {
                //if grounded and ableToDash == false
                    // wait for dashCooldownCounter and than dash
                StartCoroutine(Dash());
            }
            if (IsGrounded())
            {
                //counter for dash cooldown
                if (dashCooldownCounter > 0)
                {
                    dashCooldownCounter -= Time.deltaTime;
                }
                else
                {
                    dashCooldownCounter = dashCooldown;
                    ableToDash = true;
                }
                
            }
            #endregion
            horizontalInput = Input.GetAxisRaw("Horizontal");
            #region Jump
            //Jump impuls
            if (Input.GetButtonDown("Jump") && IsGrounded() == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                isJumping = true;
                jumpTimeCounter = jumpTime;
            }

            #region Slam
            if (Input.GetKeyDown(KeyCode.S) && ableToSlam == true)
            {
                rb.velocity = new Vector2(rb.velocity.x, -slamStrength);
                ableToSlam = false;
            }
            #endregion

            //Jump float higher
            if (Input.GetKey(KeyCode.W) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    rb.velocity = new Vector2(rb.velocity.x, jumpStrength);
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    ableToSlam = true;
                }
            }

            //Stop Jump
            if (Input.GetKeyUp(KeyCode.W))
            {
                isJumping = false;
                ableToSlam = true;
            }
            #endregion
        }
    }
    private bool IsGrounded()
    {
        if (Physics2D.BoxCast(boxColider.bounds.center, boxColider.bounds.size, 0f, Vector2.down, .5f, ground))
        {
            ableToSlam = false;
            return true;
        }
        else
        {
            return false;
        }
    }
    private IEnumerator Dash()
    {
        ableToDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        if (faceRight)
        {
            rb.velocity = new Vector2(dashStrength, 0);
        }
        else
        {
            rb.velocity = new Vector2(-dashStrength, 0);
        }
        yield return new WaitForSeconds(dashTime);
        rb.velocity = Vector2.zero;
        isDashing = false;
        ableToSlam = true;
        yield return new WaitForSeconds(hangTime);
        rb.gravityScale = originalGravity;

    }
}
