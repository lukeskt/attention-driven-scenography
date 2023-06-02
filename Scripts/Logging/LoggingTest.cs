using UnityEngine;
using Unity.Logging;

public class UserLogger : MonoBehaviour
{
    void Awake()
    {
        Log.Info("Hello, {username}!", "World");
    }
}