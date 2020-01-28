using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration, maxMovementSpeed, jumpForce, rotateSpeed;
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

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<AnimationController>();
        sounds = GetComponent<SoundController>();
        canInput = true;
    }

    private void Update()
    {
        CheckGround();

        if (canInput)
        {
            DoForwardMovement();

            DoSidewaysMovement();

            DoJump();

            // clamp velocity
            if (rb.velocity.magnitude > maxMovementSpeed || rb.velocity.magnitude < -maxMovementSpeed)
            {
                forwardVelToAdd = 0;
                rightVelToAdd = 0;
            }
        }

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
