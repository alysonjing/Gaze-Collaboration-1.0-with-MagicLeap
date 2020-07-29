using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class csvWriter : MonoBehaviour {

    /// ===============================
    /// AUTHOR: Kieran William May
    /// PURPOSE: Writes to and generates a .csv file
    /// NOTES:
    /// (If CSV file(s) with the same name already exists it will loop through using a counter until it finds a name that doesn't exist)
    /// ===============================

    public bool logData;
    public bool saveData;
    public string dataTest;
    public string start = "";
    private List<string> logInfo = new List<string>();

    private const string FILE_NAME = "trial";
    private const string DIR = "/LogTest/";

    public void WriteLine(string line) {
        logInfo.Add(line);
    }

    private void Start() {
        WriteLine("Timestamp, xL, yL, zL, xR, yR, zR");
        Debug.Log("Application is starting..");
        dataTest = "starting";
    }

    void OnApplicationQuit() {
        Debug.Log("Application Exited - Automatically logging data..");
        writeToFile();
    }

    private void Update() {
        if (saveData) {
            saveData = false;
            Debug.Log("Data saved..");
            writeToFile();
        }
    }

    public void writeToFile() {
        dataTest = "method called";
        ///
        //start = ("Writen to file:" + Application.dataPath);
        //string dest = Application.dataPath + "/" + FILE_NAME + ".csv";
        string dest ="/documents/C2/" + FILE_NAME + ".csv";
        StreamWriter writer = null;
        int count = 1;
        bool foundPath = false;
        while (foundPath == false) {
            if (File.Exists(dest)) {
                dataTest = "file exists...";
                dest = "/documents/C2/" + FILE_NAME + count + ".csv";
                //dest = "C:/Users/ECL/MagicLeap/captures/" + FILE_NAME + count + ".csv";
                //dest = "Logs/trial" + count + ".csv";
                count++;
            } else {
                print("Found path:" + dest);
                dataTest = "file complete";
                writer = new StreamWriter(dest, true) as StreamWriter;
                foundPath = true;
            }
        }
        print("Writing..");
        dataTest += "\n writing..";
        for (int i = 0; i < logInfo.Count; i++) {
            //print(logInfo[i]);
            writer.Write(logInfo[i]);
            writer.WriteLine();
        }
        print("Writen to file:" + dest);
        start = ("Writen to file:" + dest);
        dataTest = "file finished";
        writer.Close();
    }
}