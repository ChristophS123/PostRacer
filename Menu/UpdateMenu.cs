using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UpdateMenu : MonoBehaviour
{
    
    public void BackToGameMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
