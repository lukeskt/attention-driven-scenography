using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionAudioCrossfade : AttentionBehaviour
    {
        public AudioSource audioSrc1;
        public AudioSource audioSrc2;
        public AudioSource audioSrc3;

        private bool clickOffPlayed;
        private bool clickOnPlayed;

        // Update is called once per frame
        public override void AttentionEffect()
        {
            CrossfadeAudio();
        }

        private void CrossfadeAudio()
        {
            if (CurrentAttentionRating <= 0)
            {
                audioSrc1.volume = 0;
                audioSrc2.volume = 0;
                clickOnPlayed = false;
                if (!clickOffPlayed)
                {
                    audioSrc3.Play();
                    clickOffPlayed = true;
                }
            }
            else
            {
                clickOffPlayed = false;
                if (!clickOnPlayed)
                {
                    audioSrc3.Play();
                    clickOnPlayed = true;
                }
                audioSrc1.volume = Mathf.InverseLerp(1, 0, (float)CurrentAttentionRating);
                audioSrc2.volume = Mathf.InverseLerp(0, 1, (float)CurrentAttentionRating);
            }
        }
    }
}
