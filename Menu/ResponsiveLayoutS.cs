using UnityEngine;
using UnityEngine.UI;

public class ResponsiveLayoutSwitcher : MonoBehaviour
{
    public GameObject mobileLayout;  // Das UI-Layout für mobile Geräte
    public GameObject tabletLayout;  // Das UI-Layout für Tablets

    private void Start()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio <= 1.5f) // Beispielwert für Seitenverhältnis (anpassen)
        {
            mobileLayout.SetActive(true);
            tabletLayout.SetActive(false);
        }
        else
        {
            mobileLayout.SetActive(false);
            tabletLayout.SetActive(true);
        }
    }
}