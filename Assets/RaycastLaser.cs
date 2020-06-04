using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class RaycastLaser : MonoBehaviour
{

    public GameObject Camera;
    public Color startColor;
    public Color endColor;

    Vector3 filterd = new Vector3();
    int bufferIndex = 0;
    Vector3[] buffer = new Vector3[5];
    Vector3 sum2 = Vector3.zero;

    [SerializeField]
    private Vector3 offsetRot;
    private Vector3 currPos;
    private Vector3 _heading;
    private LineRenderer lineRenderer;


    private float t = 0;
    //public bool testLerp = false;
    public float smoothing = 10;

    /***
     * spatial alignment
     * 
     ***/
    public ControlInput controlLocator;
    public Text info;
    public TextMesh debug;

    private List<TransmissionObject> _spawned = new List<TransmissionObject>();
    private string _initialInfo;

    //public Vector3 fixationPoint;

    //spacial alignment Init:
    private void Awake()
    {
        //hooks:
        controlLocator.OnTriggerDown.AddListener(HandleTriggerDown);
        controlLocator.OnBumperDown.AddListener(HandleBumperDown);

        //shared head locator:
        TransmissionObject headTransmissionObject = Transmission.Spawn("SampleTransmissionObjectP", Vector3.zero, Quaternion.identity, Vector3.one);
        headTransmissionObject.motionSource = Camera.transform;

        //shared controll locator:
        TransmissionObject controlTransmissionObject = Transmission.Spawn("SampleTransmissionObjectP", Vector3.zero, Quaternion.identity, Vector3.one);
        controlTransmissionObject.motionSource = controlLocator.transform;

        //share gaze locator: Not sure how to change?
        TransmissionObject gazeTransmissionObject = Transmission.Spawn("Laser", Vector3.zero, Quaternion.identity, Vector3.one);
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
        //spawn.transform.position = new Vector3(x, y, z);

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
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();
        transform.position = Camera.transform.position + Camera.transform.forward * 2.0f;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startColor = startColor;
        lineRenderer.endColor = endColor;
    }

    private void OnDisable()
    {
        //MLEyes.Stop();
    }

    void Update()
    {
        //Spatial alignment
        string output = _initialInfo + System.Environment.NewLine;
        output += "Peers Available: " + Transmission.Instance.Peers.Length + System.Environment.NewLine;
        output += "Localized: " + SpatialAlignment.Localized;

        info.text = output;
        //end 

        

        RaycastHit rayHit;
        if (Physics.Raycast(Camera.transform.position, Camera.transform.forward, out rayHit))
        {            
            
            currPos = rayHit.point;
            gameObject.transform.position = currPos;

            Vector3 delta = (currPos - Camera.transform.position).normalized;
            delta *= .05f;
            lineRenderer.useWorldSpace = true;
            lineRenderer.SetPosition(0, Camera.transform.position);
            lineRenderer.SetPosition(1, currPos + delta);

            // update the transmission object by updating my own pos and scale as two end points of line renderer
            gameObject.transform.position = Camera.transform.position;
            gameObject.transform.localScale = currPos;




            Vector3 offset = MLEyes.FixationPoint - currPos;
            float offsetPos = offset.magnitude;

            if (offsetPos < 0.05)
            {
                t += Time.deltaTime;
                if (t > 3)
                {
                    lineRenderer.startColor = startColor;
                }
            }
            else
            {
                lineRenderer.endColor = endColor;
                t = 0;
            }

        }
    }
    #endregion
}
