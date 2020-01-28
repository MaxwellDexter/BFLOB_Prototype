using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        FrogLocked,
        Ball
    }

    public Transform target;
    public Transform lookAtPos;
    public Transform ballCentre;
    public float mouseSpeed, sensitivityX, sensitivityY, distance, lockOnRadius;

    private const float Y_ANGLE_MIN = -90f;
    private const float Y_ANGLE_MAX = 90f;

    private PlayerController controller;
    private TongueController tongueController;
    private float currentX, currentY;
    private Vector3 currentLookAtPos;
    private CameraMode cameraMode;

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

        switch (cameraMode)
        {
            case CameraMode.FrogLocked:
                currentY = mouseHorizontal * sensitivityY;
                break;
            case CameraMode.Ball:
                currentY += mouseHorizontal * sensitivityY;
                break;
        }

        // clamp the x from rotating past regular
        currentX = Mathf.Clamp(currentX, Y_ANGLE_MIN, Y_ANGLE_MAX);
    }

    private void FixedUpdate()
    {
        Vector3 currentVel = controller.rb.velocity;
        float yValue = 0;

        switch (cameraMode)
        {
            case CameraMode.FrogLocked:
                LockOn();
                yValue = target.rotation.eulerAngles.y;
                target.Rotate(new Vector3(0, currentY, 0));
                break;
            case CameraMode.Ball:
                currentLookAtPos = Vector3.SmoothDamp(currentLookAtPos, ballCentre.position, ref currentVel, 0.05f);
                yValue = currentY;
                break;
        }


        Vector3 offset = new Vector3(0, 0, -distance);
        Quaternion rotation = Quaternion.Euler(currentX, yValue, 0);
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

    public void SetCameraMode(CameraMode mode)
    {
        cameraMode = mode;
        if (cameraMode == CameraMode.Ball)
        {
            SwapToBallMode();
        }
    }

    public void SwapToBallMode()
    {
        currentY = target.rotation.eulerAngles.y;
    }
}
