using UnityEngine;
using System.IO;
using Oculus.Interaction;
using UnityEngine.UI;
using System;

public class VRButtonLog : MonoBehaviour
{
    [SerializeField] private Button myButton;
    //[SerializeField] private Image color;

    private string logFilePath;

    private void Awake()
    {
        logFilePath = Path.Combine(Application.persistentDataPath, "interaction_log.txt");
    }

    private void OnEnable()
    {
        myButton.onClick.AddListener(OnButtonPressed);
    }

    private void OnDisable()
    {
        myButton.onClick.RemoveListener(OnButtonPressed);
        //color.Color = "866C6F";
    }

    private void OnButtonPressed()
    {
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
