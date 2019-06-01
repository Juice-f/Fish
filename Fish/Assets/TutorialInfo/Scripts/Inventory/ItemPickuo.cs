using UnityEngine;

[SerializeField]
public class ItemPickuo : Interactable
{
    
    public GameObject item; 
   
    public override void Interact()
    {
        base.Interact();

        Pickup();
    }

    void Pickup()
    {
        Debug.Log("Picking up " + item.name);
       
            Destroy(gameObject);
       
        
    }
}
