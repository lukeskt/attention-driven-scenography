using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionComparisonBehaviour : MonoBehaviour
    {
        // Attention Ratings Sources
        [field: SerializeField, HideInInspector] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public List<AttentionTracker> AttentionTrackers { get; set; }

        public struct AttentionComparisonResult
        {
            public string comparisonName { get; set; }
            public float attentionValue { get; set; }
        }
        public AttentionComparisonResult currentAttentionResult = new AttentionComparisonResult();
        public AttentionComparisonResult cumulativeAttentionResult = new AttentionComparisonResult();

        public enum Comparisons
        {
            Total,
            Proportional,
            Largest,
            Median,
            Smallest
        }

        // Event Flags
        [Flags]
        public enum EventChecks
        {
            None = 0,
            AwakeCheck = 1,
            OnEnableCheck = 2,
            StartCheck = 4,
            FixedUpdateCheck = 8,
            UpdateCheck = 16,
            OnDisableCheck = 32
        }
        public EventChecks eventChecks = EventChecks.None;
        // Modes
        public Comparisons comparisonMode = new Comparisons();

        public Color trackerLineColor = Color.magenta;

        void Awake()
        {
            AttentionDatastore = FindObjectOfType<AttentionDatastore>();
            if (!AttentionDatastore) Debug.LogWarning("Attention Datastore not found in scene, please add to avoid issues with cumulative attention behaviours.");
            if (eventChecks == EventChecks.AwakeCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode); AttentionEffect(); }
        }

        void OnEnable() { if (eventChecks == EventChecks.OnEnableCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode); AttentionEffect(); } }
        void Start() { if (eventChecks == EventChecks.StartCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode); AttentionEffect(); } }
        void FixedUpdate() { if (eventChecks == EventChecks.FixedUpdateCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode); AttentionEffect(); } }
        void Update() { 
            if (eventChecks == EventChecks.UpdateCheck) { 
                GetAttentionValues(); 
                ProcessAttentionValues(comparisonMode); 
                AttentionEffect(); 
            }
        }
        void OnDisable() { if (eventChecks == EventChecks.OnDisableCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode); AttentionEffect(); } }

        private void GetAttentionValues()
        {
            foreach (var tracker in AttentionTrackers)
            {
                float currentAttention = 0f;
                float cumulativeAttention = 0f;
                if (tracker == null)
                {
                    cumulativeAttention = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == tracker.name).cumulativeAttention;
                }
                else
                {
                    currentAttention = tracker.CurrentAttention;
                    cumulativeAttention = tracker.CumulativeAttention;
                }
            }
        }

        private void ProcessAttentionValues(Comparisons comparisonMode)
        {
            switch (comparisonMode)
            {
                case Comparisons.Total:
                    GetCombinedAttention();
                    return;
                case Comparisons.Proportional:
                    GetProportionalAttention();
                    return;
                case Comparisons.Largest:
                    GetLargestAttention();
                    return;
                case Comparisons.Median:
                    GetMedianAttention();
                    return;
                case Comparisons.Smallest:
                    GetSmallestAttention();
                    return;
            }
        }

        public void GetSmallestAttention()
        {
            currentAttentionResult.comparisonName = AttentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[0].name;
            currentAttentionResult.attentionValue = AttentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[0].CurrentAttention;
            cumulativeAttentionResult.comparisonName = AttentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[0].name;
            cumulativeAttentionResult.attentionValue = AttentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[0].CumulativeAttention;
        }

        public void GetMedianAttention()
        {
            int mid = (AttentionTrackers.Count - 1) / 2;
            currentAttentionResult.comparisonName = AttentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[mid].name;
            currentAttentionResult.attentionValue = AttentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[mid].CurrentAttention;
            cumulativeAttentionResult.comparisonName = AttentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[mid].name;
            cumulativeAttentionResult.attentionValue = AttentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[mid].CumulativeAttention;
        }

        public void GetLargestAttention()
        {
            currentAttentionResult.comparisonName = AttentionTrackers.OrderBy(x => x.CurrentAttention).Reverse().ToList()[0].name;
            currentAttentionResult.attentionValue = AttentionTrackers.OrderBy(x => x.CurrentAttention).Reverse().ToList()[0].CurrentAttention;
            cumulativeAttentionResult.comparisonName = AttentionTrackers.OrderBy(x => x.CumulativeAttention).Reverse().ToList()[0].name;
            cumulativeAttentionResult.attentionValue = AttentionTrackers.OrderBy(x => x.CumulativeAttention).Reverse().ToList()[0].CumulativeAttention;
        }

        public void GetCombinedAttention()
        {
            float currentTotal = AttentionTrackers.Sum(x => x.CurrentAttention);
            currentAttentionResult.comparisonName = "Current Attention Total";
            currentAttentionResult.attentionValue = currentTotal;
            float cumulativeTotal = AttentionTrackers.Sum(x => x.CumulativeAttention);
            cumulativeAttentionResult.comparisonName = "Cumulative Attention Total";
            cumulativeAttentionResult.attentionValue = cumulativeTotal;
        }

        public void GetProportionalAttention()
        {
            // NOTE THIS IS SUPER HACKY. IT GETS THE FIRST TRACKER IN THE LIST AND WORKS IT OUT AS A PROPORTION OF THE TOTAL
            // IT'S NOT GOOD BUT IT IS A FUDGE FOR A STICKY PROBLEM I CAN'T BE BOTHERED TO SOLVE RIGHT NOW. ANSWERS ON A POSTCARD.
            // Can we save it to a list available in the behaviour instead maybe?
            // Current
            float currentTotal = AttentionTrackers.Sum(x => x.CurrentAttention);
            if (currentTotal == 0)
            {
                currentAttentionResult.attentionValue = 0f; // eugh this is hacky, NaN divide by zero workaround? - is there a better way?
            } 
            else
            {
                float currentFirstTrackerPecentage = AttentionTrackers[0].CurrentAttention / currentTotal; // this returns a 0.0f to 1.0f based value, more useful than 100% based.
                currentAttentionResult.comparisonName = $"Proportional for {AttentionTrackers[0].name}";
                currentAttentionResult.attentionValue = currentFirstTrackerPecentage;
            }
            // Cumulative
            float cumulativeTotal = AttentionTrackers.Sum(x => x.CumulativeAttention);
            if (cumulativeTotal == 0)
            {
                cumulativeAttentionResult.attentionValue = 0f; // eugh this is hacky, NaN divide by zero workaround? - is there a better way?
            }
            else
            {
                float cumulativeFirstTrackerPecentage = AttentionTrackers[0].CumulativeAttention / cumulativeTotal;
                cumulativeAttentionResult.comparisonName = $"Proportional for {AttentionTrackers[0].name}";
                cumulativeAttentionResult.attentionValue = cumulativeFirstTrackerPecentage;
            }
        }

        public float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        public abstract void AttentionEffect(); // Override this to create bespoke effects in subclass.

        private void OnDrawGizmos()
        {
            foreach (AttentionTracker tracker in AttentionTrackers)
            {
                if (tracker.gameObject != gameObject)
                {
                    Gizmos.color = trackerLineColor;
                    Gizmos.DrawLine(transform.position, tracker.transform.position);
                }
            }

        }
    }
}
