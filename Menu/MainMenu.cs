using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    const long currentVersion = 1;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;
    private PostRacerAPI postRacerAPI;

    private GameObject loadingScreenInstance;

    private void Start()
    {
        PlayerPrefs.SetInt("adrounds", 0);
        postRacerAPI = new PostRacerAPI();
    }

    public void LoadSettingsMenu()
    {
        SceneManager.LoadScene("SettingsScene");
    }

    public void LoadOfflineMenu()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        SceneManager.LoadScene("OfflineMenu");
    }

    public async void LoadOnlineMenu()
    {
        if(await postRacerAPI.GetVersion() > currentVersion)
        {
            SceneManager.LoadScene("UpdateScene");
            return;
        }
        if(loadingScreenInstance != null)
        {
            return;
        }
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string username = PlayerPrefs.GetString("username", "");
        string token = PlayerPrefs.GetString("token", "");
        if (username == "" || token == "")
        {
            SceneManager.LoadScene("LoginScene");
            return;
        }
        bool response = await postRacerAPI.AutomaticSignIn(token);
        if (response)
        {
            PlayerPrefs.SetInt("news", 1);
            SceneManager.LoadScene("OnlineMenu");
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    /*  
     *  Mehr Shop Items
     */

}
