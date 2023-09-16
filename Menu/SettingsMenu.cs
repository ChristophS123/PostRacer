using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider effectsSlider;
    [SerializeField] private Slider sensitivitySlider;

    void Start()
    {
        LoadSliders();
    }

    private void LoadSliders()
    {
        musicSlider.value = PlayerPrefs.GetInt("music", 50);
        effectsSlider.value = PlayerPrefs.GetInt("effects", 50);
        sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity", 50);
    }

    public void BackToMainMenu()
    {
        PlayerPrefs.SetInt("music", (int)musicSlider.value);
        PlayerPrefs.SetInt("effects", (int)effectsSlider.value);
        PlayerPrefs.SetInt("sensitivity", (int)sensitivitySlider.value);
        SceneManager.LoadScene("MainMenu");
    }

}
