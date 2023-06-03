using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class ColourChangeCurrent : CurrentAttentionBehaviour
    {
        public Renderer rend;

        public override void CurrentAttentionReaction(float? attentionRating)
        {
            Color col1 = Color.yellow;
            Color col2 = Color.magenta;
            Color lerpy = Color.Lerp(col1, col2, (float)attentionRating);
            rend.material.SetColor("_BaseColor", lerpy);
        }
    }
}