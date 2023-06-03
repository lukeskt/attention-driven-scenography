using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class ColourChangeCumulative : CumulativeAttentionBehaviour
    {
        public Renderer rend;

        public override void CumulativeAttentionReaction(float? attentionRating)
        {
            Color col1 = Color.green;
            Color col2 = Color.red;
            Color lerpy = Color.Lerp(col1, col2, (float)attentionRating * 0.1f);
            rend.material.SetColor("_BaseColor", lerpy);
        }
    }
}
