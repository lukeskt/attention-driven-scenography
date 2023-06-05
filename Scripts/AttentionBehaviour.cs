using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        // Attention Ratings Sources
        [field: SerializeField, HideInInspector] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        // Current Attention
        private float? currentAttentionRating = null;
        public virtual float? CurrentAttentionRating { get => currentAttentionRating; set => currentAttentionRating = value; }
        // Cumulative Attention
        private float? cumulativeAttentionRating = null;
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        // Flags
        // TODO: Maybe also onenable, ondisable, fixedupdate, when called externally, etc?
        [Tooltip("Calls code on Awake.")]
        public bool AwakeCheck;
        [Tooltip("Calls code on OnEnable.")]
        public bool OnEnableCheck;
        [Tooltip("Calls code in Start, useful for a one-off action based on current attention when an object is activated.")]
        public bool StartCheck;
        [Tooltip("Calls code on FixedUpdated.")]
        public bool FixedUpdateCheck;
        [Tooltip("Calls code in Update, useful for continuously change in response to current attention.")]
        public bool UpdateCheck;
        [Tooltip("Calls code on OnDisable.")]
        public bool OnDisableCheck;

        void Awake() {
            try { AttentionDatastore = FindObjectOfType<AttentionDatastore>(); }
            catch (System.NullReferenceException) { Debug.LogWarning("Attention Datastore not found in scene, please add."); }
            if (AwakeCheck) { GetAttentionValues(); AttentionEffect(); }
        }

        void OnEnable() { if (OnEnableCheck) { GetAttentionValues(); AttentionEffect(); } }
        void Start() { if (StartCheck) { GetAttentionValues(); AttentionEffect(); } }
        void FixedUpdate() { if (FixedUpdateCheck) { GetAttentionValues(); AttentionEffect(); } }
        void Update() { if (UpdateCheck) { GetAttentionValues(); AttentionEffect(); } }
        void OnDisable() { if (OnDisableCheck) { GetAttentionValues(); AttentionEffect(); } }

        private void GetAttentionValues()
        {
            // Get Attention Values
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

        public virtual void AttentionEffect ()
        {
            return;
        }
    }
}
