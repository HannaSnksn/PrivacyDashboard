using UnityEngine;
using System;
using System.IO;
using System.Text;

public class GazeLogger : MonoBehaviour
{
    [SerializeField] private OVREyeGaze eyeGaze;
    [SerializeField] private float maxRayDistance = 10f;
    [SerializeField] private LayerMask hitLayers = ~0;
    [SerializeField] private float logIntervalSeconds = 0.1f; // 10 samples/sec

    private string logFilePath;
    private float timeSinceLastLog;
    private StringBuilder buffer = new StringBuilder();
    private int samplesSinceFlush;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "gaze_log.csv");
        File.WriteAllText(logFilePath, "timestamp,pos_x,pos_y,pos_z,hit_object\n"); // header, fresh file per session
    }

    private void Update()
    {
        if (eyeGaze == null || !eyeGaze.EyeTrackingEnabled)
            return;

        timeSinceLastLog += Time.deltaTime;
        if (timeSinceLastLog < logIntervalSeconds)
            return;

        timeSinceLastLog = 0f;

        if (Physics.Raycast(eyeGaze.transform.position, eyeGaze.transform.forward, out RaycastHit hit, maxRayDistance, hitLayers))
        {
            LogSample(hit.point, hit.collider.gameObject.name);
        }
    }

    private void LogSample(Vector3 worldPoint, string hitObjectName)
    {
        buffer.AppendLine($"{Time.time:F3},{worldPoint.x:F4},{worldPoint.y:F4},{worldPoint.z:F4},{hitObjectName}");
        samplesSinceFlush++;

        // Flush periodically instead of every single sample, to reduce disk I/O
        if (samplesSinceFlush >= 20)
        {
            FlushBuffer();
        }
    }

    private void FlushBuffer()
    {
        if (buffer.Length == 0) return;

        try
        {
            File.AppendAllText(logFilePath, buffer.ToString());
            buffer.Clear();
            samplesSinceFlush = 0;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write gaze log: {e.Message}");
        }
    }

    private void OnDisable()
    {
        FlushBuffer(); // make sure nothing's lost when the app closes/pauses
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause) FlushBuffer(); // Quest apps get paused, not always cleanly closed
    }
}