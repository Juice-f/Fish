﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(FishingZone))]
public class Fishing : Interactable
{
    bool fishing = false;
    Vector3 playerPosition;
    GameObject playerGO;
    Vector3 fishingTargetPos;
    public FishingZone FishingZone { get => GetComponent<FishingZone>(); }
    public override void Interact()
    {
        base.Interact();
        //This is where we add the fishing state
        Debug.Log("Hej du fiskar nu");
        fishing = true;


    }

    public override void OnFocused(Transform playerTransform)
    {


        if (Vector3.Distance(playerTransform.position, FishingZone.FishCenter) < FishingZone.fishingZoneRadius)
        {
            // base.OnFocused(playerTransform);
            playerGO = playerTransform.gameObject;
            Ray rayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
            //GameObject TESTcube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            RaycastHit hit;
            if (Physics.Raycast(rayCast, out hit) && Vector3.Distance(playerTransform.position, hit.point) <= playerTransform.GetComponent<playercontroller>().fishingRange)
            {
               // TESTcube.transform.position = hit.point;
                playerTransform.LookAt(hit.point);
                playerTransform.GetComponent<NavMeshAgent>().SetDestination(playerTransform.transform.position);
                playerTransform.GetComponent<playercontroller>().StartFishing(hit.point);
            }



        }
        //  fishingTargetPos = 
    }

    // Update is called once per frame
    void Update()
    {
        if (fishing)
        {

        }
    }
}