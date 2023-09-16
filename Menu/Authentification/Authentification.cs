using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Authentification : MonoBehaviour
{

    [SerializeField] private Text usernameInput;
    [SerializeField] private Text passwordInput;
    [SerializeField] private Text consoleText;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;

    private PostRacerAPI postRacerAPI;

    private GameObject loadingScreenInstance;

    private void Start()
    {
        postRacerAPI = new PostRacerAPI();
    }

    public async void SignUp()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string response = await postRacerAPI.SignUp(usernameInput.text, passwordInput.text);
        if(response == "")
        {
            SaveUserDataInPlayerPrefs();
            SceneManager.LoadScene("OnlineMenu");
        } else
        {
            ToastManager toastManager = FindObjectOfType<ToastManager>();
            toastManager.ShowToast(response);
        }
        Destroy(loadingScreenInstance);
    }

    public async void Login()
    {
        if (loadingScreenInstance != null)
        {
            return;
        }
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string response = await postRacerAPI.SignIn(usernameInput.text, passwordInput.text);
        if (response == "")
        {
            SaveUserDataInPlayerPrefs();
            SceneManager.LoadScene("OnlineMenu");
        } else
        {
            ToastManager toastManager = FindObjectOfType<ToastManager>();
            toastManager.ShowToast(response);
        }
        Destroy(loadingScreenInstance);
    }

    private void SaveUserDataInPlayerPrefs()
    {
        PlayerPrefs.SetString("username", usernameInput.text);
        PlayerPrefs.Save();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
