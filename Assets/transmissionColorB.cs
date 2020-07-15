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
    public Material startingC, changingC, focus, fix;
    private float t = 0;
    private Vector3 prevPos;


    public static string currentMaterial = "????";
    public static Vector3 currentPos;
    public bool Laser = false;

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
        //Vector3 newPos = transform.localPosition;
        Vector3 newPos = Laser ? (transform.position + transform.forward * transform.localScale.x) : transform.localPosition;
        //Vector3 newPos = transform.position;
        currentPos = newPos;
        float offsetPos = (newPos - prevPos).magnitude;
        currentMaterial = "" + currentPos;

        if (offsetPos < 0.05)
        {
            t += Time.deltaTime;
            if (t > 2 && t < 5)
            {
                //currentMaterial = mutual.name;
                if (lineRenderer)
                {
                    lineRenderer.material = focus;
                }
                if (meshRenderer)
                {
                    meshRenderer.material = focus;
                }
                if (trailRenderer)
                {
                    trailRenderer.material = focus;
                }
            }
            if (t >= 5)
            {
                //currentMaterial = mutual.name;
                if (lineRenderer)
                {
                    lineRenderer.material = fix;
                }
                if (meshRenderer)
                {
                    meshRenderer.material = fix;
                }
                if (trailRenderer)
                {
                    trailRenderer.material = fix;
                }
            }
        }
        else
        {
            prevPos = newPos;
            //currentMaterial = changingC.name;
            t = 0;
            if (lineRenderer)
            {
                lineRenderer.material = changingC;
            }
            if (meshRenderer)
            {
                meshRenderer.material = changingC;
            }
            if (trailRenderer)
            {
                trailRenderer.material = changingC;
            }          
        }
    }
}
