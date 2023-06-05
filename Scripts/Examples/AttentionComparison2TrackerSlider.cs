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
            slider.value = MapValue(processingAttentionResult.cumulativeAttention, 0, AttentionTrackers.Sum(x => x.CumulativeAttention), 0, 1);
        }
    }
}
