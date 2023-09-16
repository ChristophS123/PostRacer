using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PaketTopList : MonoBehaviour
{

    [SerializeField] private Text top1;
    [SerializeField] private Text top2;
    [SerializeField] private Text top3;
    [SerializeField] private Text top4;
    [SerializeField] private Text top5;
    [SerializeField] private Text top6;
    [SerializeField] private Text top7;
    [SerializeField] private Text top8;
    [SerializeField] private Text top9;
    [SerializeField] private Text top10;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;

    private PostRacerAPI postRacerAPI;

    private async void Start()
    {
        List<Text> texts = new List<Text>();
        texts.Add(top1);
        texts.Add(top2);
        texts.Add(top3);
        texts.Add(top4);
        texts.Add(top5);
        texts.Add(top6);
        texts.Add(top7);
        texts.Add(top8);
        texts.Add(top9);
        texts.Add(top10);
        postRacerAPI = new PostRacerAPI();
        GameObject load = Instantiate(loadingScreen, loadingParent);
        await postRacerAPI.LoadPaketTopList(texts);
        Destroy(load);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("OnlineMenu");
    }

    public void LoadPaketTopList()
    {
        SceneManager.LoadScene("TopListHighscore");
    }

    public void OpenPlayerStatsFinder()
    {
        SceneManager.LoadScene("SpecificHighscoreScene");
    }

}
