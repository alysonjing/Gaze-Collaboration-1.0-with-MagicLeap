using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeapTools;

public class transmissionColorB : MonoBehaviour
{

    TransmissionObject transmissionObj;
    private MeshRenderer meshRenderer;
    private TrailRenderer trailRenderer;
    private LineRenderer lineRenderer;
    public Material startingC, changingC, mutual;
    private float t = 0;
    private Vector3 prevPos;

    public static string currentMaterial = "????";
    public static Vector3 currentPos;

    void Start()
    {
        transmissionObj = GetComponent<TransmissionObject>();
        meshRenderer = GetComponent<MeshRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        currentMaterial = "Instantiated";
    }

    void Update()
    {
        Vector3 newPos = transmissionObj.localPosition;
        currentPos = newPos;
        float offsetPos = (newPos - prevPos).magnitude;

        if (offsetPos < 0.05)
        {
            t += Time.deltaTime;
            if (t > 2)
            {
                currentMaterial = mutual.name;
                meshRenderer.material = mutual;
                trailRenderer.material = mutual;
                lineRenderer.material = mutual;
            }
        }
        else
        {
            prevPos = newPos;
            t = 0;
            currentMaterial = changingC.name;
            meshRenderer.material = changingC;
            trailRenderer.material = changingC;
            lineRenderer.material = changingC;            
        }
    }
}
