using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(playerMotor))]
public class playercontroller : MonoBehaviour
{
    public Interactable focus;
    Camera cam;
    playerMotor motor;
    public LayerMask movemenMask;
    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<playerMotor>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
          //  return;
        if (Input.GetMouseButtonDown(0))
        {
            //Skjuter ut en raycast från muspositionen
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, movemenMask))
            {
                motor.MovetoPoint(hit.point);
            }

            RemoveFocus();
        }

        if (Input.GetMouseButtonDown(1))
        {
            //Skjuter ut en raycast från muspositionen
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    SetFocus(interactable);
                }
            }
        }
    }
    void SetFocus (Interactable newFocus)
    {
        if (newFocus !=focus)
        {
            if (focus != null)
                focus.OndeFocused();



            focus = newFocus;
            motor.FollowTarget(newFocus);

        }
        newFocus.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
            focus.OndeFocused();
        
        focus = null;
        motor.StopFollowingTarget();
    }
}
        

    
