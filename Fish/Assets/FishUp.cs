using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishUp : Interactable
{
    public playercontroller player;
    public GameObject fisk;
    public float staminaVal;

    public void Start()
    {
        player = FindObjectOfType<playercontroller>();
        fisk = GameObject.Find("ipickthingsup");
    }
    
    public override void Interact()
    {
        base.Interact();
        fisk = gameObject;
        gameObject.transform.parent = player.transform;
        fisk.tag = "Fisk";

    }
}
