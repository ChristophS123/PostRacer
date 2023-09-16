using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerScores : MonoBehaviour
{

    [SerializeField] private Text nameInput;
    [SerializeField] private Text highscoreText;
    [SerializeField] private Text paketsText;

    private PostRacerAPI postRacerAPI;

    private void Start()
    {
        postRacerAPI = new PostRacerAPI();
    }

    public async void LoadPlayerValues()
    {
        string username = nameInput.text;
        highscoreText.text = "Loading...";
        paketsText.text = "Loading...";
        float highscore = await postRacerAPI.GetHighscore(username);
        float highscoreValue = await postRacerAPI.GetPlayerTopListScore(username);
        float paketsHighscore = await postRacerAPI.GetHighscorePakets(username);
        float paketsHighscoreValue = await postRacerAPI.GetPlayerPaketsTopListScore(username);
        highscoreText.text = $"Number: {highscoreValue}; Value: {highscore}";
        paketsText.text = $"Number: {paketsHighscoreValue}; Value: {paketsHighscore}";
    }

    public void Back()
    {
        SceneManager.LoadScene("TopListHighscore");
    }

}
