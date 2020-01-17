using UnityEngine;
using System.Collections.Generic;

public class TongueController : MonoBehaviour
{
    public float maxTongueDistance;
    public float numOfNodes;
    public float slingshotPower;
    public GameObject nodePrefab;
    public GameObject springPrefab;
    public Transform mouthPos;

    [HideInInspector]
    public Transform swingObject;

    private Camera theCamera;
    private List<GameObject> currentNodes;
    private LineRenderer lineRenderer;
    private SpringJoint spring;
    private Rigidbody rb;

    private void Start()
    {
        currentNodes = new List<GameObject>();
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        spring = GetComponent<SpringJoint>();
        rb = GetComponent<Rigidbody>();

        lineRenderer = GetComponent<LineRenderer>();
        float ropeWidth = 0.1f;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
    }
    
    private void Update()
    {
        // shoot tongue
        if (Input.GetButtonDown("Fire1"))
        {
            ThrowTongue();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ClearSpring();
        }
        else if (Input.GetButtonDown("Fire3"))
        {
            SlingshotToAnchor();
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
        else if (spring.connectedAnchor != Vector3.zero)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.SetPositions(new Vector3[] {mouthPos.position, spring.connectedAnchor});
        }
    }

    private void ThrowTongue()
    {
        ClearSpring();

        bool didHit = Physics.SphereCast(
                theCamera.transform.position,
                1,
                theCamera.transform.TransformDirection(Vector3.forward),
                out RaycastHit hit,
                maxTongueDistance,
                LayerMask.NameToLayer("Player")
            );

        Vector3 position;
        if (didHit)
        {
            position = hit.point;
            swingObject = hit.transform;
        }
        else
        {
            position = theCamera.transform.position + theCamera.transform.TransformDirection(Vector3.forward) * maxTongueDistance;
        }

        DoSpring(mouthPos.position, position, didHit);
        // CreateNodes(mouthPos.position, position, didHit);
    }

    private void DoSpring(Vector3 startPos, Vector3 endPos, bool didHit)
    {
        float distance = Vector3.Distance(startPos, endPos);

        if (didHit)
        {
            spring.connectedAnchor = endPos;
            spring.spring = 50;
            spring.minDistance = distance;
            spring.minDistance = distance;
        }
    }

    private void ClearSpring()
    {
        swingObject = null;
        spring.spring = 0;
        spring.connectedAnchor = Vector3.zero;
    }

    private void SlingshotToAnchor()
    {
        // add force from position to anchor
        if (spring.connectedAnchor != Vector3.zero)
        {
            Vector3 heading = spring.connectedAnchor - mouthPos.position;
            float distance = Vector3.Distance(mouthPos.position, spring.connectedAnchor);
            ClearSpring();
            rb.AddForce(heading.normalized * distance * slingshotPower);
        }
    }

    // rope stuff

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
        node.GetComponent<HingeJoint>().connectedBody = rb;
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
