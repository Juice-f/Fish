using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Instructions : MonoBehaviour
{

    public void Backbutton()
    {
        SceneManager.LoadScene("Title Screen");
    }
}
