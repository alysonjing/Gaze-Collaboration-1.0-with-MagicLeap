using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logdata : MonoBehaviour {
    public csvWriter writer;
    public float time_elapsed = 0f;
    public float local_time_elapsed = 0f;
    public GameObject[] cursorObjs;

    public void logData() {
        //transmissionColorB.currentPos.
        float xL = transmissionColorB.currentPos.x;
        float yL= transmissionColorB.currentPos.y;
        float zL= transmissionColorB.currentPos.z;
        float xR = transmissionColor.currentPos.x;
        float yR = transmissionColor.currentPos.y;
        float zR = transmissionColor.currentPos.z;
        writer.WriteLine(time_elapsed + ","+xL+","+yL+","+zL + "," + xR + "," + yR + "," + zR);

        //output += " |B : " + transmissionColorB.currentMaterial; //debug Blue
        //output += " |P : " + transmissionColor.currentMaterial; //debug Pink
    }

    // Update is called once per frame
    void Update() {
        if (cursorObjs.Length >= 2) {
            time_elapsed += Time.deltaTime; // Get the overall time the app is running.
            local_time_elapsed += Time.deltaTime;
            if (local_time_elapsed > 0.2f)
            {
                logData(); // Logged once every 100ms
                local_time_elapsed = 0f;
            }
        } else {
            //cursorObjs = FindObjectsOfType<castray>();
            cursorObjs = GameObject.FindGameObjectsWithTag("gazePointer");
        }
    }
}
