using UnityEngine;

public class CameraController : MonoBehaviour
{
    // can have two modes of camera movement
    // on the ground it acts like a third person shooter and the camera movement is limited to in front of you
    // in the air you can rotate it anywhere and shoot the tongue
    // it will flip the frog around and do a sick tongue shot

    public Transform target;
    public PlayerController controller;
    public float mouseSpeed;
    public Transform lookAtPos;

    public float maxGroundedYAngle;

    private const float Y_ANGLE_MIN = -90f;
    private const float Y_ANGLE_MAX = 90f;

    public float currentX, currentY, sensitivityX, sensitivityY, distance;

    private void Start()
    {
        controller = target.GetComponent<PlayerController>();
    }

    private void Update()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");

        currentX += mouseVertical * sensitivityX;
        currentY += mouseHorizontal * sensitivityY;

        if (controller.isGrounded)
        {
            // smooth clamp the horizontal
            float currentVel = controller.rb.velocity.magnitude;
            currentY = Mathf.SmoothDamp(currentY, target.transform.eulerAngles.y, ref currentVel, 0.1f);
        }

        currentX = Mathf.Clamp(currentX, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void FixedUpdate()
    {
        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, currentY, 0);
        Vector3 currentVel = controller.rb.velocity;
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + rotation * offset, ref currentVel, 0.05f);
        transform.LookAt(lookAtPos);
    }
}
