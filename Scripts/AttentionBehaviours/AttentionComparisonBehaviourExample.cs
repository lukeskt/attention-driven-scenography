using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionComparisonBehaviourExample : AttentionComparisonBehaviour
    {
        public override void AttentionEffect()
        {
            print($"Comparison Mode: {Enum.GetName(comparisonMode.GetType(), comparisonMode)}. " +
                $"Current Result: {currentAttentionResult.comparisonName}, " +
                $"Current Attention {currentAttentionResult.attentionValue}, " +
                $"Cumulative Result: {cumulativeAttentionResult.comparisonName}, " +
                $"Cumulative Attention {cumulativeAttentionResult.attentionValue}");
        }
    }
}
