using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserUpdate : MonoBehaviour
{

    LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPosition(0, gameObject.transform.position);
        lineRenderer.SetPosition(1, gameObject.transform.localScale);
    }
}
