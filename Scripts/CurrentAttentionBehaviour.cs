using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class CurrentAttentionBehaviour : MonoBehaviour
    {
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        private float? attentionRating = null;
        public virtual float? AttentionRating { get => attentionRating; set => attentionRating = value; }

        [Tooltip("Calls code in Start, useful for a one-off action based on current attention when an object is activated.")]
        public bool StartCheck;
        [Tooltip("Calls code in Update, useful for continuously change in response to current attention.")]
        public bool UpdateCheck;

        void Start()
        {
            AttentionRating = AttentionTracker.CurrentAttention;
            if (StartCheck)
            {
                CurrentAttentionReaction(AttentionRating);
            }
        }

        void Update()
        {
            AttentionRating = AttentionTracker.CurrentAttention;
            if (UpdateCheck)
            {
                CurrentAttentionReaction(AttentionRating);
            }
        }

        public virtual void CurrentAttentionReaction(float? attentionRating)
        {

        }

        public virtual Dictionary<float, bool> ThresholdCheck(float rating, params float[] thresholds)
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

        public virtual float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}