using UnityEngine;
using Unity.Logging;
using AttentionDrivenScenography;

public class CurrentAttentionLog : MonoBehaviour
{
    AttentionTracker tracker;
    // Start is called before the first frame update
    void Start()
    {
        tracker = GetComponent<AttentionTracker>();       
    }

    // Update is called once per frame
    void Update()
    {
        Log.Info($"Object: {name}, Current Attention: {tracker.CurrentAttention}");
    }
}
