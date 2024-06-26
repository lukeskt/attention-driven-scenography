using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionBehaviourExample : AttentionBehaviour
    {
        // For AttentionBehaviour derived classes make sure you set public override on Upate,
        // and first call base.Update(); in it to get current attention and cumulative attention values.
        // You can then reference these as AttentionRating and CumulativeAttentionRating respectively.
        public override void AttentionEffect()
        {
            float[] threshes = { 1.0f, 0.9f, 0.8f, 0.7f, 0.6f, 0.5f, 0.4f, 0.3f, 0.2f, 0.1f, 0.05f, 0f };
            foreach (var thresh in threshes)
            {
                if (CurrentAttentionRating > thresh)
                {
                    print($"Attention Rating {CurrentAttentionRating} is higher than {thresh}!");
                    break;
                }
                else if (CurrentAttentionRating <= 0f)
                {
                    print($"Attention Rating {CurrentAttentionRating} is 0 or less!");
                    break;
                }
            }
            print($"Cumulative Attention is currently at: {CumulativeAttentionRating}");
        }
    }
}
