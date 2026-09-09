using UnityEngine;
using System.IO;
using Oculus.Interaction;
using UnityEngine.UI;
using System;

public class VRButtonController : MonoBehaviour
{
    public GameObject details;
    public GameObject details2;
    public GameObject details3;
    public GameObject details4;
    public GameObject details5;
    public GameObject select;

    [SerializeField] private Button myButton;

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
    }

    private void OnButtonPressed()
    {
        if (details != null)
        {
            details.SetActive(!details.activeSelf);
            select.SetActive(!details.activeSelf);
        }
        if (details2 != null)
        {
            details2.SetActive(false);
        }
        if (details3 != null)
        {
            details3.SetActive(false);
        }
        if (details4 != null)
        {
            details4.SetActive(false);
        }
        if (details5 != null)
        {
            details5.SetActive(false);
        }

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
