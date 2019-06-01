
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;
    bool isfocus = false;
    bool hasInteracted = false;
    Transform player;

    public virtual void Interact()
    {
        //this method is meant to be overwritten
        Debug.Log("interacting with" + transform.name);
    }
    private void Update()
    {
        if (isfocus && !hasInteracted)
        {
            float distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                hasInteracted = true;
            }
        }
    }

    public void OnFocused (Transform playerTransform)
    {
        isfocus = true;
        player = playerTransform;
        hasInteracted = false;
    }

    public void OndeFocused()
    {
        isfocus = false;
        player = null;
        hasInteracted = false;
    }
    
    private void OnDrawGizmosSelected()
    {
        if (interactionTransform == null)
            interactionTransform = transform;

        
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }
}
