using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class trailEffect : MonoBehaviour
{

    [SerializeField]
    private int frames = 200;
    private GameObject[] trailObjects;

    public GameObject trailPrefab;

    private GameObject parent;

    public void populateTrailObjects()
    {
        for (int i = 0; i < trailObjects.Length; i++)
        {
            trailObjects[i] = Instantiate(trailPrefab);
            trailObjects[i].transform.SetParent(parent.transform);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        parent = new GameObject();
        trailObjects = new GameObject[frames];
        populateTrailObjects();
    }

    int currFrame = 0;
    float timer = 0f;
    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 0.1f)
        {
            currFrame++;
            /*if (q.Count <= 20) {
                q.Enqueue(this.transform.position);
            } else {
                currFrame++;
                if (currFrame > 20)
                    currFrame = 0;
                var deq = q.Dequeue();
                q.Enqueue(this.transform.position);*/
            if (currFrame >= frames)
            {
                currFrame = 0;
            }
            trailObjects[currFrame].transform.position = this.transform.position;
            timer = 0f;
            //Debug.Log("Dequeing:" + deq + " and Enqueing:" + this.transform.position);
        }
    }
}