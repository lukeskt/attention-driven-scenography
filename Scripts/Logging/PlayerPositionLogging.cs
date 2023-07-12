using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Logging;
using ADSLogging;

public class PlayerPositionLogging : MonoBehaviour
{
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        InvokeRepeating("LogPlayerPosition", 2, 2);
    }

    private void OnEnable()
    {
        InvokeRepeating("LogPlayerPosition", 2, 2);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    void LogPlayerPosition()
    {
        Log.To(ADSLogger.ADSLogging).Info($"Player Position: {player.transform.position}, Player Rotation: {player.transform.rotation}");
    }
}
