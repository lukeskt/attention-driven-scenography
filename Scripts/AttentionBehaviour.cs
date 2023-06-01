using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        [Tooltip("Calls code in Start, useful for a one-off action based on current attention when an object is activated.")]
        public bool StartCheck;
        [Tooltip("Calls code in Update, useful for continuously change in response to current attention.")]
        public bool UpdateCheck;

        [SerializeField] public AttentionTracker[] attentionTrackers;

        [field: SerializeField] public AttentionTracker PrimaryAttentionTracker { get; set; }
        [field: SerializeField] public AttentionDatastore AttentionDatastore { get; set; }

        private float? currentAttentionRating = null;
        public virtual float? CurrentAttentionRating { get => currentAttentionRating; set => currentAttentionRating = value; }

        private float? cumulativeAttentionRating = null;
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        // Start is called before the first frame update
        void Start()
        {
            // TODO: try get tracker, if not live then go to datastore.
            if (PrimaryAttentionTracker)
            {
                CurrentAttentionRating = PrimaryAttentionTracker.CurrentAttention;
                CumulativeAttentionRating = PrimaryAttentionTracker.CumulativeAttention;
            }
            else
            {
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == PrimaryAttentionTracker.name).cumulativeAttention;
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            if (StartCheck)
            {
                AttentionRatingReaction(CurrentAttentionRating, CumulativeAttentionRating);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (PrimaryAttentionTracker)
            {
                CurrentAttentionRating = PrimaryAttentionTracker.CurrentAttention;
                CumulativeAttentionRating = PrimaryAttentionTracker.CumulativeAttention;
            }
            else
            {
                CurrentAttentionRating = 0f; // if not live object, won't be available.
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == PrimaryAttentionTracker.name).cumulativeAttention;
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            if (UpdateCheck)
            {
                AttentionRatingReaction(CurrentAttentionRating, CumulativeAttentionRating);
            }
        }

        public float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
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

        public string FirstPastThePost(bool first, int lower = 0, int upper = int.MaxValue)
        {
            var trackersCumulative = attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList();
            if (first) trackersCumulative.Reverse();
            if (RangeCheck(trackersCumulative[0].CumulativeAttention, lower, upper)) return trackersCumulative[0].name;
            else return null;
        }

        public void ProportionalRepresentation()
        {
            float totalAttention = 0f;
            foreach (var tracker in attentionTrackers)
            {
                totalAttention += tracker.CumulativeAttention;
            }
            foreach (var tracker in attentionTrackers)
            {
                float trackerPercentage = (tracker.CumulativeAttention / totalAttention) * 100;
                print($"{tracker.name}: {trackerPercentage}%");
            }
        }

        public float TugOfWar(float negRating, float posRating)
        {
            // Trying a version of prop rep stuff here:
            float totalAttention = negRating + posRating;
            float negPercentage = (negRating / totalAttention) * 1.00f;
            float posPercentage = (posRating / totalAttention) * 1.00f;
            float difference = 0f;
            if (negPercentage > posPercentage) difference = negPercentage - posPercentage;
            else difference = posPercentage - negPercentage;
            // want to do something here where the neg pulls it down to 0, the pos pulls it up to 1?
            print($"Negative: {negPercentage}, Positive: {posPercentage}, Difference: {difference}");
            return difference;
        }

        public virtual void AttentionRatingReaction(float? currentAttentionRating, float? cumulativeAttentionRating)
        {
            print($"FPTP Result: {FirstPastThePost(true)}");
            ProportionalRepresentation();
            TugOfWar(attentionTrackers[0].CumulativeAttention, attentionTrackers[1].CumulativeAttention);
        }
    }
}
