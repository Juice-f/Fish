﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireplace : Interactable
//{
{
    public playercontroller player;
    public GameObject Fisk;


    public void Start()
    {
        player = FindObjectOfType<playercontroller>();
        
    }



    public override void Interact()
    {
        base.Interact();
        FindObjectOfType<playercontroller>().playerMaxStamina += GameObject.FindGameObjectWithTag("Fisk").GetComponent<FishUp>().staminaVal;
        Destroy(Fisk = GameObject.FindGameObjectWithTag("Fisk"));


    }
}

//    public playercontroller player;
//    public GameObject fiskar;


//  public override void Interact()
//    {
//        base.Interact();
      

//    }
//    public void Start()
//    {
//        player = FindObjectOfType<playercontroller>();
//    }

//    public void Update()
//    {
//        fiskar = GameObject.FindGameObjectWithTag("Fisk");
//    }


      
//}


