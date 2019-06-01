using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fishing : Interactable
{
    public override void Interact()
    {
        base.Interact();
        //This is where we add the fishing state
        Debug.Log("Hej du fiskar nu");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
