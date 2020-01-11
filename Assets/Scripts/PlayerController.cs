using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxMovementSpeed;
    public float jumpForce;
    public float rotateSpeed;

    private Transform groundCheck;
    private AnimationController animator;

    [HideInInspector]
    public bool isGrounded;

    private float forwardVelToAdd;

    [HideInInspector]
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponentInChildren<AnimationController>();
    }

    private void Update()
    {
        // is grounded
        bool wasGrounded = isGrounded;
        Physics.Raycast(groundCheck.position, new Vector3(0, -1), out RaycastHit raycastHit, 0.1f);
        isGrounded = raycastHit.transform != null && raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");
        animator.IsGrounded(isGrounded);
        if (!wasGrounded && isGrounded)
        {
            animator.Landed();
        }

        // forward / backward
        forwardVelToAdd = Input.GetAxis("Vertical");
        animator.SetVelocity(forwardVelToAdd);
        if (rb.velocity.magnitude > maxMovementSpeed || rb.velocity.magnitude < -maxMovementSpeed)
        {
            forwardVelToAdd = 0;
        }

        // turn
        float rotate = Input.GetAxis("Horizontal");
        transform.RotateAround(transform.position, transform.up, rotate * (isGrounded ? rotateSpeed : rotateSpeed / 2));
        animator.SetTurning(rotate);

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(0, jumpForce, 0);
            animator.Jump();
        }
    }

    private void FixedUpdate()
    {
        if (forwardVelToAdd > 0 || forwardVelToAdd < 0)
        {
            rb.AddForce(transform.rotation * new Vector3(0, 0, forwardVelToAdd * acceleration));
        }
    }
}
