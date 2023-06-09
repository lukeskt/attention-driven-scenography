
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;
using UnityEngine.Video;

namespace AttentionDrivenScenography
{
    public class CumulativeAttentionBehaviourAudioChorusFilter : AttentionBehaviour
    {
        public AudioChorusFilter audioChorusFilter;
                               
        public override void AttentionEffect()
        {
            audioChorusFilter.depth = Mathf.InverseLerp(0, 1, (float)CurrentAttentionRating);
        }
    }
}
