using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionTextureBlender : AttentionBehaviour
    {
        public Material mat;

        public override void AttentionEffect()
        {
            var blendValue = MapValue((float)CurrentAttentionRating, 0, 1, 1, 10);
            var clampedCumulative = Mathf.Clamp((float)CumulativeAttentionRating, 1, 20);
            mat.SetFloat("_BlendOpacity", clampedCumulative);
        }
    }
}
