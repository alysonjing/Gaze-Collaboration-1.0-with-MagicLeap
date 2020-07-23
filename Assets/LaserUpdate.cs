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
        //Vector3 v = gameObject.transform.rotation * Vector3.forward * gameObject.transform.localScale.x;  //gun suggested
        Vector3 v = gameObject.transform.forward * gameObject.transform.localScale.x; //nick suggested
        lineRenderer.SetPosition(1, gameObject.transform.position + v);
    }
}
