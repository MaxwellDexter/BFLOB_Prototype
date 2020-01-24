using UnityEngine;
using System.Collections.Generic;

public class TongueController : MonoBehaviour
{
    public float maxTongueDistance;
    public float numOfNodes;
    public float slingshotPower;
    public GameObject nodePrefab;
    public Transform mouthPos;

    [HideInInspector]
    public Transform swingObject;

    private Camera theCamera;
    private List<GameObject> currentNodes;
    private LineRenderer lineRenderer;
    private SpringJoint spring;
    private Rigidbody rb;
    private InventoryController inventory;
    private PlayerController playerController;
    private SoundController sounds;

    private void Start()
    {
        currentNodes = new List<GameObject>();
        theCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        playerController = GetComponent<PlayerController>();
        sounds = GetComponent<SoundController>();
        spring = GetComponent<SpringJoint>();
        rb = GetComponent<Rigidbody>();
        inventory = GetComponent<InventoryController>();

        lineRenderer = GetComponent<LineRenderer>();
        float ropeWidth = 0.1f;
        lineRenderer.startWidth = ropeWidth;
        lineRenderer.endWidth = ropeWidth;
    }
    
    private void Update()
    {
        // shoot tongue
        if (Input.GetButtonDown("Fire1") && playerController.canInput)
        {
            ThrowTongue();
        }
        else if (Input.GetButtonDown("Fire2"))
        {
            ClearSpring();
            sounds.PlaySound("Retract");
        }
        else if (Input.GetButtonDown("Fire3") && playerController.canInput)
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
                PlayerScriptHelper.GetHittableLayers()
            );

        if (didHit)
        {
            int layer = hit.transform.gameObject.layer;
            if (layer == LayerMask.NameToLayer(PlayerScriptHelper.COLLECTIBLE_LAYER))
            {
                // collect it
                HitThingWithTongue(hit.transform.gameObject);
                sounds.PlaySound("Collect");
            }
            else if (layer == LayerMask.NameToLayer("Hook"))
            {
                // we swing
                swingObject = hit.transform;
                DoSpring(mouthPos.position, hit.point);
                sounds.PlaySound("Hit");
            }
            else
            {
                sounds.PlaySound("Miss");
            }
        }
        else
        {
            sounds.PlaySound("Miss");
            // throw tongue out and play with it
            //Vector3 endPosition = theCamera.transform.position + theCamera.transform.TransformDirection(Vector3.forward) * maxTongueDistance;
            //CreateNodes(mouthPos.position, endPosition, didHit);
        }
    }

    private void DoSpring(Vector3 startPos, Vector3 endPos)
    {
        float distance = Vector3.Distance(startPos, endPos);

        spring.connectedAnchor = endPos;
        spring.spring = 50;
        spring.minDistance = distance;
        spring.minDistance = distance;
        lineRenderer.enabled = true;
    }

    private void ClearSpring()
    {
        swingObject = null;
        spring.spring = 0;
        spring.connectedAnchor = Vector3.zero;
        lineRenderer.enabled = false;
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
            sounds.PlaySound("Slingshot");
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
                currentNodes.Add(CreateFirstNode(nodePos, nodeLength));
                continue;
            }

            bool isKinematic = i == numOfNodes - 1 && didHit;

            currentNodes.Add(CreateNode(nodePos, currentNodes[i - 1], isKinematic, nodeLength));
        }
    }

    private GameObject CreateFirstNode(Vector3 pos, float distance)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        //node.GetComponent<Rigidbody>().isKinematic = true;
        SpringJoint joint = node.GetComponent<SpringJoint>();
        joint.minDistance = distance;
        joint.maxDistance = distance;
        joint.connectedBody = rb;
        return node;
    }

    private GameObject CreateNode(Vector3 pos, GameObject prevJoint, bool isKinematic, float distance)
    {
        GameObject node = Instantiate(nodePrefab, pos, Quaternion.identity);
        node.GetComponent<Rigidbody>().isKinematic = isKinematic;
        SpringJoint joint = node.GetComponent<SpringJoint>();
        joint.minDistance = distance;
        joint.maxDistance = distance;
        if (prevJoint != null)
        {
            joint.connectedBody = prevJoint.GetComponent<Rigidbody>();
        }

        return node;
    }

    private void ClearTongue()
    {
        foreach (GameObject node in currentNodes)
        {
            Destroy(node);
        }
        currentNodes.Clear();
    }

    private void HitThingWithTongue(GameObject thing)
    {
        inventory.AddCollectible();
        Destroy(thing);
    }
}
