using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagicLeapTools;

public class transmissionColor : MonoBehaviour
{

    TransmissionObject transmissionObj;
    private MeshRenderer meshRenderer;
    public Material startingC, changingC, mutual;
    private float t = 0;
    private Vector3 prevPos;

    public static string currentMaterial = "????";

    void Start()
    {
        transmissionObj = GetComponent<TransmissionObject>();
        meshRenderer = GetComponent<MeshRenderer>();
        currentMaterial = "Instantiated";
    }

    void Update()
    {
        Vector3 newPos = transmissionObj.localPosition;
        float offsetPos = (newPos - prevPos).magnitude;

        if (offsetPos < 0.05)
        {
            t += Time.deltaTime;
            if (t > 3)
            {
                currentMaterial = mutual.name;
                meshRenderer.material = mutual;
            }
        }
        else
        {
            prevPos = newPos;
            currentMaterial = startingC.name;
            meshRenderer.material = startingC;
            t = 0;
        }
    }
}
