using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionDecay : AttnBehaviourNew
    {
        public float decayRate = 0.5f;

        public override void CurrentAttentionBehaviour(float? attentionRating)
        {
            if(attentionRating <= 0 && AttentionTracker.CumulativeAttention > 0)
            {
                AttentionTracker.CumulativeAttention -= decayRate * Time.deltaTime;
            }
        }
    }
}
