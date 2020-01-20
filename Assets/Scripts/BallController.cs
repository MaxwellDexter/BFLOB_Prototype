using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject frogObject;
    public GameObject ballObject;

    [HideInInspector]
    public bool isBall;

    private CameraController cameraController;
    private PlayerController playerController;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playerController = GetComponent<PlayerController>();

        isBall = false;
        frogObject.SetActive(true);
        ballObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Ball"))
        {
            EngageTransform();
        }
    }

    private void EngageTransform()
    {
        isBall = !isBall;
        frogObject.SetActive(!isBall);
        ballObject.SetActive(isBall);
        if (isBall)
        {
            TransformToBall();
        }
        else
        {
            TransformToFrog();
        }
    }

    private void TransformToBall()
    {
        rb.freezeRotation = false;
        cameraController.cameraMode = CameraController.CameraMode.Ball;
        playerController.canInput = false;
    }

    private void TransformToFrog()
    {
        rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;
        cameraController.cameraMode = CameraController.CameraMode.FrogLocked;
        playerController.canInput = true;
    }
}
