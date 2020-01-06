using UnityEngine;

[System.Serializable]
public class CameraCollisionHandler : MonoBehaviour
{
    public LayerMask collisionLayer;

    [HideInInspector]
    public bool isColliding;

    [HideInInspector]
    public Vector3[] adjustedCameraClipPoints;

    [HideInInspector]
    public Vector3[] desiredCameraClipPoints;

    [HideInInspector]
    public Camera myCamera;

    public void Initialize(Camera cam)
    {
        myCamera = cam;
        adjustedCameraClipPoints = new Vector3[5];
        desiredCameraClipPoints = new Vector3[5];
    }

    public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
    {
        if (!myCamera)
            return;

        // clear contents
        intoArray = new Vector3[5];

        float z = myCamera.nearClipPlane;
        float x = Mathf.Tan(myCamera.fieldOfView / 2) * z;
        float y = x / myCamera.aspect;

        // top left
        intoArray[0] = (atRotation * new Vector3(-x, y, z)) + cameraPosition;
        // top right
        intoArray[1] = (atRotation * new Vector3(x, y, z)) + cameraPosition;
        // bottom left
        intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
        // bottom right
        intoArray[3] = (atRotation * new Vector3(x, -y, z)) + cameraPosition;
        // cam position
        intoArray[4] = cameraPosition - myCamera.transform.forward;
    }

    public bool CollisionDetectedAtPoints(Vector3[] points, Vector3 fromPosition)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Ray ray = new Ray(fromPosition, points[i]);
            float distance = Vector3.Distance(points[i], fromPosition);
            if (Physics.Raycast(ray, distance, collisionLayer))
            {
                return true;
            }
        }
        return false;
    }

    public float GetAdjustedDistanceWithRayFrom(Vector3 from)
    {
        float distance = -1;

        for (int i = 0; i < desiredCameraClipPoints.Length; i++)
        {
            Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit) && (distance == -1 || hit.distance < distance))
            {
                distance = hit.distance;
            }
        }

        if (distance == -1)
            return 0;
        else
            return distance;
    }

    public void CheckColliding(Vector3 targetPosition)
    {
        isColliding = CollisionDetectedAtPoints(desiredCameraClipPoints, targetPosition);
    }
}
