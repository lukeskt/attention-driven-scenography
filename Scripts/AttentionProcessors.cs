using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public static class AttentionProcessors
    {
        public enum ProcessorMode
        {
            Total,
            Proportional,
            Largest,
            Median,
            Smallest
        }

        public static ProcessorMode proc = new ProcessorMode();

        public enum AttentionType
        {
            Current,
            Cumulative
        }

        public static AttentionType attnType = new AttentionType();

        // TODO: Figure out how to make this returnable.
        private static Dictionary<string, float> ProportionalAttention (List<AttentionTracker> attentionTrackers, AttentionType attentionType)
        {
            float totalAttention = 0f;
            foreach (var tracker in attentionTrackers)
            {
                if (attentionType == AttentionType.Cumulative) totalAttention += tracker.CumulativeAttention;
                else if (attentionType == AttentionType.Current) totalAttention += tracker.CurrentAttention;
            }
            Dictionary<string, float> proportionsList = new Dictionary<string, float>();
            foreach (var tracker in attentionTrackers)
            {
                float trackerPercentage = 0f;
                if (attentionType == AttentionType.Cumulative) trackerPercentage = (tracker.CumulativeAttention / totalAttention) * 100;
                else if (attentionType == AttentionType.Current) trackerPercentage = (tracker.CurrentAttention / totalAttention) * 100;
                proportionsList.Add(tracker.name, trackerPercentage);
                //print($"{tracker.name}: {trackerPercentage}%");
            }
            return proportionsList;
        }

        private static float TugOfWar(float negRating, float posRating)
        {
            // Trying a version of prop rep stuff here:
            float totalAttention = negRating + posRating;
            float negPercentage = (negRating / totalAttention) * 1.00f;
            float posPercentage = (posRating / totalAttention) * 1.00f;
            float difference = 0f;
            if (negPercentage > posPercentage) difference = negPercentage - posPercentage;
            else difference = posPercentage - negPercentage;
            // want to do something here where the neg pulls it down to 0, the pos pulls it up to 1?
            //print($"Negative: {negPercentage}, Positive: {posPercentage}, Difference: {difference}");
            return difference;
        }
    }
}
