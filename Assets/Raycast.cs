﻿using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class Raycast : MonoBehaviour
{

    public GameObject Camera;
    public GameObject cursor;
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


    private float t = 0;
    //public bool testLerp = false;
    public float smoothing = 10;

    public TransmissionObject gazeTransmissionObject;

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
       TransmissionObject headTransmissionObject = Transmission.Spawn("Cursor", Vector3.zero, Quaternion.identity, Vector3.one);
       headTransmissionObject.motionSource = Camera.transform;

        //shared controll locator:
        TransmissionObject controlTransmissionObject = Transmission.Spawn("SampleTransmissionObjectP", Vector3.zero, Quaternion.identity, Vector3.one);
        controlTransmissionObject.motionSource = controlLocator.transform;

        //share gaze locator: Not sure how to change?
        gazeTransmissionObject = Transmission.Spawn("Cursor", Vector3.zero, Quaternion.identity, Vector3.one);
        gazeTransmissionObject.motionSource = gameObject.transform;

        //sets:
        _initialInfo = info.text;

  
    }


    //Event Handlers:
    private void HandleTriggerDown()
    {
        //stamp a cube in space:
        TransmissionObject spawn = Transmission.Spawn("SampleTransmissionObjectP", controlLocator.Position, controlLocator.Orientation, Vector3.one);
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
        //MLEyes.Start();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //transform.position = Camera.transform.position + Camera.transform.forward * 2.0f;

        //cursorInstance = Instantiate(cursorPrefab);


    }
    private void OnDisable()
    {
        //MLEyes.Stop();
    }
    private Vector3 hitpoint;
    void Update()
    {
        //Spatial alignment
        string output = _initialInfo + System.Environment.NewLine;
        output += "Peers Available: " + Transmission.Instance.Peers.Length + System.Environment.NewLine;
        output += "Localized: " + SpatialAlignment.Localized + System.Environment.NewLine;
        //output += "Hitpoint: " + hitpoint + "pos" + gazeTransmissionObject.transform.position + " | " + gazeTransmissionObject.gameObject.activeInHierarchy;

        info.text = output;

        /***
         *
         * Raycast
         */

        RaycastHit rayHit;
        //_heading = MLEyes.FixationPoint - Camera.transform.position;
        //if (Physics.Raycast(Camera.transform.position, transform.forward, out rayHit))
        Debug.Log("here..");
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out rayHit))
        {
            Debug.Log("ray hitting:" + rayHit.point + " , hit:" + rayHit.transform.gameObject);
            Debug.DrawRay(Camera.transform.position, Camera.transform.forward, Color.red);
            hitpoint = new Vector3(rayHit.point.x, rayHit.point.y, rayHit.point.z- 0.025f);
            //gazeTransmissionObject.transform.position = hitpoint;
            gameObject.transform.position = hitpoint;
            //cursor.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
            //gazeTransmissionObject.transform.rotation = Quaternion.Euler(rayHit.normal);
            //gazeTransmissionObject.transform.rotation = Camera.transform.rotation;
            gameObject.transform.rotation = Camera.transform.rotation;
            //GameObject headgaze = Instantiate(cursor, rayHit.point, Quaternion.Euler(rayHit.normal));
            //gazeTransmissionObject.motionSource = gameObject.transform;
        }
        //end 

        //original gaze script
        //if (MLEyes.IsStarted)
        //{

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
            // gameObject.transform.position = currPos;
            //gameObject.transform.position = Vector3.MoveTowards(transform.position, currPos, smoothing * Time.deltaTime);
            //transform.position = Vector3.MoveTowards(transform.position, MLEyes.FixationPoint, smoothing * Time.deltaTime);
            //gaze pointer rotation offset
            //gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
            //MLdebugger.text = MLEyes.FixationPoint + "\n" + currPos + "\n" + offsetRot + "\n"+ t + "\n";

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
            else {
                //meshRenderer.material = startingC;
                //t = 0;
                //gazeTransmissionObject.Despawn();
            }



            //V3
            /*            sum2 -= buffer[bufferIndex]; // subtract the oldest value
                        buffer[bufferIndex] = MLEyes.FixationPoint;
                        sum2 += buffer[bufferIndex]; // add the newest value
                        bufferIndex = (bufferIndex + 1) % 5;

                        gameObject.transform.position = sum2 / 5;*/



        //}
    }
    #endregion
}
