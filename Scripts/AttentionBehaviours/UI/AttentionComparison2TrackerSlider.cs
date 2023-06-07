using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AttentionDrivenScenography
{
    public class AttentionComparison2TrackerSlider : AttentionComparisonBehaviour
    {
        public Slider slider;
        public override void AttentionComparisonEffect()
        {
            float val = 0f;
            if (attentionType == AttentionType.Current) val = Mathf.InverseLerp(0, 100, processingAttentionResult.currentAttention);
            else if (attentionType == AttentionType.Cumulative) val = Mathf.InverseLerp(0, 100, processingAttentionResult.cumulativeAttention);
            print(val);
            slider.value = val;
        }
    }
}
