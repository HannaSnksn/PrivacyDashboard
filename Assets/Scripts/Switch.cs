using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject elementToDisable;
    [SerializeField] private GameObject elementToEnable;

    public void SwapElements()
    {
        elementToDisable.SetActive(false);
        StartCoroutine(EnableAfterDelay());
    }

    private IEnumerator EnableAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        elementToEnable.SetActive(true);
    }
}