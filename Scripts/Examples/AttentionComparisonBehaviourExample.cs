using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionComparisonBehaviourExample : AttentionComparisonBehaviour
    {
        public override void AttentionComparisonEffect()
        {
            print($"Comparison Mode: {Enum.GetName(comparisonMode.GetType(), comparisonMode)}. " +
                $"Result: {processingAttentionResult.name}, " +
                $"Current Attention {processingAttentionResult.currentAttention}, " +
                $"Cumulative Attention {processingAttentionResult.cumulativeAttention}");
        }
    }
}
