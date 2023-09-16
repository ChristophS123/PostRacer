using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NoClanMenu : MonoBehaviour
{

    [SerializeField] private GameObject invitation;
    [SerializeField] private Text invitationClanNameText;

    [SerializeField] private Text clanNameText;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingScreenParent;

    private GameObject loadingScreenInstance;
    private PostRacerAPI postRacerAPI;

    private void Start()
    {
        postRacerAPI = new PostRacerAPI();
        LoadInvitation();
    }

    private async void LoadInvitation()
    {
        invitation.SetActive(false);
        if(loadingScreenInstance != null)
        {
            return;
        }
        loadingScreenInstance = Instantiate(loadingScreen, loadingScreenParent);
        string invitationClanName = await postRacerAPI.GetInvitation(PlayerPrefs.GetString("username"));
        if(invitationClanName == "")
        {
            Destroy(invitation);
        }
        else
        {
            invitation.SetActive(true);
            invitationClanNameText.text = "Clan: " + invitationClanName;
        }
        Destroy(loadingScreenInstance);
    }

    public async void AcceptInvitation()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        await postRacerAPI.AcceptInvitation(PlayerPrefs.GetString("username"));
        SceneManager.LoadScene("MainClanScene");
    }

    public async void DenyInvitation()
    {
        if (loadingScreenInstance != null)
        {
            return;
        }
        await postRacerAPI.DenyInvitation(PlayerPrefs.GetString("username"));
        Destroy(invitation);
    }

    public async void CreateClan()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        if (clanNameText.text == "")
            return;
        ToastManager toastManager = FindObjectOfType<ToastManager>();
        string clanName = clanNameText.text;
        loadingScreenInstance = Instantiate(loadingScreen, loadingScreenParent);
        string response = await postRacerAPI.CreateClan(clanName, PlayerPrefs.GetString("username"));
        if(response == "")
        {
            SceneManager.LoadScene("MainClanScene");
            return;
        }
        toastManager.ShowToast(response);
        Destroy(loadingScreenInstance);
    }

    public void BackToOnlineMenu()
    {
        SceneManager.LoadScene("OnlineMenu");
    }

}
