using UnityEngine;
using System.IO;
using Oculus.Interaction;
using UnityEngine.UI;
using System;

public class VRToggleLog : MonoBehaviour
{
    [SerializeField] private Toggle myToggle;

    private string logFilePath;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "interaction_log.txt");
    }

    private void OnEnable()
    {
        myToggle.onValueChanged.AddListener(OnToggleSwitched);
    }

    private void OnDisable()
    {
        myToggle.onValueChanged.RemoveListener(OnToggleSwitched);
    }

    private void OnToggleSwitched(bool isOn)
    {
        WriteLog($"Toggle '{myToggle.gameObject.name}' changed — now {(isOn ? "ON" : "OFF")}.");
    }

    private void WriteLog(string message)
    {
        string timestampedMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";
        Debug.Log(timestampedMessage); // still useful for Editor/dev builds

        try
        {
            File.AppendAllText(logFilePath, timestampedMessage + Environment.NewLine);
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write log: {e.Message}");
        }
    }
}