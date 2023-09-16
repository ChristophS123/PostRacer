using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BreakMenu : MonoBehaviour
{
    
    public void Resume()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        gameManager.ToggleBreak();
    }

    public void BackToMenu()
    {
        if(PlayerPrefs.GetString("gametype") == "offline")
        {
            SceneManager.LoadScene("OfflineMenu");
        } else
        {
            SceneManager.LoadScene("OnlineMenu");
        }
    }

}
