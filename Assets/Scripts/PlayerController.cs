using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float acceleration, maxMovementSpeed, jumpForce, rotateSpeed, dashPower;
    public Transform groundCheck;

    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool canInput;
    [HideInInspector]
    public Rigidbody rb;

    private AnimationController animator;
    private SoundController sounds;
    private float forwardVelToAdd, rightVelToAdd;

    public BarContainer bar;
    public Image dashBarImage;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<AnimationController>();
        sounds = GetComponent<SoundController>();
        canInput = true;
        //bar = new BarContainer(dashPower * 5, dashPower * 5, 10, 0);
    }

    private void Update()
    {
        CheckGround();

        if (canInput)
        {
            DoForwardMovement();

            DoSidewaysMovement();

            DoJump();

            DoDash();

            // clamp velocity
            if (rb.velocity.magnitude > maxMovementSpeed || rb.velocity.magnitude < -maxMovementSpeed)
            {
                forwardVelToAdd = 0;
                rightVelToAdd = 0;
            }
        }

        bar.UpdateBar();
        dashBarImage.fillAmount = bar.GetFillPercentage();
    }

    private void CheckGround()
    {
        bool wasGrounded = isGrounded;
        Physics.Raycast(groundCheck.position, new Vector3(0, -1), out RaycastHit raycastHit, 0.1f);
        isGrounded = raycastHit.transform != null && raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");
        animator.IsGrounded(isGrounded);
        if (!wasGrounded && isGrounded)
        {
            animator.Landed();
            sounds.PlaySound("Land");
        }
    }

    private void DoForwardMovement()
    {
        forwardVelToAdd = Input.GetAxis("Vertical");
        animator.SetVelocity(forwardVelToAdd);
    }

    private void DoSidewaysMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        rightVelToAdd = horizontal;
        animator.SetTurning(horizontal);
    }

    private void DoJump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(0, jumpForce, 0);
            animator.Jump();
            sounds.PlaySound("Jump");
        }
    }

    private void DoDash()
    {
        if (Input.GetButtonDown("Dash") && bar.AddToCurrentIfLegal(-dashPower))
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, dashPower));
        }
    }

    private void FixedUpdate()
    {
        if (canInput && (forwardVelToAdd > 0 || forwardVelToAdd < 0 || rightVelToAdd > 0 || rightVelToAdd < 0))
        {
            rb.AddForce(transform.rotation * new Vector3(
                rightVelToAdd * acceleration,
                0,
                forwardVelToAdd * acceleration));
        }
    }
}
