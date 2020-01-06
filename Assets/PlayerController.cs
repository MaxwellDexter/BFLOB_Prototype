using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxMovementSpeed;
    public float jumpForce;
    public float rotateSpeed;

    private Transform groundCheck;

    [HideInInspector]
    public bool isGrounded;

    private float forwardVelToAdd;

    [HideInInspector]
    public Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundCheck = transform.Find("GroundCheck");
    }

    private void Update()
    {
        // is grounded
        Physics.Raycast(groundCheck.position, new Vector3(0, -1), out RaycastHit raycastHit, 0.1f);
        isGrounded = raycastHit.transform != null && raycastHit.transform.gameObject.layer == LayerMask.NameToLayer("Ground");

        // forward / backward
        forwardVelToAdd = Input.GetAxis("Vertical");
        if (rb.velocity.magnitude > maxMovementSpeed || rb.velocity.magnitude < -maxMovementSpeed)
        {
            forwardVelToAdd = 0;
        }

        // turn
        float rotate = Input.GetAxis("Horizontal");
        transform.RotateAround(transform.position, transform.up, rotate * (isGrounded ? rotateSpeed : rotateSpeed / 2));

        // jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(0, jumpForce, 0);
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
