using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionBehaviour : MonoBehaviour
    {
        // Attention Ratings Sources
        [field: SerializeField] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public AttentionTracker AttentionTracker { get; set; }
        // Current Attention
        private float? currentAttentionRating = null;
        public virtual float? CurrentAttentionRating { get => currentAttentionRating; set => currentAttentionRating = value; }
        // Cumulative Attention
        private float? cumulativeAttentionRating = null;
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        [Tooltip("Calls code in Start, useful for a one-off action based on current attention when an object is activated.")]
        public bool StartCheck;
        [Tooltip("Calls code in Update, useful for continuously change in response to current attention.")]
        public bool UpdateCheck;
        // TODO: Maybe also awake, onenable, on disable, fixedupdate, when called externally, etc?

        [Tooltip("Calls the Current Attention Behaviour if checked.")]
        public bool DoCurrentBehaviour;
        [Tooltip("Calls the Cumulative Attention Behaviour if checked.")]
        public bool DoCumulativeBehaviour;

        void Start()
        {
            // TODO: account for startcheck w/o tracker... then final else? just... better here and in update.
            // MAYBE ALSO EXTRACT ALL THIS TO A FUNC TO CALL ON BOTH REDUCE CODE COPYPASTA
            // Get Attention Values
            if (AttentionTracker != null)
            {
                CurrentAttentionRating = AttentionTracker.CurrentAttention;
                CumulativeAttentionRating = AttentionTracker.CumulativeAttention;
            }
            else
            {
                CumulativeAttentionRating = AttentionDatastore.AttentionTrackingObjects.Find(x => x.name == AttentionTracker.name).cumulativeAttention;
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            // Run Behaviour Code
            if (StartCheck && AttentionTracker != null)
            {
                if (DoCurrentBehaviour) CurrentAttentionBehaviour(CurrentAttentionRating);
                if (DoCumulativeBehaviour) CumulativeAttentionBehaviour(CumulativeAttentionRating);
            }
            else
            {
                //Debug.Log("Tracker not available, can't run current attention behaviour...");
                if (DoCumulativeBehaviour) CumulativeAttentionBehaviour(CumulativeAttentionRating);
            }
        }

        
        void Update()
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
                Debug.Log("Tracker not available, getting from Datastore...");
            }
            // Run Behaviour Code
            if (UpdateCheck && AttentionTracker != null)
            {
                if (DoCurrentBehaviour) CurrentAttentionBehaviour(CurrentAttentionRating);
                if (DoCumulativeBehaviour) CumulativeAttentionBehaviour(CumulativeAttentionRating);
            }
            else
            {
                //Debug.Log("Tracker not available, can't run current attention behaviour...");
                if (DoCumulativeBehaviour) CumulativeAttentionBehaviour(CumulativeAttentionRating);
            }
        }

        public virtual void CurrentAttentionBehaviour (float? currentAttention)
        {
            return;
        }

        public virtual void CumulativeAttentionBehaviour (float? cumulativeAttention)
        {
            return;
        }
    }
}
