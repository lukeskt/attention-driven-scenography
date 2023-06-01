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
            CumulativeAttentionRating = AttentionTracker.CumulativeAttention;
            if (UpdateCheck)
            {
                CumulativeAttentionReaction(CumulativeAttentionRating);
            }
        }

        public virtual void CumulativeAttentionReaction(float? CumulativeAttentionRating)
        {

        }

        public Dictionary<float, bool> ThresholdCheck(float rating, params float[] thresholds)
        {
            Dictionary<float, bool> thresholdChecks = new Dictionary<float, bool>();
            foreach (var threshold in thresholds)
            {
                if (rating > threshold)
                {
                    thresholdChecks.Add(threshold, true);
                }
                else
                {
                    thresholdChecks.Add(threshold, false);
                }
            }
            return thresholdChecks;
        }

        public bool RangeCheck(float attentionRating, int lower, int upper)
        {
            if (lower <= attentionRating && attentionRating <= upper) return true;
            else return false;
        }

        public void FirstPastThePost ()
        {
            throw new NotImplementedException();
        }

        public void ProportionalRepresentation()
        {
            throw new NotImplementedException();
        }

        public void TugOfWar()
        {
            throw new NotImplementedException();
        }

        public virtual float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}