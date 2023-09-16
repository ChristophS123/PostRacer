using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System;

public class OnlineMenu : MonoBehaviour
{

    [SerializeField] private Text highscoreText;
    [SerializeField] private Text lastscoreText;
    [SerializeField] private Text paketshighscoreText;
    [SerializeField] private Text coinsText;

    [SerializeField] private RewardedAdsButton rewardedAds;
    [SerializeField] private InterstitialAdsButton interstitialAd;

    [SerializeField] private GameObject newsObject;
    [SerializeField] private Transform newsParent;

    private PostRacerAPI postRacerAPI;

    private void Start()
    {
        Time.timeScale = 1;
        postRacerAPI = new PostRacerAPI();
        if(PlayerPrefs.GetInt("news", 0) == 1)
            LoadNews();
        LoadPlayerStats();
        interstitialAd.LoadAd();
        rewardedAds.LoadAd();
    }

    private async void LoadNews()
    {
        string message = await postRacerAPI.LoadLatestNews();
        if (message == "")
            return;
        PlayerPrefs.SetInt("news", 0);
        Instantiate(newsObject, newsParent);
        FindObjectOfType<NewsText>().SetText(message);
    }

    private async void LoadPlayerStats()
    {
        highscoreText.text = "Loading...";
        paketshighscoreText.text = "Loading...";
        lastscoreText.text = "Loading...";
        coinsText.text = "Loading...";
        float highscore = await postRacerAPI.GetHighscore(PlayerPrefs.GetString("username"));
        float paketshighscore = await postRacerAPI.GetHighscorePakets(PlayerPrefs.GetString("username"));
        float lastScore = await postRacerAPI.GetLastscore(PlayerPrefs.GetString("username"));
        float coins = await postRacerAPI.GetCoins(PlayerPrefs.GetString("username"));
        highscoreText.text = "Highscore: " + highscore + "m";
        paketshighscoreText.text = "Paket Highscore: " + paketshighscore;
        lastscoreText.text = "LastScore: " + lastScore + "m";
        coinsText.text = coins.ToString();
    }

    public void StartGame()
    {
        PlayerPrefs.SetString("gametype", "online");
        SceneManager.LoadScene("GameScene");
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void OpenTopList()
    {
        SceneManager.LoadScene("TopListHighscore");
    }

    public void LoadShop()
    {
        SceneManager.LoadScene("HatShop");
    }

    public void LogOut()
    {
        PlayerPrefs.SetString("username", "");
        PlayerPrefs.SetString("token", "");
        PlayerPrefs.Save();
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadClanMenu()
    {
        SceneManager.LoadScene("MainClanScene");
    }

    public async void ReloadCoins()
    {
        coinsText.text = "Loading...";
        float coins = await postRacerAPI.GetCoins(PlayerPrefs.GetString("username"));
        coinsText.text = coins.ToString();
    }
}
