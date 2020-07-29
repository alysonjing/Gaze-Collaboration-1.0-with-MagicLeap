using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class storeData : MonoBehaviour {

    private string timeStamp;
    private Vector3 positionL;
    private Vector3 positionR;

    public void setTimeStamp(string timeStamp) {
        this.timeStamp = timeStamp;
    }

    public void setPositionL(Vector3 positionL) {
        this.positionL = positionL;
    }

    public void setPositionR(Vector3 positionR) {
        this.positionR = positionR;
    }

    public string getTimeStamp() {
        return timeStamp;
    }

    public Vector3 getPositionL() {
        return positionL;
    }

    public Vector3 getPositionR() {
        return positionR;
    }
}
