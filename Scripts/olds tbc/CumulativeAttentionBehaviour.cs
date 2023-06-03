using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class CumulativeAttentionBehaviour : MonoBehaviour
    {
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        [field: SerializeField] public AttentionDatastore AttentionDatastore { get; set; }

        private float? cumulativeAttentionRating = null;
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        [Tooltip("Calls code in Start, useful for a one-off action based on current attention when an object is activated.")]
        public bool StartCheck;
        [Tooltip("Calls code in Update, useful for continuously change in response to current attention.")]
        public bool UpdateCheck;

        void Start()
        {
            // TODO: try get tracker, if not live then go to datastore.
            if (AttentionTracker)
            {
                CumulativeAttentionRating = AttentionTracker.CumulativeAttention;
            }
            else
            {
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == AttentionTracker.name).cumulativeAttention;
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            if (StartCheck)
            {
                CumulativeAttentionReaction(CumulativeAttentionRating);
            }
        }

        void Update()
        {
            // TODO: try get tracker, if not live then go to datastore.
            if (AttentionTracker)
            {
                CumulativeAttentionRating = AttentionTracker.CumulativeAttention;
            }
            else
            {
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == AttentionTracker.name).cumulativeAttention;
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            if (UpdateCheck)
            {
                CumulativeAttentionReaction(CumulativeAttentionRating);
            }
        }

        public virtual void CumulativeAttentionReaction(float? CumulativeAttentionRating)
        {

        }
    }
}