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

    public bool fishing = false;
    bool fishingPrepared = false;

    public LayerMask movemenMask;


    public Vector3 floatTargetPosition;


    #region Fishing rod stuffs
    [SerializeField] GameObject floatObject;
    [SerializeField] GameObject rod;
    [SerializeField] Transform rodEndPoint;
    [SerializeField] LineRenderer fishingLine;

    #endregion


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


        #region Not fishing
        if (!fishing)
        {
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
        #endregion
        else
        {
            fishingLine.SetPosition(0, rodEndPoint.position);
            fishingLine.SetPosition(1, floatObject.transform.position);

            if (fishingPrepared)
            {

            }
        }

    }
    void SetFocus(Interactable newFocus)
    {
        if (newFocus != focus)
        {
            if (focus != null)
                focus.OndeFocused();



            focus = newFocus;
            if (newFocus.GetComponent<FishingZone>() == null)
            {
                motor.FollowTarget(newFocus);
            }

        }
        newFocus.OnFocused(transform);
    }


    public void StartFishing(Vector3 floatStartPosition)
    {
        StartCoroutine(StartFish(floatStartPosition));
    }

    IEnumerator StartFish(Vector3 floatStartPosition)
    {
        fishing = true;
        float floatSpeed = 10;
        rod.gameObject.SetActive(true);
        floatObject.transform.position = rodEndPoint.position;
        while (floatObject.transform.position != floatStartPosition)
        {
            // floatObject.transform.position = Vector3.Lerp(floatObject.transform.position, floatStartPosition, floatSpeed * Time.deltaTime);
            floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, floatStartPosition, Time.deltaTime * floatSpeed);
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Float at position");
        fishingPrepared = true;

    }



    void RemoveFocus()
    {
        if (focus != null)
            focus.OndeFocused();

        focus = null;
        motor.StopFollowingTarget();
    }
}



