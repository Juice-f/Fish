using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Yeet : Interactable

{
    public float kickstrength = 10;
    public override void Interact()
    {
        base.Interact();
        GetComponent<Rigidbody>().AddForce(new Vector3(25, 25) * kickstrength);


    }
    
}
