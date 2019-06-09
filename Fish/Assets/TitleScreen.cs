using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour
{
   
  public void PlayGame()
    {
        SceneManager.LoadScene("mainscene");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("you quit the game");
    }

    public void instructions()
    {
        SceneManager.LoadScene("Instructions");
    }
}

