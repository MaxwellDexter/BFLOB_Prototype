using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bobber : MonoBehaviour
{
    public float bobAmount;
    public float bobSpeed;

    private Vector3 origPosition;
    private Vector3 topPosition;
    private Vector3 bottomPosition;

    private bool travellingUp;
    private float journeyLength;
    private float startTime;

    private void Start()
    {
        origPosition = transform.position;

        topPosition = origPosition;
        topPosition.y += bobAmount;

        bottomPosition = origPosition;
        bottomPosition.y -= bobAmount;

        journeyLength = Vector3.Distance(topPosition, bottomPosition);
        startTime = Time.time;
    }

    void Update()
    {
        float distCovered = (Time.time - startTime) * bobSpeed;
        float journeyFraction = distCovered / journeyLength;

        Vector3 target = travellingUp ? topPosition : bottomPosition;
        transform.position = Vector3.Lerp(transform.position, target, journeyFraction);

        if (transform.position.y == topPosition.y || transform.position.y == bottomPosition.y)
        {
            travellingUp = !travellingUp;
            startTime = Time.time;
        }
    }
}
