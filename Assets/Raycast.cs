using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using MagicLeapTools;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

public class Raycast : MonoBehaviour
{

    public Transform camTransform;
    public GameObject cursor;
    //public MeshRenderer meshRenderer;
    //public Material startingC, changingC, mutual;
    //public float smoothing = 10;


    private MLRaycast.QueryParams _raycastParams = new MLRaycast.QueryParams();
    private Vector3 rotation;
    private Vector3 offsetRot;
   // public Vector3 currPos;
    //private Camera mainCam;
    //private TextMesh MLdebugger;
    //private Vector3 _heading;
    //private float t = 0;
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
       //TransmissionObject headTransmissionObject = Transmission.Spawn("CursorB", Vector3.zero, Quaternion.identity, Vector3.one);
        //headTransmissionObject.motionSource = cursor.transform;

        //shared controll locator:
        TransmissionObject controlTransmissionObject = Transmission.Spawn("SampleTransmissionObject", Vector3.zero, Quaternion.identity, Vector3.one);
        controlTransmissionObject.motionSource = controlLocator.transform;

        //share gaze locator: Not sure how to change?
        gazeTransmissionObject = Transmission.Spawn("SampleTransmissionObject", Vector3.zero, Quaternion.identity, Vector3.one);
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
        MLRaycast.Start();
        //meshRenderer = gameObject.GetComponent<MeshRenderer>();
        //transform.position = cursor.transform.position + cursor.transform.forward * 2.0f;

        //cursorInstance = Instantiate(cursorPrefab);
    }


    private void OnDestroy()
    {
        // Stop raycasting.
        MLRaycast.Stop();
    }

    void Update()
    {
        //Spatial alignment
        string output = _initialInfo + System.Environment.NewLine;
        output += "Peers Available: " + Transmission.Instance.Peers.Length + System.Environment.NewLine;
        output += "Localized: " + SpatialAlignment.Localized;

        info.text = output;
        //end 


        // Update the orientation data in the raycast parameters.
        _raycastParams.Position = camTransform.position;
        _raycastParams.Direction = camTransform.forward;
        _raycastParams.UpVector = camTransform.up;
        //_raycastParams.Rotation = camTransform.rotation;

        // Make a raycast request using the raycast parameters 
        MLRaycast.Raycast(_raycastParams, HandleOnReceiveRaycast);

        /***
         *
         * Raycast
         */

        //RaycastHit rayHit;
        //    _heading = MLEyes.FixationPoint - Camera.transform.position;

        //    if (Physics.Raycast(Camera.transform.position, _heading, out rayHit, 10.0f))
        //    {
        //        gameObject.transform.position = rayHit.transform.position;
        //        gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;


        //    }
        //    else
        //    {

        //    }


            //gameObject.transform.position = Vector3.MoveTowards(transform.position, currPos, smoothing * Time.deltaTime);


            //gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
            //MLdebugger.text = MLEyes.FixationPoint + "\n" + currPos + "\n" + offsetRot + "\n"+ t + "\n";

            //Vector3 offset = MLEyes.FixationPoint - currPos;
            //float offsetPos = offset.magnitude;

            //if (offsetPos < 0.05)
            //{
               // t += Time.deltaTime;
                //if (t > 3)
               // {
                    //meshRenderer.material = mutual;

                //}              
           // }
           // else {
                //meshRenderer.material = startingC;

            //}
    }
    #endregion
    private IEnumerator NormalMarker(Vector3 point, Vector3 normal)
    {
        // Rotate the prefab to match given normal.
        Quaternion rotation = Quaternion.FromToRotation(Vector3.up, normal);
        //rotation.eulerAngles = Camera.transform.eulerAngles + offsetRot; //test offset rotation: AJ->did nont work
        //gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;

        // Instantiate the prefab at the given point.
        GameObject go = Instantiate(cursor, point, rotation);
        // Wait 2 seconds then destroy the prefab.
        yield return new WaitForSeconds(0);
        Destroy(go);
    }

    void HandleOnReceiveRaycast(MLRaycast.ResultState state,
                                UnityEngine.Vector3 point, Vector3 normal,
                                float confidence)
    {
        if (state == MLRaycast.ResultState.HitObserved)
        {
            StartCoroutine(NormalMarker(point, normal));
        }
    }
}
