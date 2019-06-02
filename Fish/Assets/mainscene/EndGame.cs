using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : Interactable
{
    [SerializeField]
    public GameObject Black;
    public playercontroller player;
    
    public bool Fadeout = false;

    public void Start()
    {
        Black.SetActive(false);
        player = FindObjectOfType<playercontroller>();
    }

    public override void Interact()
    {
        Debug.Log("Fade in plox");
        FadeOut();
    }


    public void FadeOut()
    {
        Fadeout = true;
        Black.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(Timer());
        player.GetComponent<playercontroller>().ResetLines();
        
    }

    public IEnumerator Timer()
    {
        Cursor.lockState = CursorLockMode.None;
        yield return new WaitForSeconds(2f);
        Fadeout = false;
        Black.SetActive(false);

    }

}
