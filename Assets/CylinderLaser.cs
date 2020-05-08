using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

public class CylinderLaser : MonoBehaviour
{

    private Vector3 fixationPoint;
    [SerializeField]
    private float dist = 50;
    public Transform MLCam;
    public GameObject camTest;

    public EyeTracking eyeTracking;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = MLCam.position;
        fixationPoint = eyeTracking.currPos;
        dist = Vector3.Distance(MLCam.position, fixationPoint);
        transform.LookAt(fixationPoint);
        
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, dist);
    }

    private void OnValidate()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, dist);
    }
}
