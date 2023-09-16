using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClanOwnerMenu : MonoBehaviour
{

    [SerializeField] private Text usernameText;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;

    private GameObject loadingScreenInstance;
    private string clan;
    private List<string> clanMembers;
    private PostRacerAPI postRacerAPI;

    private void Start()
    {
        postRacerAPI = new PostRacerAPI();
        LoadPlayerClan();
    }

    private async void LoadPlayerClan()
    {
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string clanName = await postRacerAPI.GetClanFromUser(PlayerPrefs.GetString("username"));
        if (clanName == "")
        {
            SceneManager.LoadScene("OnlineMenu");
        }
        else
        {
            clan = clanName;
            clanMembers = await postRacerAPI.GetMembersFromClan(clan);
            Destroy(loadingScreenInstance);
        }
    }

    public async void OnKickButtonClicked()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        string username = usernameText.text;
        ToastManager toastManager = FindObjectOfType<ToastManager>();
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string result = await postRacerAPI.KickPlayerFromClan(clan, username);
        toastManager.ShowToast(result);
        usernameText.text.Remove(0);
        Destroy(loadingScreenInstance);
    }

    public async void OnInviteButtonClicked()
    {
        if (loadingScreenInstance != null)
        {
            return;
        }
        string username = usernameText.text;
        ToastManager toastManager = FindObjectOfType<ToastManager>();
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string result = await postRacerAPI.InvitePlayerToClan(clan, username);
        toastManager.ShowToast(result);
        usernameText.text.Remove(0);
        Destroy(loadingScreenInstance);
    }

    public async void OnDeleteButtonClicked()
    {
        if (loadingScreenInstance != null)
        {
            return;
        }
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        await postRacerAPI.DeleteClan(clan);
        Destroy(loadingScreenInstance);
        SceneManager.LoadScene("OnlineMenu");
    }

    public void BackToMainClanMenu()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        SceneManager.LoadScene("MainClanScene");
    }

}
