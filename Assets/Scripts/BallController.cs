using UnityEngine;

public class BallController : MonoBehaviour
{
    public GameObject frogObject;
    public GameObject ballObject;

    [HideInInspector]
    public bool isBall;

    private CameraController cameraController;
    private PlayerController playerController;
    private SoundController sounds;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        cameraController = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
        playerController = GetComponent<PlayerController>();
        sounds = GetComponent<SoundController>();

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
        cameraController.SetCameraMode(CameraController.CameraMode.Ball);
        playerController.canInput = false;
        sounds.PlaySound("BallTransform");
    }

    private void TransformToFrog()
    {
        rb.freezeRotation = true;
        transform.rotation = Quaternion.identity;
        cameraController.SetCameraMode(CameraController.CameraMode.FrogLocked);
        playerController.canInput = true;
        sounds.PlaySound("FrogTransform");
    }

    void OnCollisionEnter(Collision col)
    {
        if (isBall)
        {
            sounds.PlaySound("Bounce");
        }
    }
}
