using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class ComparisonAttentionBehaviour : MonoBehaviour
    {
        // Attention Ratings Sources
        [field: SerializeField] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public List<AttentionTracker> AttentionTrackers { get; set; }

        void Start()
        {
            foreach (var tracker in AttentionTrackers)
            {
                if (tracker == null)
                {
                    tracker.CumulativeAttention = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == tracker.name).cumulativeAttention;
                } 
            }
            // Do Behaviour
        }

        void Update()
        {
            // Do Behaviour
        }
    }
}
