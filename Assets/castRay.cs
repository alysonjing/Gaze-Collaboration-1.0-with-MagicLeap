using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class castRay : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit rayHit;
        //_heading = MLEyes.FixationPoint - Camera.transform.position;
        //if (Physics.Raycast(Camera.transform.position, transform.forward, out rayHit))
        Debug.Log("here..");
        if (Physics.Raycast(transform.position, transform.forward, out rayHit))
        {
            Debug.Log("ray hitting:" + rayHit.point + " , hit:" + rayHit.transform.gameObject);
            Debug.DrawRay(transform.position, transform.forward, Color.blue);
            gameObject.transform.position = rayHit.point;
            //gameObject.transform.eulerAngles = Camera.transform.eulerAngles + offsetRot;
            gameObject.transform.rotation = Quaternion.Euler(rayHit.normal);
            //GameObject headgaze = Instantiate(cursor, rayHit.point, Quaternion.Euler(rayHit.normal));
        }
    }
}
