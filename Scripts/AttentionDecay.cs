using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionDecay : AttentionBehaviour
    {
        public float decayRate = 0.5f;

        public override void CurrentAttentionBehaviour()//float? currentAttention)
        {
            if(CurrentAttentionRating <= 0 && AttentionTracker.CumulativeAttention > 0)
            {
                AttentionTracker.CumulativeAttention -= decayRate * Time.deltaTime;
            }
        }
    }
}
