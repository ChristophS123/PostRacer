using UnityEngine;
using UnityEngine.UI;

public class ToastManager : MonoBehaviour
{
    public Text toastText;
    public GameObject toastPanel;

    private float toastDuration = 3.0f;

    private float currentToastTime = 0.0f;
    private bool isToastShowing = false;

    private void Update()
    {
        if (isToastShowing)
        {
            currentToastTime += Time.deltaTime;
            if (currentToastTime >= toastDuration)
            {
                HideToast();
            }
        }
    }

    public void ShowToast(string message)
    {
        toastText.text = message;
        toastPanel.SetActive(true);
        currentToastTime = 0.0f;
        isToastShowing = true;
    }

    public void HideToast()
    {
        toastPanel.SetActive(false);
        isToastShowing = false;
    }
}
