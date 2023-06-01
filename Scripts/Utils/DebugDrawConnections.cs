using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class DebugDrawConnections : MonoBehaviour
    {
        public AttentionTracker tracker;
        private LineRenderer connectionLine;
        public Material lineMaterial;
        public Color gizmoLineColor = Color.red;
        // Start is called before the first frame update
        void Start()
        {
            if (GetComponent<LineRenderer>())
            {
                connectionLine = GetComponent<LineRenderer>();
            }
            else
            {
                gameObject.AddComponent<LineRenderer>();
                connectionLine = GetComponent<LineRenderer>();
            }
            if (tracker.gameObject != gameObject)
            {
                connectionLine.material = lineMaterial;
                connectionLine.startWidth = 0.05f;
                connectionLine.endWidth = 0.05f;
                connectionLine.SetPosition(0, tracker.gameObject.transform.position);
                connectionLine.startColor = Color.yellow;
                connectionLine.SetPosition(1, transform.position);
                connectionLine.endColor = Color.red;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKey(KeyCode.E))
            {
                connectionLine.enabled = true;
            } else
            {
                connectionLine.enabled = false;
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Gizmos.color = gizmoLineColor;
            Gizmos.DrawLine(tracker.gameObject.transform.position, transform.position);
        }
#endif
    }
}
