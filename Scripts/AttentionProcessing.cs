using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public class AttentionProcessing
    {
        public enum ListReturnMode
        {
            Largest,
            Median,
            Smallest,
        }

        public static string ResultFromList (List<AttentionTracker> attentionTrackers, ListReturnMode listReturnMode)
        {
            switch (listReturnMode)
            {
                case ListReturnMode.Largest:
                    return attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).Reverse().ToList()[0].name;
                case ListReturnMode.Median:
                    int mid = (attentionTrackers.Count - 1 ) / 2;
                    return attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[mid].name;
                case ListReturnMode.Smallest:
                    return attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[0].name;
                default:
                    return "Error!";
            }
        }

        public static float CombinedAttention (List<AttentionTracker> attentionTrackers)
        {
            float combined = attentionTrackers.Sum(x => x.CumulativeAttention);
            return combined;
        }

        // TODO: Figure out how to make this returnable.
        public static Dictionary<string, float> Proportions (List<AttentionTracker> attentionTrackers)
        {
            float totalAttention = 0f;
            foreach (var tracker in attentionTrackers)
            {
                totalAttention += tracker.CumulativeAttention;
            }
            Dictionary<string, float>  proportionsList = new Dictionary<string, float>();
            foreach (var tracker in attentionTrackers)
            {
                float trackerPercentage = (tracker.CumulativeAttention / totalAttention) * 100;
                proportionsList.Add(tracker.name, trackerPercentage);
                //print($"{tracker.name}: {trackerPercentage}%");
            }
            return proportionsList;
        }

        // TODO: Maybe just replace this with custom thresholds as normal... i.e. if (rating > 100) etc...
        public static Dictionary<float, bool> ThresholdCheck(float rating, params float[] thresholds)
        {
            Dictionary<float, bool> thresholdChecks = new Dictionary<float, bool>();
            foreach (var threshold in thresholds)
            {
                if (rating > threshold)
                {
                    thresholdChecks.Add(threshold, true);
                }
                else
                {
                    thresholdChecks.Add(threshold, false);
                }
            }
            return thresholdChecks;
        }

        public static float TugOfWar(float negRating, float posRating)
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

        public static bool RangeCheck(float attentionRating, int lower, int upper)
        {
            if (lower <= attentionRating && attentionRating <= upper) return true;
            else return false;
        }

        public static float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
