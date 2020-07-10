using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.MagicLeap;
using MagicLeap.Core;


[RequireComponent(typeof(MLImageTrackerBehavior))]
public class TrackingVisuals : MonoBehaviour
{

    private bool _targetFound = false;
    private MLImageTrackerBehavior _trackingBehaviour = null;
    public GameObject TrackedObject;
    void Start()
    {
        _trackingBehaviour = GetComponent<MLImageTrackerBehavior>();
        _trackingBehaviour.OnTargetFound += OnTargetFound;
        _trackingBehaviour.OnTargetLost += OnTargetLost;
        _trackingBehaviour.OnTargetUpdated += OnTargetUpdated;

        RefreshViewMode();
    }

    void OnTargetFound(MLImageTracker.Target target, MLImageTracker.Target.Result result)
    {
        _targetFound = true;
        RefreshViewMode();
    }

    void OnTargetLost(MLImageTracker.Target target, MLImageTracker.Target.Result result)
    {
        _targetFound = false;
        RefreshViewMode();
    }

    void OnTargetUpdated(MLImageTracker.Target target, MLImageTracker.Target.Result result)
    {
        transform.position = result.Position;
        transform.rotation = result.Rotation;
    }

    void OnDestroy()
    {
        _trackingBehaviour.OnTargetLost -= OnTargetLost;
        _trackingBehaviour.OnTargetFound -= OnTargetFound;
        _trackingBehaviour.OnTargetUpdated -= OnTargetUpdated;
    }

    void RefreshViewMode()
    {
        if (_targetFound)
        {
            TrackedObject.SetActive(true);
        }
        else
        {
            TrackedObject.SetActive(false);
        }
    }
}
