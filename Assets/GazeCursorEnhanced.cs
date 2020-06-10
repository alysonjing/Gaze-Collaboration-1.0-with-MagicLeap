﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class GazeCursorEnhanced : MonoBehaviour
{

    public GameObject Camera;
    public MeshRenderer meshRenderer;
    public Material startingC, changingC, mutual;

    Vector3 filterd = new Vector3();
    int bufferIndex = 0;
    Vector3[] buffer = new Vector3[5];
    Vector3 sum2 = Vector3.zero;

    [SerializeField]
    private Vector3 offsetRot;
    //private Vector3 offsetPos;
    public Vector3 currPos;
    private Camera mainCam;
    private TextMesh MLdebugger;

    private Vector3 _heading;
    private Vector3 hitpoint;


    private float t = 0;
    //public bool testLerp = false;
    public float smoothing = 10;

    TransmissionObject gazeTransmissionObject;

    /***
     * spatial alignment
     * 
     ***/
    public ControlInput controlLocator;
    public Text info;

    private List<TransmissionObject> _spawned = new List<TransmissionObject>();
    private string _initialInfo;


    //spacial alignment Init:
    private void Awake()
    {
        //hooks:
        controlLocator.OnTriggerDown.AddListener(HandleTriggerDown);
        controlLocator.OnBumperDown.AddListener(HandleBumperDown);

        //shared head locator:
        TransmissionObject headTransmissionObject = Transmission.Spawn("CursorB", Vector3.zero, Quaternion.identity, Vector3.one);
        headTransmissionObject.motionSource = Camera.transform;

        //shared controll locator:
        TransmissionObject controlTransmissionObject = Transmission.Spawn("SampleTransmissionObject", Vector3.zero, Quaternion.identity, Vector3.one);
        controlTransmissionObject.motionSource = controlLocator.transform;

        //share gaze locator: Not sure how to change?
        gazeTransmissionObject = Transmission.Spawn("CursorB", Vector3.zero, Quaternion.identity, Vector3.one);
        gazeTransmissionObject.motionSource = gameObject.transform;

        //sets:
        _initialInfo = info.text;
    }

    //Event Handlers:
    private void HandleTriggerDown()
    {
        //stamp a cube in space:
        TransmissionObject spawn = Transmission.Spawn("SampleTransmissionObject", controlLocator.Position, controlLocator.Orientation, Vector3.one);
        _spawned.Add(spawn);

    }

    private void HandleBumperDown()
    {
        //remove all stamped cubes:
        foreach (var item in _spawned)
        {
            item.Despawn();
        }

        _spawned.Clear();
    }




    #region Unity Methods
    void Start()
    {
        MLEyes.Start();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //transform.position = Camera.transform.position + Camera.transform.forward * 2.0f;

        //cursorInstance = Instantiate(cursorPrefab);


    }
    private void OnDisable()
    {
        MLEyes.Stop();
    }
    void Update()
    {
        //Spatial alignment
        string output = _initialInfo + System.Environment.NewLine;
        output += "Peers Available: " + Transmission.Instance.Peers.Length + System.Environment.NewLine;
        output += "Localized: " + SpatialAlignment.Localized;

        info.text = output;
        //end 

        //original gaze script
        if (MLEyes.IsStarted)
        {

            //v1
            //filterd = Vector3.Lerp(filterd, MLEyes.FixationPoint, 0.7f);
            //gameObject.transform.position = filterd;
            // oldPos = gameObject.transform.position;


            //v2
            buffer[bufferIndex] = MLEyes.FixationPoint;
            bufferIndex = (bufferIndex + 1) % 5;

            Vector3 sum = Vector3.zero;
            for (int i = 0; i < 5; i++)
            {
                sum += buffer[i];
            }
            currPos = sum / 5;

            /**
             * position for the cursor
             **/
            RaycastHit rayHit;
            _heading = MLEyes.FixationPoint - Camera.transform.position;
            if (Physics.Raycast(Camera.transform.position, _heading, out rayHit, 10.0f))
            {
                hitpoint = new Vector3(rayHit.point.x, rayHit.point.y, rayHit.point.z - 0.025f);
                //gameObject.transform.position = hitpoint;
                gameObject.transform.position = Vector3.MoveTowards(transform.position, hitpoint, smoothing * Time.deltaTime);
                gameObject.transform.rotation = Camera.transform.rotation;
            }
            else {
                gameObject.transform.position = Vector3.MoveTowards(transform.position, currPos, smoothing * Time.deltaTime);
                //gaze pointer rotation offset
                gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
                //MLdebugger.text = MLEyes.FixationPoint + "\n" + currPos + "\n" + offsetRot + "\n" + t + "\n";
            }



            /**
             * Offset position radius
             **/
            Vector3 offset = MLEyes.FixationPoint - currPos;
            float offsetPos = offset.magnitude;

            if (offsetPos < 0.05)
            {
                t += Time.deltaTime;
                if (t > 3)
                {
                    //meshRenderer.material = mutual;
                    //gazeTransmissionObject.Despawn();
                    //gazeTransmissionObject = Transmission.Spawn("CursorM", Vector3.zero, Quaternion.identity, Vector3.one);
                    //gazeTransmissionObject.motionSource = gameObject.transform;
                }
            }
            else
            {
                //meshRenderer.material = startingC;
                t = 0;
                //gazeTransmissionObject.Despawn();
            }
        }
    }
    #endregion
}