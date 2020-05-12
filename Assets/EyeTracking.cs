using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class EyeTracking : MonoBehaviour
{

    public GameObject Camera;
    public MeshRenderer meshRenderer;
    public Material startingC, changingC;

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
    public Color startColor;
    public Color endColor;
    //public LaserPointer lineRenderer;

    private float t = 0;
    //public bool testLerp = false;
    public float smoothing = 10;

    /***
     * spatial alignment
     * 
     ***/
    public ControlInput controlLocator;
    public Text info;

    private List<TransmissionObject> _spawned = new List<TransmissionObject>();
    private string _initialInfo;

    /**
     * Playspace
     * **/
    //Public Variables:
    //public Transform primaryWallPlaque;
    //public Transform rearWallPlaque;
    //public Transform rightWallPlaque;
    //public Transform leftWallPlaque;
    //public Transform centerPlaque;
    //public Transform floorPlaque;
    //public Transform ceilingPlaque;

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
        TransmissionObject gazeTransmissionObject = Transmission.Spawn("CursorB", Vector3.zero, Quaternion.identity, Vector3.one);
        gazeTransmissionObject.motionSource = gameObject.transform;

        //share gaze locatorv2: Not sure how to change?
        //TransmissionObject gazeTransmissionObject = Transmission.Spawn("GazePoint", Vector3.zero, Quaternion.identity, Vector3.one);
        //gazeTransmissionObject.motionSource = gameObject.transform;

        //sets:
        _initialInfo = info.text;

        /**
         * PlaySpace
         * **/
        //hooks:
        //Playspace.Instance.OnCleared.AddListener(HandleCleared);
        //Playspace.Instance.OnCompleted.AddListener(HandleCompleted);
    }

    //private void HandleCleared()
    //{
    //    primaryWallPlaque.gameObject.SetActive(false);
    //    rearWallPlaque.gameObject.SetActive(false);
    //    rightWallPlaque.gameObject.SetActive(false);
    //    leftWallPlaque.gameObject.SetActive(false);
    //    ceilingPlaque.gameObject.SetActive(false);
    //    floorPlaque.gameObject.SetActive(false);
    //    centerPlaque.gameObject.SetActive(false);
    //}

    //private void HandleCompleted()
    //{
    //    //place plaques:
    //    PlayspaceWall primaryWall = Playspace.Instance.Walls[Playspace.Instance.PrimaryWall];
    //    primaryWallPlaque.gameObject.SetActive(true);
    //    primaryWallPlaque.position = primaryWall.Center + Vector3.up * .5f;
    //    primaryWallPlaque.rotation = Quaternion.LookRotation(primaryWall.Back);

    //    PlayspaceWall rearWall = Playspace.Instance.Walls[Playspace.Instance.RearWall];
    //    rearWallPlaque.gameObject.SetActive(true);
    //    rearWallPlaque.position = rearWall.Center + Vector3.up * .5f;
    //    rearWallPlaque.rotation = Quaternion.LookRotation(rearWall.Back);

    //    PlayspaceWall rightWall = Playspace.Instance.Walls[Playspace.Instance.RightWall];
    //    rightWallPlaque.gameObject.SetActive(true);
    //    rightWallPlaque.position = rightWall.Center + Vector3.up * .5f;
    //    rightWallPlaque.rotation = Quaternion.LookRotation(rightWall.Back);

    //    PlayspaceWall leftWall = Playspace.Instance.Walls[Playspace.Instance.LeftWall];
    //    leftWallPlaque.gameObject.SetActive(true);
    //    leftWallPlaque.position = leftWall.Center + Vector3.up * .5f;
    //    leftWallPlaque.rotation = Quaternion.LookRotation(leftWall.Back);

    //    ceilingPlaque.gameObject.SetActive(true);
    //    ceilingPlaque.position = Playspace.Instance.CeilingCenter;
    //    ceilingPlaque.rotation = Quaternion.LookRotation(Vector3.up, primaryWall.Forward);

    //    floorPlaque.gameObject.SetActive(true);
    //    floorPlaque.position = Playspace.Instance.FloorCenter;
    //    floorPlaque.rotation = Quaternion.LookRotation(Vector3.down, primaryWall.Back);

    //    centerPlaque.gameObject.SetActive(true);
    //    centerPlaque.position = Playspace.Instance.Center;
    //}

    //Event Handlers:
    private void HandleTriggerDown()
    {
        //stamp a cube in space:
        TransmissionObject spawn = Transmission.Spawn("SampleTransmissionObject", controlLocator.Position, controlLocator.Orientation, Vector3.one);
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
            //original
            //meshRenderer.transform.position = MLEyes.FixationPoint; 

            //v1
            //filterd = Vector3.Lerp(filterd, MLEyes.FixationPoint, 0.7f);
            //gameObject.transform.position = filterd;
            // oldPos = gameObject.transform.position;

            /***
             *
             * Raycast
             */

            //RaycastHit rayHit;
            //_heading = MLEyes.FixationPoint - Camera.transform.position;


            //if (Physics.Raycast(Camera.transform.position, _heading, out rayHit, 10.0f))
            //{
            //    lineRenderer.useWorldSpace = true;
            //    lineRenderer.SetPosition(0, Camera.transform.position);
            //    lineRenderer.SetPosition(1, rayHit.point);
            //}
            //else
            //{
            //    lineRenderer.useWorldSpace = false;
            //    lineRenderer.SetPosition(0, Camera.transform.position);
            //    lineRenderer.SetPosition(1, Vector3.forward * 5);
            //}

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
            gameObject.transform.position = Vector3.MoveTowards(transform.position, currPos, smoothing * Time.deltaTime);

            //transform.position = Vector3.MoveTowards(transform.position, MLEyes.FixationPoint, smoothing * Time.deltaTime);

            //gaze pointer rotation offset
            gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
            MLdebugger.text = MLEyes.FixationPoint + "\n" + currPos + "\n" + offsetRot + "\n"+ t + "\n";



            

            Vector3 offset = MLEyes.FixationPoint - currPos;
            float offsetPos = offset.magnitude;
/*          float x = Mathf.Abs(MLEyes.FixationPoint.x - currPos.x);
            float y = Mathf.Abs(MLEyes.FixationPoint.y - currPos.y);
            float z = Mathf.Abs(MLEyes.FixationPoint.z - currPos.z);
            offsetPos = new Vector3(x, y, z);*/

            if (offsetPos < 0.05)
            {
                t += Time.deltaTime;
                if (t > 3)
                {
                    meshRenderer.material = changingC;
                }              
            }
            else {
                meshRenderer.material = startingC;
                t = 0;
            }



            //V3
            /*            sum2 -= buffer[bufferIndex]; // subtract the oldest value
                        buffer[bufferIndex] = MLEyes.FixationPoint;
                        sum2 += buffer[bufferIndex]; // add the newest value
                        bufferIndex = (bufferIndex + 1) % 5;

                        gameObject.transform.position = sum2 / 5;*/



        }
    }
    #endregion
}

public class LaserPointer
{
}