using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Logging;
using Logger = Unity.Logging.Logger;
using Unity.Logging.Sinks;

public class ADSLoggers : MonoBehaviour
{
    Logger attentionTrackersLogger;
    Logger attentionBehavioursLogger;
    Logger attentionDecisionLogger;

    void Awake()
    {
        attentionTrackersLogger = new Logger(new LoggerConfig()
                                        .MinimumLevel.Debug()
                                        .OutputTemplate("{Timestamp} - {Level} - {Message}")
                                        .WriteTo.File("./Logs/AttentionTrackers.log", minLevel: LogLevel.Verbose)
                                        .WriteTo.UnityEditorConsole(outputTemplate: "{Level} || {Timestamp} || {Message}"));

        attentionBehavioursLogger = new Logger(new LoggerConfig()
                                             .MinimumLevel.Debug()
                                             .OutputTemplate("{Timestamp} - {Level} - {Message}")
                                             .WriteTo.File("./Logs/AttentionBehaviours.log", minLevel: LogLevel.Verbose)
                                             .WriteTo.UnityEditorConsole(outputTemplate: "{Level} || {Timestamp} || {Message}"));

        attentionDecisionLogger = new Logger(new LoggerConfig()
                                               .MinimumLevel.Debug()
                                               .OutputTemplate("{Timestamp} - {Level} - {Message}")
                                               .WriteTo.File("./Logs/AttentionDecisions.log", minLevel: LogLevel.Verbose)
                                               .WriteTo.UnityEditorConsole(outputTemplate: "{Level} || {Timestamp} || {Message}"));

        Log.To(attentionTrackersLogger).Info("Testing attentiontrackerslogger!");
        Log.To(attentionBehavioursLogger).Info("Testing attentionBehavioursLogger!");
        Log.To(attentionDecisionLogger).Info("Testing attentionDecisionLogger!");
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
