using UnityEngine;
using UnityEngine.UI;

public class ResponsiveLayoutSwitcher : MonoBehaviour
{
    public GameObject mobileLayout;  // Das UI-Layout f�r mobile Ger�te
    public GameObject tabletLayout;  // Das UI-Layout f�r Tablets

    private void Start()
    {
        float aspectRatio = (float)Screen.width / Screen.height;

        if (aspectRatio <= 1.5f) // Beispielwert f�r Seitenverh�ltnis (anpassen)
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