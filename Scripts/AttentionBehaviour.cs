using System;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        // Attention Ratings Sources
        [field: SerializeField, HideInInspector] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        // Current Attention
        private float currentAttentionRating = 0f;
        public virtual float CurrentAttentionRating { get => currentAttentionRating; set => currentAttentionRating = value; }
        // Cumulative Attention
        private float cumulativeAttentionRating = 0f;
        public virtual float CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

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

        public Color trackerLineColor = Color.magenta;

        void Awake() {
            AttentionDatastore = FindObjectOfType<AttentionDatastore>();
            if (!AttentionDatastore) Debug.LogWarning("Attention Datastore not found in scene, please add to avoid issues with cumulative attention behaviours.");
            if (eventChecks == EventChecks.AwakeCheck) { GetAttentionValues(); AttentionEffect(); }
        }

        void OnEnable() { if (eventChecks == EventChecks.OnEnableCheck) { GetAttentionValues(); AttentionEffect(); } }
        void Start() { if (eventChecks == EventChecks.StartCheck) { GetAttentionValues(); AttentionEffect(); } }
        void FixedUpdate() { if (eventChecks == EventChecks.FixedUpdateCheck) { GetAttentionValues(); AttentionEffect(); } }
        void Update() { 
            if (eventChecks == EventChecks.UpdateCheck) { 
                GetAttentionValues(); 
                AttentionEffect(); 
            }
        }
        void OnDisable() { if (eventChecks == EventChecks.OnDisableCheck) { GetAttentionValues(); AttentionEffect(); } }

        private void GetAttentionValues()
        {
            if (AttentionTracker != null)
            {
                CurrentAttentionRating = AttentionTracker.CurrentAttention;
                CumulativeAttentionRating = AttentionTracker.CumulativeAttention;
            }
            else
            {
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == AttentionTracker.name).cumulativeAttention;
                Debug.LogWarning("Tracker not active, no current attention value available, getting cumulative attention value from Datastore...");
            }
        }

        public float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }

        public abstract void AttentionEffect(); // Override this to create bespoke effects in subclass.

        private void OnDrawGizmos()
        {
            if(AttentionTracker.gameObject != gameObject)
            {
                Gizmos.color = trackerLineColor;
                Gizmos.DrawLine(transform.position, AttentionTracker.transform.position);
            }
        }
    }
}
