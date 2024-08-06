using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AttentionDrivenScenography
{
    public class AttentionComparison2TrackerSlider : AttentionComparisonBehaviour
    {
        public bool currentAttention = true;
        public Slider slider;
        public override void AttentionEffect()
        {
            float val;
            if (currentAttention) val = currentAttentionResult.attentionValue;
            else val = Mathf.InverseLerp(0, 1, cumulativeAttentionResult.attentionValue);
            print(val);
            slider.value = val;
        }
    }
}
