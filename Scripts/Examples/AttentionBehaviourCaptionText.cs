using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionBehaviourCaptionText : AttentionBehaviour
    {
        public TMP_Text captionText;

        // Start is called before the first frame update
        void Start()
        {
            captionText.text = "";
        }

        // Update is called once per frame
        public override void AttentionEffect()
        {
            if (CurrentAttentionRating > 0.80)
            {
                captionText.text = "- I couldn't believe this was happening.";
            }
            else if (CurrentAttentionRating > 0.50)
            {
                captionText.text = "- Each breath I took became shorter.";
            }
            else if (CurrentAttentionRating > 0.25)
            {
                captionText.text = "- Something out of the corner of my eye...";
            }
            else if (CurrentAttentionRating < 0.01)
            {
                captionText.text = "";
            }
        }
    }
}
