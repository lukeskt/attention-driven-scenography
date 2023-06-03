using GluonGui.Dialog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AttentionDrivenScenography
{
    public abstract class AttentionEffect : MonoBehaviour
    {
        [field: SerializeField] public AttentionDatastore AttentionDatastore { get; set; }
        [field: SerializeField] public List<AttentionTracker> AttentionTrackers { get; set; }
        
        private float? currentAttentionRating = null;
        public virtual float? CurrentAttentionRating { get => currentAttentionRating; set => currentAttentionRating = value; }
        private float? cumulativeAttentionRating = null;
        public virtual float? CumulativeAttentionRating { get => cumulativeAttentionRating; set => cumulativeAttentionRating = value; }

        // Flags
        public bool AwakeCheck;
        public bool StartCheck;
        public bool UpdateCheck;
        public bool FixedUpdateCheck;
        public bool CalledCheck;

        public enum ProcessorMode
        {
            Total,
            Proportional,
            Largest,
            Median,
            Smallest
        }

        public ProcessorMode processor = new ProcessorMode();

        public enum AttentionType
        {
            Current,
            Cumulative
        }

        public AttentionType attentionType = new AttentionType();

        public float[] thresholds;

        private void Awake()
        {
            if (AwakeCheck) AttentionToEffect(processor, attentionType);
        }


        private void Start()
        {

            if (StartCheck) AttentionToEffect(processor, attentionType);
        }

        
        private void Update()
        {
            if (UpdateCheck) AttentionToEffect(processor, attentionType);
        }

        private void FixedUpdate()
        {
            if (FixedUpdateCheck) AttentionToEffect(processor, attentionType);
        }

        // Maybe not a good idea?
        public void CalledExternally()
        {

        }

        // Dataflow Path?
        public virtual void AttentionToEffect(ProcessorMode processor, AttentionType attentionType)
        {
            var value = ProcessorSelector(AttentionTrackers, processor, attentionType);
            print(value);
        }

        // Processors
        private (string, float) ProcessorSelector(List<AttentionTracker> attentionTrackers, ProcessorMode processor, AttentionType attentionType)
        {
            AttentionTracker result;
            switch (processor)
            {
                case ProcessorMode.Total:
                    return AttentionTrackersTotal(attentionTrackers, attentionType);
                case ProcessorMode.Proportional:
                    return default; // ugh this is a special case! TODO: FIGURE THIS OUT
                case ProcessorMode.Largest:
                    if (attentionType == AttentionType.Cumulative)
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).Reverse().ToList()[0];
                        return (result.name, result.CumulativeAttention);
                    }
                    else
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CurrentAttention).Reverse().ToList()[0];
                        return (result.name, result.CurrentAttention);
                    }
                case ProcessorMode.Median:
                    int mid = (attentionTrackers.Count - 1) / 2;
                    if (attentionType == AttentionType.Cumulative)
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[mid];
                        return (result.name, result.CumulativeAttention);
                    }
                    else
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[mid];
                        return (result.name, result.CurrentAttention);
                    }
                case ProcessorMode.Smallest:
                    if (attentionType == AttentionType.Cumulative)
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CumulativeAttention).ToList()[0];
                        return (result.name, result.CumulativeAttention);
                    }
                    else
                    {
                        result = attentionTrackers.ToList().OrderBy(x => x.CurrentAttention).ToList()[0];
                        return (result.name, result.CurrentAttention);
                    }
                default:
                    return ("Error!", 0f);
            }
        }

        private (string, float) AttentionTrackersTotal(List<AttentionTracker> attentionTrackers, AttentionType attentionType)
        {
            float combined = 0f;
            if (attentionType == AttentionType.Cumulative) combined = attentionTrackers.Sum(x => x.CumulativeAttention);
            else if (attentionType == AttentionType.Current) combined = attentionTrackers.Sum(x => x.CurrentAttention);
            return ("Total", combined);
        }

        private Dictionary<string, float> AttentionTrackersProportional (List<AttentionTracker> attentionTrackers, AttentionType attentionType)
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

        // Outputs
        public virtual float MapValue(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
