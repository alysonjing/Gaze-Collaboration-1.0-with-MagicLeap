using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;

[RequireComponent(typeof(MLPrivilegeRequesterBehavior))]

public class ImageTrackingManager : MonoBehaviour
{
    public GameObject TrackedImage;
    private MLPrivilegeRequesterBehavior _privRequester = null;

    void Awake()
    {
        _privRequester = GetComponent<MLPrivilegeRequesterBehavior>();
        _privRequester.OnPrivilegesDone += HandlePrivilegesDone;
    }

    void HandlePrivilegesDone(MLResult result)
    {
        if (!result.IsOk)
        {
            Debug.Log("Error: Priv Request Failed");
        }
        else
        {
            Debug.Log("Success: Priv granted");
            StartCapture();
        }
    }

    void StartCapture()
    {
        TrackedImage.SetActive(true);
    }
}

