using UnityEngine;
using UnityEngine.UI;

public class MyButtonHandler : MonoBehaviour
{
    [SerializeField] private Button myButton;
    [SerializeField] private UIManager uiManager; // reference to the always-active manager

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
        uiManager.SwapElements();
        LaunchBeatSaber();
    }

    public void LaunchBeatSaber()
    {
        Debug.Log("LaunchBeatSaber called"); // 1. is this even firing?

    #if PLATFORM_ANDROID && !UNITY_EDITOR
        try
        {
            string packageName = "com.beatgames.beatsaber";
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
            AndroidJavaObject launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", packageName);

            if (launchIntent != null)
            {
                currentActivity.Call("startActivity", launchIntent);
            }
            else
            {
                Debug.LogWarning("launchIntent was null — check <queries> manifest entry or confirm Beat Saber is installed");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Exception launching app: " + e); // 3. catches anything else
        }
    #else
            Debug.LogWarning("Not running on Android device — this code path is skipped in Editor");
    #endif
    }
}