using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
[RequireComponent(typeof(playerMotor))]
public class playercontroller : MonoBehaviour
{
    public Interactable focus;
    Camera cam;
    playerMotor motor;
    [SerializeField] public float fishingRange = 10;
    public bool fishing = false;
    bool fishingPrepared = false;
    bool fishOnLine = false;

    public LayerMask movemenMask;


    public Vector3 floatTargetPosition;


    #region Fishing rod stuffs
    [SerializeField] GameObject floatObject;
    [SerializeField] GameObject rod;
    [SerializeField] Transform rodEndPoint;
    [SerializeField] LineRenderer fishingLine;
    [SerializeField] Animator floatAnimator;
    [SerializeField] Transform lineEndPoint;
    [SerializeField] GameObject exclamationPoint;
    [SerializeField] ParticleSystem splashSystem;


    #endregion
    #region Fishing Ui
    [SerializeField] Graphic playerStaminaBar;
    [SerializeField] Graphic playerLineStrBar;

    #endregion

    #region Player Fishing Stats

    [SerializeField] float playerMaxStamina = 100;
    [SerializeField] float playerMaxLineStr = 100;
    [SerializeField] float playerStamina;
    [SerializeField] float playerLineStr;

    #endregion


    void Start()
    {
        cam = Camera.main;
        motor = GetComponent<playerMotor>();
        rod.transform.localScale = Vector3.zero;
        rod.gameObject.SetActive(false);

        playerStamina = playerMaxStamina;
        playerLineStr = playerMaxLineStr;

    }

    void UpdateBars()
    {
        float currentStm = playerStamina / playerMaxStamina;
        playerStaminaBar.rectTransform.localScale = new Vector3(currentStm, 1, 1);
        float currentLineStr = playerLineStr / playerMaxLineStr;
        playerLineStrBar.rectTransform.localEulerAngles = new Vector3(currentLineStr, 1, 1);
    }

    // Update is called once per frame
    void Update()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //  return;
        UpdateBars();

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
            fishingLine.SetPosition(1, lineEndPoint.position);
            //Debug.Log("Fishinf");
            if (fishingPrepared)
            {
                Debug.Log("prepared");
                if (Random.Range(0, 100) <= 25 && !fishOnLine)
                {
                    StartCoroutine(FishOnLine(Resources.Load("", typeof(FishInfo)) as FishInfo));
                }

                //      Debug.Log("Fish prep");
                if (Input.GetKey(KeyCode.Escape))
                {
                    //          Debug.Log("Escpae");
                    StartCoroutine(StopFishing());
                }
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

    IEnumerator FishOnLine(FishInfo fishInfo)
    {
        Vector3 lineStartPosition = floatObject.transform.position;
        Debug.Log("HOOKED");
        fishOnLine = true;
        floatAnimator.SetBool("Bounce", true);
        bool playerReacted = false;

        while (fishing && fishingPrepared && fishOnLine)
        {

            if (Input.GetKey(KeyCode.Space) && !playerReacted)
            {
                Debug.Log("!");

                exclamationPoint.SetActive(true);
                exclamationPoint.GetComponent<Animator>().Play("Expoint", 0);


                playerReacted = true;
                splashSystem.gameObject.SetActive(true);
                floatAnimator.SetBool("Bounce", false);
                yield return new WaitForSeconds(1);
                exclamationPoint.SetActive(false);
            }

            yield return new WaitForEndOfFrame();


        }
    }

    IEnumerator StopFishing()
    {
        splashSystem.gameObject.SetActive(false);
        ResetFishBools();
        while (floatObject.transform.position != rodEndPoint.transform.position)
        {
            floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, rodEndPoint.transform.position, 20f * Time.deltaTime);
            fishingLine.SetPosition(0, rodEndPoint.position);
            fishingLine.SetPosition(1, lineEndPoint.position);
            yield return new WaitForEndOfFrame();
        }
        while (rod.transform.localScale != Vector3.zero)
        {
            rod.transform.localScale = Vector3.MoveTowards(rod.transform.localScale, Vector3.zero, 10f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        rod.SetActive(false);

    }

    void ResetFishBools()
    {
        fishing = false;
        fishingPrepared = false;
        fishOnLine = false;
    }


    IEnumerator StartFish(Vector3 floatStartPosition)
    {
        rod.SetActive(true);
        fishing = true;
        float floatSpeed = 20;
        rod.gameObject.SetActive(true);
        floatObject.transform.position = rodEndPoint.position;
        while (rod.transform.localScale != Vector3.one)
        {
            rod.transform.localScale = Vector3.MoveTowards(rod.transform.localScale, Vector3.one, 10f * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }


        while (Vector3.Distance(floatObject.transform.position, floatStartPosition) > .5f)
        {
            // floatObject.transform.position = Vector3.Lerp(floatObject.transform.position, floatStartPosition, floatSpeed * Time.deltaTime);
            floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, floatStartPosition, Time.deltaTime * floatSpeed);

            yield return new WaitForEndOfFrame();
        }
        //     Debug.Log("Float at position");
        fishingPrepared = true;

    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, -fishingRange);
    }


    void RemoveFocus()
    {
        if (focus != null)
            focus.OndeFocused();

        focus = null;
        motor.StopFollowingTarget();
    }
}



