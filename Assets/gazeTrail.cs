using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class gazeTrail : MonoBehaviour
{

    public GameObject Camera;
    public Vector3 currPos;
    //public MeshRenderer meshRenderer;
    public Material startingC, changingC, mutual;
    //public bool testLerp = false;
    public float smoothing = 10;
    public Vector3 offset;
    public float offsetPos;

    Vector3 filterd = new Vector3();
    int bufferIndex = 0;
    Vector3[] buffer = new Vector3[5];
    Vector3 sum2 = Vector3.zero;

    [SerializeField]
    private Vector3 offsetRot;
    //private Vector3 offsetPos;
    
    private Camera mainCam;
    private TextMesh MLdebugger;
    private Renderer _gazeRenderer;

    private Vector3 _heading;
    private Vector3 hitpoint;
    private float t = 0;

 

    /***
     * spatial alignment
     * 
     ***/
    public ControlInput controlLocator;
    public Text info;

    private List<TransmissionObject> _spawned = new List<TransmissionObject>();
    private string _initialInfo;

    TransmissionObject gazeTransmissionObject;
    //TransmissionObject gazeTransmissionObject2;


    //spacial alignment Init:
    private void Awake()
    {
        //hooks:
        controlLocator.OnTriggerDown.AddListener(HandleTriggerDown);
        controlLocator.OnBumperDown.AddListener(HandleBumperDown);

        //shared head locator:
        TransmissionObject headTransmissionObject = Transmission.Spawn("CursorP", Vector3.zero, Quaternion.identity, Vector3.one);
        headTransmissionObject.motionSource = Camera.transform;

        //shared controll locator:
        TransmissionObject controlTransmissionObject = Transmission.Spawn("SampleTransmissionObjectP", Vector3.zero, Quaternion.identity, Vector3.one);
        controlTransmissionObject.motionSource = controlLocator.transform;

        //share gaze locator: Not sure how to change?
        gazeTransmissionObject = Transmission.Spawn("TrailP", Vector3.zero, Quaternion.identity, Vector3.one);
        _gazeRenderer = gazeTransmissionObject.GetComponent<Renderer>();
        gazeTransmissionObject.GetComponent<TrailRenderer>().enabled = false;  //debug > should turn to false in user study
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

    public void SendFixation()
    {
        RPCMessage rpcMessage = new RPCMessage("ChangeFixation");
        Transmission.Send(rpcMessage);
    
    }

    public void SendPink()
    {
        RPCMessage rpcMessage = new RPCMessage("ChangePink");
        Transmission.Send(rpcMessage);

    }

    public void SendBlue()
    {
        RPCMessage rpcMessage = new RPCMessage("ChangeBlue");
        Transmission.Send(rpcMessage);

    }

    public void ChangeFixation()
    {
        _gazeRenderer.material= mutual;
    }

    public void ChangePink()
    {
        _gazeRenderer.material = startingC;
    }

    public void ChangeBlue()
    {
        _gazeRenderer.material = changingC;
    }



    #region Unity Methods
    void Start()
    {
        MLEyes.Start();
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();
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
        //output += " | : " + transmissionColorB.currentMaterial; //debug Blue
        //output += " | : " + transmissionColor.currentMaterial; //debug Pink
        //output += "currentPos" + transmissionColorB.currentPos;

        info.text = output;
        //end 

        //original gaze script
        if (MLEyes.IsStarted)
        {

            /**
             * position for the cursor
             **/
            RaycastHit rayHit;
            _heading = MLEyes.FixationPoint - Camera.transform.position;
            if (Physics.Raycast(Camera.transform.position, _heading, out rayHit))
            {
                
                buffer[bufferIndex] = rayHit.point;
                bufferIndex = (bufferIndex + 1) % 5;

                Vector3 sum = Vector3.zero;
                for (int i = 0; i < 5; i++)
                {
                    sum += buffer[i];
                }
                currPos = sum / 5;
                hitpoint = new Vector3(currPos.x, currPos.y, currPos.z) - (_heading.normalized * 0.05f);

                gameObject.transform.position = Vector3.MoveTowards(transform.position, hitpoint, smoothing * Time.deltaTime);
                //gameObject.transform.rotation = Camera.transform.rotation;
                gameObject.transform.rotation = Quaternion.FromToRotation(Vector3.back, rayHit.normal);

                /**
                * Offset position radius
                **/
                offset = MLEyes.FixationPoint - currPos;
                offsetPos = offset.magnitude;

                if (offsetPos < 0.05)
                {
                    t += Time.deltaTime;
                    if (t > 3)
                    {
                        //meshRenderer.material = mutual;
                        //SendFixation();

                    }
                }
                else
                {
                    //meshRenderer.material = startingC;
                    //t = 0;
                    //SendPink();
                    //SendBlue();
                }

            }
            else
            {

                //buffer[bufferIndex] = MLEyes.FixationPoint;
                //bufferIndex = (bufferIndex + 1) % 5;

                //Vector3 sum = Vector3.zero;
                //for (int i = 0; i < 5; i++)
                //{
                //    sum += buffer[i];
                //}
                //currPos = sum / 5;

                //gameObject.transform.position = Vector3.MoveTowards(transform.position, currPos, smoothing * Time.deltaTime);
                //gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;

            }
        }
    }
    #endregion
}
