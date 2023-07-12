using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Logging;
using Logger = Unity.Logging.Logger;
using Unity.Logging.Sinks;

namespace ADSLogging
{
    public class ADSLogger : MonoBehaviour
    {
        public static ADSLogger loggerInstance;
        public static Logger ADSLogging;
        [HideInInspector] public string logSessionID;

        void Awake()
        {
            // Singleton
            if (loggerInstance && this != loggerInstance)
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
                loggerInstance = this;
            }
        }
        private void Start()
        {
            // Create Logger
            ADSLogging = new Logger(new LoggerConfig()
                                  .MinimumLevel.Debug()
                                  .SyncMode.FullAsync()
                                  .OutputTemplate("{Timestamp} - {Level} - {Message}")
                                  .WriteTo.File($"{Application.dataPath}/Logs/ADSLog-{logSessionID}.log", minLevel: LogLevel.Verbose)
                                  .WriteTo.UnityEditorConsole(outputTemplate: "{Level} || {Timestamp} || {Message}"));

            Log.To(ADSLogging).Info("ADS Logger is active");
            Log.To(ADSLogging).Info($"Running Session {logSessionID}");
        }
    }
}
