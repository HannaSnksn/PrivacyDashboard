using UnityEngine;
using Oculus.Interaction;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;

public class PrivacyPolicy : MonoBehaviour
{
    [SerializeField] private Button myButton;

    private string logFilePath;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "interaction_log.txt");
    }

    private void OnEnable()
    {
        myButton.onClick.AddListener(OpenWebsite);
    }

    private void OnDisable()
    {
        myButton.onClick.RemoveListener(OpenWebsite);
    }

    public void OpenWebsite()
    {
        Application.OpenURL("https://www.meta.com/legal/privacy-policy/");
        WriteLog($"Button '{myButton.gameObject.name}' was pressed.");
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
