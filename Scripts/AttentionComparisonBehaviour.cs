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

        private List<LocalTrackerValuesCopy> LocalAttentionRecordsList = new List<LocalTrackerValuesCopy>();
        public LocalTrackerValuesCopy processingAttentionResult = new LocalTrackerValuesCopy();

        public enum Comparisons
        {
            Total,
            Proportional,
            Largest,
            Median,
            Smallest
        }

        public enum AttentionType
        {
            Current,
            Cumulative
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
        public AttentionType attentionType = new AttentionType();
        public Comparisons comparisonMode = new Comparisons();

        public Color gizmoColor = Color.magenta;

        void Awake()
        {
            AttentionDatastore = FindObjectOfType<AttentionDatastore>();
            if (!AttentionDatastore) Debug.LogWarning("Attention Datastore not found in scene, please add to avoid issues with cumulative attention behaviours.");
            if (eventChecks == EventChecks.AwakeCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode, attentionType); AttentionComparisonEffect(); }
        }

        void OnEnable() { if (eventChecks == EventChecks.OnEnableCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode, attentionType); AttentionComparisonEffect(); } }
        void Start() { if (eventChecks == EventChecks.StartCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode, attentionType); AttentionComparisonEffect(); } }
        void FixedUpdate() { if (eventChecks == EventChecks.FixedUpdateCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode, attentionType); AttentionComparisonEffect(); } }
        void Update() { 
            if (eventChecks == EventChecks.UpdateCheck) { 
                GetAttentionValues(); 
                ProcessAttentionValues(comparisonMode, attentionType); 
                AttentionComparisonEffect(); 
            }
        }
        void OnDisable() { if (eventChecks == EventChecks.OnDisableCheck) { GetAttentionValues(); ProcessAttentionValues(comparisonMode, attentionType); AttentionComparisonEffect(); } }

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
        private void ProcessAttentionValues(Comparisons comparisonMode, AttentionType attentionType)
        {
            switch (comparisonMode)
            {
                case Comparisons.Total:
                    GetCombinedAttention(attentionType);
                    return;
                case Comparisons.Proportional: // TODO: FIGURE OUT THIS SPECIAL CASE!
                    GetProportionalAttention(attentionType);
                    return;
                case Comparisons.Largest:
                    GetLargestAttention(attentionType);
                    return;
                case Comparisons.Median:
                    GetMedianAttention(attentionType);
                    return;
                case Comparisons.Smallest:
                    GetSmallestAttention(attentionType);
                    return;
            }
        }

        public void GetSmallestAttention(AttentionType attentionType)
        {
            if (attentionType == AttentionType.Current)
            {
                processingAttentionResult = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[0];
            }
            else if (attentionType == AttentionType.Cumulative)
            {
                processingAttentionResult = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[0];
            }
        }

        public void GetMedianAttention(AttentionType attentionType)
        {
            int mid = (LocalAttentionRecordsList.Count - 1) / 2;
            if (attentionType == AttentionType.Current)
            {
                processingAttentionResult = LocalAttentionRecordsList.ToList().OrderBy(x => x.currentAttention).ToList()[mid];
            }
            else if (attentionType == AttentionType.Cumulative)
            {
                processingAttentionResult = LocalAttentionRecordsList.ToList().OrderBy(x => x.cumulativeAttention).ToList()[mid];
            }
        }

        public void GetLargestAttention(AttentionType attentionType)
        {
            if (attentionType == AttentionType.Current)
            {
                processingAttentionResult = LocalAttentionRecordsList.OrderBy(x => x.currentAttention).Reverse().ToList()[0];
            }
            else if (attentionType == AttentionType.Cumulative)
            {
                processingAttentionResult = LocalAttentionRecordsList.OrderBy(x => x.cumulativeAttention).Reverse().ToList()[0];
            }
        }

        public void GetCombinedAttention(AttentionType attentionType)
        {
            if (attentionType == AttentionType.Current)
            {
                float total = LocalAttentionRecordsList.Sum(x => x.currentAttention);
                processingAttentionResult.name = "Total";
                processingAttentionResult.currentAttention = total;
                processingAttentionResult.cumulativeAttention = 0f;
            }
            else if (attentionType == AttentionType.Cumulative)
            {
                float total = LocalAttentionRecordsList.Sum(x => x.cumulativeAttention);
                processingAttentionResult.name = "Total";
                processingAttentionResult.currentAttention = 0f;
                processingAttentionResult.cumulativeAttention = total;
            }
        }

        public void GetProportionalAttention(AttentionType attentionType)
        {
            // NOTE THIS IS SUPER HACKY. IT GETS THE FIRST TRACKER IN THE LIST AND WORKS IT OUT AS A PROPORTION OF THE TOTAL
            // IT'S NOT GOOD BUT IT IS A FUDGE FOR A STICKY PROBLEM I CAN'T BE BOTHERED TO SOLVE RIGHT NOW. ANSWERS ON A POSTCARD.
            // Can we save it to a list available in the behaviour instead maybe?
            if (attentionType == AttentionType.Current)
            {
                float total = LocalAttentionRecordsList.Sum(x => x.currentAttention);
                if (total == 0)
                {
                    total = 1f; // eugh this is hacky, NaN divide by zero workaround? - is there a better way?
                }
                float firstTrackerPecentage = LocalAttentionRecordsList[0].currentAttention / total; // this returns a 0.0f to 1.0f based value, more useful than 100% based.
                // Pop into the results.
                processingAttentionResult.name = $"Proportional for {LocalAttentionRecordsList[0].name}";
                processingAttentionResult.currentAttention = firstTrackerPecentage;
                processingAttentionResult.cumulativeAttention = 0f;
            }
            else if (attentionType == AttentionType.Cumulative)
            {
                float total = LocalAttentionRecordsList.Sum(x => x.cumulativeAttention);
                float firstTrackerPecentage = LocalAttentionRecordsList[0].cumulativeAttention / total;
                // Pop into the results.
                processingAttentionResult.name = $"Proportional for {LocalAttentionRecordsList[0].name}";
                processingAttentionResult.currentAttention = 0f;
                processingAttentionResult.cumulativeAttention = firstTrackerPecentage;
            }
        }

        public float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        public abstract void AttentionComparisonEffect(); // Override this to create bespoke effects in subclass.

        private void OnDrawGizmos()
        {
            foreach (AttentionTracker tracker in AttentionTrackers)
            {
                if (tracker.gameObject != gameObject)
                {
                    Gizmos.color = gizmoColor;
                    Gizmos.DrawLine(transform.position, tracker.transform.position);
                }
            }

        }

        //public void ShowTrackerConnection()
        //{
        //    foreach (AttentionTracker tracker in AttentionTrackers)
        //    {
        //        if (tracker.gameObject != gameObject)
        //        {
        //            gameObject.AddComponent<LineRenderer>();
        //            LineRenderer connection = GetComponent<LineRenderer>();
        //            connection.material = lineMaterial;
        //            connection.startWidth = 0.025f;
        //            connection.endWidth = 0.025f;
        //            connection.SetPosition(0, tracker.transform.position);
        //            connection.startColor = Color.magenta;
        //            connection.SetPosition(1, transform.position);
        //            connection.endColor = Color.magenta;
        //        }
        //    }

        //}
    }
}
