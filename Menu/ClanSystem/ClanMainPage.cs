using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClanMainPage : MonoBehaviour
{

    [SerializeField] private Text leaveButtonText;
    [SerializeField] private Text titleText;
    [SerializeField] private List<Text> memberTexts;
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Transform loadingParent;

    private string clan;
    private bool isOnwer;

    private GameObject loadingScreenInstance;
    private PostRacerAPI postRacerAPI;

    private void Start()
    {
        isOnwer = false;
        postRacerAPI = new PostRacerAPI();
        LoadPlayerClan();
    }

    private async void LoadPlayerClan()
    {
        titleText.text = "Loading...";
        loadingScreenInstance = Instantiate(loadingScreen, loadingParent);
        string clanName = await postRacerAPI.GetClanFromUser(PlayerPrefs.GetString("username"));
        if(clanName == "")
        {
            SceneManager.LoadScene("NoClanScene");
        } else
        {
            clan = clanName;
            titleText.text = clan;
            string clanOwner = await postRacerAPI.GetOwnerFromClan(clan);
            List<string> clanMembers = await postRacerAPI.GetMembersFromClan(clan);
            int i = 0;
            foreach(string currentClanMember in clanMembers)
            {
                float highscore = await postRacerAPI.GetHighscore(currentClanMember);
                float paketsHighscore = await postRacerAPI.GetHighscorePakets(currentClanMember);
                string displayName = "";
                if (currentClanMember.Length >= 9)
                {
                    for (int n = 0; n < 9; n++)
                    {
                        displayName += currentClanMember[n];
                    }
                    displayName += "...";
                }
                else
                    displayName = currentClanMember;
                memberTexts[i].text = displayName + ": " + highscore + " | " + paketsHighscore;
                if(clanOwner == currentClanMember)
                {
                    memberTexts[i].color = Color.red;
                }
                i++;
            }
            if(clanOwner == PlayerPrefs.GetString("username"))
            {
                leaveButtonText.text = "Owner Menu";
            } else
            {
                leaveButtonText.text = "LeaveClan";
            }
            Destroy(loadingScreenInstance);
        }
    }

    public void BackToOnlineMenu()
    {
        SceneManager.LoadScene("OnlineMenu");
    }

    public async void LeaveButtonClicked()
    {
        if(loadingScreenInstance != null)
        {
            return;
        }
        if(leaveButtonText.text.Contains("Leave"))
        {
            await postRacerAPI.RemovePlayerFromClan(PlayerPrefs.GetString("username"));
            SceneManager.LoadScene("OnlineMenu");
        } else // User is Owner of Clan
        {
            SceneManager.LoadScene("ClanOwnerMenu");
        }
    }

}
