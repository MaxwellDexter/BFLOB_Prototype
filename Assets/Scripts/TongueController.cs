using UnityEngine;
using System.Collections.Generic;

public class TongueController : MonoBehaviour
{
    public float maxTongueDistance;
    public float numOfNodes;
    public GameObject nodePrefab;
    public Transform mouthPos;

    private Camera theCamera;
    private List<GameObject> currentNodes;
    private LineRenderer lineRenderer;

    private void Start()
    {
        currentNodes = new List<GameObject>();
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        lineRenderer = GetComponent<LineRenderer>();
        float ropeWidth = 0.1f;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
    }

    // maybe attach the first joint as a child of the player object

    private void Update()
    {
        // shoot tongue
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowTongue();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ClearTongue();
        }

        DrawTongue();
    }

    private void DrawTongue()
    {
        if(currentNodes.Count > 0)
        {
            lineRenderer.positionCount = currentNodes.Count;
            for (int i = 0; i < currentNodes.Count; i++)
            {
                lineRenderer.SetPosition(i, currentNodes[i].transform.position);
            }
        }
    }

    private void ThrowTongue()
    {
        bool didHit = Physics.Raycast(
                theCamera.transform.position,
                theCamera.transform.TransformDirection(Vector3.forward),
                out RaycastHit hit,
                maxTongueDistance,
                LayerMask.NameToLayer("Player")
            );

        Vector3 position;
        if (didHit)
        {
            position = hit.point;
            Debug.Log("Did hit!");
        }
        else
        {
            position = theCamera.transform.position + theCamera.transform.TransformDirection(Vector3.forward) * maxTongueDistance;
            Debug.Log("Did NOT hit!");
        }
        CreateNodes(mouthPos.position, position, didHit);
    }

    private void CreateNodes(Vector3 startPos, Vector3 endPos, bool didHit)
    {
        ClearTongue();

        float nodeLength = Vector3.Distance(startPos, endPos) / numOfNodes;

        Vector3 nodePos = startPos;

        for (int i = 0; i < numOfNodes; i++)
        {
            nodePos = Vector3.MoveTowards(nodePos, endPos, nodeLength);

            // first one has to be attached to the frog
            if (i == 0)
            {
                currentNodes.Add(CreateFirstNode(nodePos));
                continue;
            }

            bool isKinematic = i == numOfNodes - 1 && didHit;

            currentNodes.Add(CreateNode(nodePos, currentNodes[i - 1], isKinematic));
        }
    }

    private GameObject CreateFirstNode(Vector3 pos)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        //node.GetComponent<Rigidbody>().isKinematic = true;
        node.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
        return node;
    }

    private GameObject CreateNode(Vector3 pos, GameObject prevJoint, bool isKinematic)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.GetComponent<Rigidbody>().isKinematic = isKinematic;
        if (prevJoint != null)
        {
            node.GetComponent<HingeJoint>().connectedBody = prevJoint.GetComponent<Rigidbody>();
        }

        return node;
    }

    private void ClearTongue()
    {
        foreach (GameObject node in currentNodes)
        {
            GameObject.Destroy(node);
        }
        currentNodes.Clear();
    }
}
