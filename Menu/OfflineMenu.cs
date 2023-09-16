using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OfflineMenu : MonoBehaviour
{

    [SerializeField] private Text highscoreText;
    [SerializeField] private Text lastscoreText;
    [SerializeField] private Text paketsHighscore;
    [SerializeField] private InterstitialAdsButton interstitialAd;

    private void Start()
    {
        Time.timeScale = 1;
        interstitialAd.LoadAd();
        highscoreText.text = "Highscore: " + PlayerPrefs.GetFloat(StatsField.HIGHSCORE, 0).ToString() + "m";
        lastscoreText.text = "Lastscore: " + PlayerPrefs.GetFloat(StatsField.LASTSCORE, 0).ToString() + "m";
        paketsHighscore.text = "Pakets Highscore: " + PlayerPrefs.GetFloat(StatsField.PAKETHIGHSCORE, 0).ToString();
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("gametype", "offline");
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
