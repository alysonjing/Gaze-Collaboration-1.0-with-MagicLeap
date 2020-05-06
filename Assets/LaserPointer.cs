using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MLToolsAndThings
{
    public class LaserPointer : MonoBehaviour
    {

        [SerializeField]
        private LineRenderer lineRenderer;

        // Startpoint of the laserpointer
        [SerializeField]
        private Vector3 start;

        // Endpoint of the laserpointer
        [SerializeField]
        private Vector3 end;

        private void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Update is called once per frame
        void Update()
        {
            // update line
            if (lineRenderer != null)
            {
                lineRenderer.SetPosition(0, start);
                lineRenderer.SetPosition(1, end);
            }
        }

        public void setEndPoint(Vector3 endPoint)
        {
            this.end = endPoint;
        }

        public void setStartPoint(Vector3 startPoint)
        {
            this.start = startPoint;
        }
    }
}
