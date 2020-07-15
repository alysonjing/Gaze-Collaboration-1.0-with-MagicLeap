using MagicLeapTools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshController : MonoBehaviour
{
    public ControlInput controlLocator;


    private void Awake()
    {
        //hooks:
        controlLocator.OnTriggerDown.AddListener(HandleTriggerDown);

    }

    //Event Handlers:
    private void HandleTriggerDown()
    {

        GetComponent<MeshRenderer>().enabled = !GetComponent<MeshRenderer>().enabled;

    }
}
