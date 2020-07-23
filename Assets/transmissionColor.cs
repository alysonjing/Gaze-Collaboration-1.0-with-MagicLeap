using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeapTools;

public class transmissionColor : MonoBehaviour
{

    TransmissionObject transmissionObj;
    private MeshRenderer meshRenderer;
    public MeshRenderer childRenderer;
    private TrailRenderer trailRenderer;
    private LineRenderer lineRenderer;
    private float t = 0;
    private Vector3 prevPos;


    public static string currentMaterial = "????";
    public static Vector3 currentPos;
    public bool Laser = false;
    public Material startingC, changingC, focus, fix;

    void Start()
    {
        transmissionObj = GetComponent<TransmissionObject>();
        meshRenderer = GetComponent<MeshRenderer>();
        childRenderer.enabled = false;
        trailRenderer = GetComponent<TrailRenderer>();
        lineRenderer = GetComponent<LineRenderer>();
        currentMaterial = "Instantiated";
    }

    void Update()
    {
        //Vector3 newPos = transmissionObj.localPosition;
        Vector3 newPos = Laser ? (transform.position + transform.forward * transform.localScale.x) : transform.localPosition;
        //Vector3 newPos = transform.position;
        currentPos = newPos;
        float offsetPos = (newPos - prevPos).magnitude;
        currentMaterial = "" + currentPos;

        if (offsetPos < 0.05)
        {
            t += Time.deltaTime;
            if ( t > 0.5 && t < 1.5)
            {
                //currentMaterial = "" + mutual.name;
                if (lineRenderer)
                {
                    lineRenderer.material = focus;
                }
                if (meshRenderer)
                {
                    meshRenderer.material = focus;
                }
                if (childRenderer)
                {
                    childRenderer.material = focus;
                    childRenderer.enabled = true;
                }
                    if (trailRenderer)
                {
                    trailRenderer.material = focus;
                }
            }

            if (t >= 1.5)
            {
                //currentMaterial = "" + mutual.name;
                if (lineRenderer)
                {
                    lineRenderer.material = fix;
                }
                if (meshRenderer)
                {
                    meshRenderer.material = fix;
                }
                if (childRenderer)
                {
                    childRenderer.material = fix;
                    childRenderer.enabled = true;
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
            //currentMaterial = startingC.name;
            t = 0;
            if (lineRenderer)
            {
                lineRenderer.material = startingC;
            }
            if (meshRenderer)
            {
                meshRenderer.material = startingC;
            }
            if (childRenderer)
            {
                childRenderer.material = startingC;
                childRenderer.enabled = false;
            }
            if (trailRenderer)
            {
                trailRenderer.material = startingC;
            }
        }
    }
}
