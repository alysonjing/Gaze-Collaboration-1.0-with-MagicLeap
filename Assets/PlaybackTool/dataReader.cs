using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class dataReader : MonoBehaviour
{
    public GameObject cubeLocalPlaybackObj;
    public GameObject cubeRemotePlaybackObj;
    public Slider slider;
    public Text timeStampText;
    public string importFile = "";
    private const string DIR = "Logs/";
    private bool dataInitialized = false;

    private void Update() {
        if (dataInitialized) {
            setCube((int)slider.value);
        }
    }

    public void setCube(int slider) {
        Debug.Log("At pos:" + slider + " | Local User:" + data[slider].getPositionL() + " | Remote User:" + data[slider].getPositionR());
        timeStampText.text = "Timestamp = "+ data[slider].getTimeStamp();
        cubeLocalPlaybackObj.transform.position = data[slider].getPositionL();
        cubeRemotePlaybackObj.transform.position = data[slider].getPositionR();
    }

    // Start is called before the first frame update
    void Start() {
        cubeLocalPlaybackObj.SetActive(true);
        cubeRemotePlaybackObj.SetActive(true);
        readData();
    }

    public int getLines(StreamReader reader) {
        int count = 0;
        while (reader.ReadLine() != null) {
            count++;
        }
        return count;
    }
    storeData[] data = null;
    public void readData() {
        using (var reader = new StreamReader(@DIR + importFile + ".csv")) {
            int lineCount = getLines(new StreamReader(@DIR + importFile + ".csv"));
            slider.maxValue = lineCount;
            Debug.Log("Lines:" + lineCount);
            data = new storeData[lineCount];
            int count = 0;
            while (!reader.EndOfStream) {
                var line = reader.ReadLine();
                string[] split = line.Split(',');
                if (count >= 1) {
                    //Debug.Log(data[count-1]);
                    if (split.Length > 3) {
                        data[count - 1] = new storeData();
                        data[count-1].setTimeStamp(split[0]);
                        data[count-1].setPositionL(new Vector3(float.Parse(split[1]), float.Parse(split[2]), float.Parse(split[3])));
                        data[count-1].setPositionR(new Vector3(float.Parse(split[4]), float.Parse(split[5]), float.Parse(split[6])));
                        //Debug.Log("Timestamp:" + split[0] + ", xL:" + split[1] + ", yL:" + split[2] + ", zL" + split[3] + ", xR:" + split[4] + ", yR:" + split[5] + ", zR" + split[6]);
                    }
                }
                count++;
            }
        }
        dataInitialized = true;
    }

}
