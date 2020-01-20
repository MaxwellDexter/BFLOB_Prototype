using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Transform lookAtPos;
    public float mouseSpeed, sensitivityX, sensitivityY, distance, lockOnRadius;

    private const float Y_ANGLE_MIN = -90f;
    private const float Y_ANGLE_MAX = 90f;

    private PlayerController controller;
    private TongueController tongueController;
    private float currentX, currentY;
    private Vector3 currentLookAtPos;

    private void Start()
    {
        controller = target.GetComponent<PlayerController>();
        tongueController = target.GetComponent<TongueController>();
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseHorizontal = Input.GetAxis("Mouse X");
        float mouseVertical = Input.GetAxis("Mouse Y");

        currentX += mouseVertical * sensitivityX;
        currentY = mouseHorizontal * sensitivityY;

        // clamp the x from rotating past regular
        currentX = Mathf.Clamp(currentX, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void FixedUpdate()
    {
        LockOn();

        float yValue = target.rotation.eulerAngles.y;
        target.Rotate(new Vector3(0, currentY, 0));

        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, yValue, 0);
        Vector3 currentVel = controller.rb.velocity;
        transform.position = Vector3.SmoothDamp(transform.position, target.transform.position + rotation * offset, ref currentVel, 0.05f);
        transform.LookAt(currentLookAtPos);
    }

    private void LockOn()
    {
        bool didHit = Physics.SphereCast(
                transform.position,
                lockOnRadius,
                lookAtPos.position - transform.position,
                out RaycastHit hit,
                tongueController.maxTongueDistance,
                PlayerScriptHelper.GetHittableLayers()
            );

        Vector3 currentVel = controller.rb.velocity;
        currentLookAtPos = Vector3.SmoothDamp(
            currentLookAtPos,
            // if did hit && point is in front of player && it's not what we're swinging on
            didHit && hit.distance > distance / 2 && hit.transform != tongueController.swingObject ? hit.transform.position : lookAtPos.position,
            ref currentVel,
            0.05f
        );
    }
}
