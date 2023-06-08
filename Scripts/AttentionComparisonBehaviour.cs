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

        public struct LocalTrackerValuesCopy
        {
            public string name { get; set; }
            public float currentAttention { get; set; }
            public float cumulativeAttention { get; set; }
        }

        public struct AttentionComparisonResult
        {
            public string comparisonName { get; set; }
            public float attentionValue { get; set; }
        }

        private List<LocalTrackerValuesCopy> LocalAttentionRecordsList = new List<LocalTrackerValuesCopy>();
        //public LocalTrackerValuesCopy processingAttentionResult = new LocalTrackerValuesCopy();
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
            LocalAttentionRecordsList.Clear(); // probably best clear this each frame before performing update? hmmmmm! not sure!
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
                LocalTrackerValuesCopy localTrackerValues = new LocalTrackerValuesCopy();
                localTrackerValues.name = tracker.name;
                localTrackerValues.currentAttention = currentAttention;
                localTrackerValues.cumulativeAttention = cumulativeAttention;
                LocalAttentionRecordsList.Add(localTrackerValues);
            }
        }

        // Maybe collapse these to calculate results for both cumulative and current, making behaviour handling easier? But does more calculation work as a tradeoff.
        // But then can also play off results of both against each other same as in the non comparison behaviour.
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
            currentAttentionResult.comparisonName = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[0].name;
            currentAttentionResult.attentionValue = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[0].currentAttention;
            cumulativeAttentionResult.comparisonName = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[0].name;
            cumulativeAttentionResult.attentionValue = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[0].cumulativeAttention;
        }

        public void GetMedianAttention()
        {
            int mid = (LocalAttentionRecordsList.Count - 1) / 2;
            currentAttentionResult.comparisonName = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[mid].name;
            currentAttentionResult.attentionValue = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[mid].currentAttention;
            cumulativeAttentionResult.comparisonName = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[mid].name;
            cumulativeAttentionResult.attentionValue = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[mid].cumulativeAttention;
        }

        public void GetLargestAttention()
        {
            currentAttentionResult.comparisonName = LocalAttentionRecordsList.OrderBy(x => x.currentAttention).Reverse().ToList()[0].name;
            currentAttentionResult.attentionValue = LocalAttentionRecordsList.OrderBy(x => x.currentAttention).Reverse().ToList()[0].currentAttention;
            cumulativeAttentionResult.comparisonName = LocalAttentionRecordsList.OrderBy(x => x.cumulativeAttention).Reverse().ToList()[0].name;
            cumulativeAttentionResult.attentionValue = LocalAttentionRecordsList.OrderBy(x => x.cumulativeAttention).Reverse().ToList()[0].cumulativeAttention;
        }

        public void GetCombinedAttention()
        {
            float currentTotal = LocalAttentionRecordsList.Sum(x => x.currentAttention);
            currentAttentionResult.comparisonName = "Total";
            currentAttentionResult.attentionValue = currentTotal;
            float cumulativeTotal = LocalAttentionRecordsList.Sum(x => x.cumulativeAttention);
            cumulativeAttentionResult.comparisonName = "Total";
            cumulativeAttentionResult.attentionValue = cumulativeTotal;
        }

        public void GetProportionalAttention()
        {
            // NOTE THIS IS SUPER HACKY. IT GETS THE FIRST TRACKER IN THE LIST AND WORKS IT OUT AS A PROPORTION OF THE TOTAL
            // IT'S NOT GOOD BUT IT IS A FUDGE FOR A STICKY PROBLEM I CAN'T BE BOTHERED TO SOLVE RIGHT NOW. ANSWERS ON A POSTCARD.
            // Can we save it to a list available in the behaviour instead maybe?

            // Current
            float currentTotal = LocalAttentionRecordsList.Sum(x => x.currentAttention);
            if (currentTotal == 0)
            {
                currentTotal = 1f; // eugh this is hacky, NaN divide by zero workaround? - is there a better way?
            }
            float currentFirstTrackerPecentage = LocalAttentionRecordsList[0].currentAttention / currentTotal; // this returns a 0.0f to 1.0f based value, more useful than 100% based.
            currentAttentionResult.comparisonName = $"Proportional for {LocalAttentionRecordsList[0].name}";
            currentAttentionResult.attentionValue = currentFirstTrackerPecentage;
            // Cumulative
            float cumulativeTotal = LocalAttentionRecordsList.Sum(x => x.cumulativeAttention);
            if (cumulativeTotal == 0)
            {
                cumulativeTotal = 1f; // eugh this is hacky, NaN divide by zero workaround? - is there a better way?
            }
            float cumulativeFirstTrackerPecentage = LocalAttentionRecordsList[0].cumulativeAttention / cumulativeTotal;
            cumulativeAttentionResult.comparisonName = $"Proportional for {LocalAttentionRecordsList[0].name}";
            cumulativeAttentionResult.attentionValue = cumulativeFirstTrackerPecentage;
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
