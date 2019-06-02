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
    FishInfo fishInfo;
    public LayerMask movemenMask;
    bool playerReacted = false;
    float fishStamina;
    float FishStamina
    {
        get => fishStamina;

        set
        {
            if (value < 0) { fishStamina = 0; return; }
            if (value > fishInfo.maxFishStamina) { fishStamina = fishInfo.maxFishStamina; return; }
            fishStamina = value;
        }
    }

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
    [SerializeField] float playerHeldStaminaDrain = 10;

    #endregion
    #region Fishing Ui
    [SerializeField] Graphic playerStaminaBar;
    [SerializeField] Graphic playerLineStrBar;
    [SerializeField] Graphic fishBar;
    #endregion

    #region Player Fishing Stats
    [SerializeField] float staminaRegenRate = 5;
    [SerializeField] float playerMaxStamina = 100;
    [SerializeField] float playerMaxLineStr = 100;
    [SerializeField] float playerStamina;
    [SerializeField] float playerLineStr;
    [SerializeField] float lineDrain = 5;
    float PlayerStamina
    {
        get => playerStamina;
        set
        {
            if (value < 0) { playerStamina = 0; return; }
            if (value > playerMaxStamina) { playerStamina = playerMaxStamina; return; }
            playerStamina = value;
        }
    }
    #endregion
    [SerializeField]
    FishInfo debugFish;

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
        playerLineStrBar.rectTransform.localScale = new Vector3(currentLineStr, 1, 1);
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
                    StartCoroutine(FishOnLine(debugFish));
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
    private void LateUpdate()
    {
        if (fishing)
            Camera.main.GetComponent<CameraController>().cameraRotation = Mathf.LerpAngle(Camera.main.GetComponent<CameraController>().cameraRotation, transform.eulerAngles.y, 3 * Time.deltaTime);

        if (!playerReacted && playerStamina >= playerMaxStamina)
        {
            playerStaminaBar.transform.parent.gameObject.SetActive(false);
        }
        else playerStaminaBar.transform.parent.gameObject.SetActive(true);

        if (!playerReacted && playerLineStr >= playerMaxLineStr)
        {
            playerLineStrBar.transform.parent.gameObject.SetActive(false);
        }
        else playerLineStrBar.transform.parent.gameObject.SetActive(true);

        if (!playerReacted)
        {
            fishBar.gameObject.SetActive(false);
            fishBar.transform.parent.gameObject.SetActive(false);
        }
        else
        {
            fishBar.gameObject.SetActive(true);
            fishBar.transform.parent.gameObject.SetActive(true);
        }
    }
    IEnumerator FishOnLine(FishInfo _fishInfo)
    {
        fishInfo = _fishInfo;
        Vector3 lineStartPosition = floatObject.transform.position;
        Debug.Log("HOOKED");
        fishOnLine = true;
        floatAnimator.SetBool("Bounce", true);
         playerReacted = false;
        //bool fishMovingRight = false;
        //bool fishMoving = false;
        int fishMoveDir = 0;
        fishStamina = fishInfo.maxFishStamina;
        int playerHeldDir = 0;
        float dirChangeTime = 6;
        float timeTilDirChange = 0;
        float timerVal1 = 0;
        float wrongMod = 5;

        float timeBetweenAttacks = 0;
        float actualtTimeBetweenAttacks = 3;
        float attackTimer = 0;

        float yeetWindow = .5f;
        float diveWindow = 3;


        float attackDurationTimer = 0;
        bool yeeting = false;
        bool diving = false;

        while (fishing && fishingPrepared && fishOnLine)
        {

            if (Input.GetKey(KeyCode.Space) && !playerReacted)
            {
                Debug.Log("!");
                Camera.main.GetComponent<AudioSource>().clip = fishInfo.song;
                Camera.main.GetComponent<AudioSource>().Play(0);
                exclamationPoint.SetActive(true);
                exclamationPoint.GetComponent<Animator>().Play("Expoint", 0);


                playerReacted = true;
                splashSystem.gameObject.SetActive(true);
                floatAnimator.SetBool("Bounce", false);
                yield return new WaitForSeconds(1);
                exclamationPoint.SetActive(false);
            }


            if (playerReacted)
            {
                fishBar.rectTransform.localScale = new Vector3(FishStamina / fishInfo.maxFishStamina, 1, 1);
                //Debug.LogFormat("Fish Moving: {0}, Fish Moving Right: {1}, Player holding: {2}, Time Left: {3} / {4}", fishMoving, fishMovingRight, playerHeldDir, timerVal1, timeTilDirChange);
                //  Debug.LogFormat("Player held dir: {0} Fish move direction: {1} Time out of: {2} / {3}", playerHeldDir, fishMoveDir, timerVal1, timeTilDirChange);
                if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D)) { playerHeldDir = -1; } else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) playerHeldDir = 1; else playerHeldDir = 0;
                //   PlayerStamina -= _fishInfo.passiveDrain * Time.deltaTime;

                switch (fishMoveDir)
                {
                    case (0):
                        floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, lineStartPosition, .5f);
                        if (playerHeldDir == 0)
                        {
                            FishStamina += fishInfo.fishRegen * Time.deltaTime;
                            //  PlayerStamina -= _fishInfo.passiveDrain * Time.deltaTime;
                        }
                        break;
                    case (-1):

                        floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, lineStartPosition + Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(-4f, 0, 0), .5f);
                        if (playerHeldDir == -1)
                        {
                            FishStamina += fishInfo.fishRegen * wrongMod * Time.deltaTime;

                        }
                        else if (playerHeldDir == 1)
                        {
                            FishStamina -= playerHeldStaminaDrain * Time.deltaTime;
                            PlayerStamina -= fishInfo.passiveDrain * Time.deltaTime;
                        }
                        else if (playerHeldDir == 0)
                        {
                            FishStamina += fishInfo.fishRegen * wrongMod * Time.deltaTime;
                            PlayerStamina -= fishInfo.passiveDrain * Time.deltaTime;
                        }

                        break;
                    case (1):

                        floatObject.transform.position = Vector3.MoveTowards(floatObject.transform.position, lineStartPosition + Quaternion.Euler(0, transform.eulerAngles.y, 0) * new Vector3(4f, 0, 0), .5f);


                        if (playerHeldDir == 1)
                        {
                            FishStamina += fishInfo.fishRegen * 2 * Time.deltaTime;

                        }
                        else if (playerHeldDir == -1)
                        {
                            FishStamina -= playerHeldStaminaDrain * Time.deltaTime;
                            PlayerStamina -= fishInfo.passiveDrain * Time.deltaTime;
                        }
                        else if (playerHeldDir == 0)
                        {
                            FishStamina += fishInfo.fishRegen * 2 * Time.deltaTime;
                            PlayerStamina -= fishInfo.passiveDrain * Time.deltaTime;
                        }

                        break;


                }
                attackTimer += Time.deltaTime;
                if (attackTimer >= actualtTimeBetweenAttacks)
                {
                    actualtTimeBetweenAttacks = Random.Range(0, timeBetweenAttacks);
                    attackTimer = 0;
                    int rnd = 0;// Random.Range(0, 2);
                    if (!yeeting && !diving)
                    {
                        switch (rnd)
                        {
                            case (0):
                                //YeEEt
                                floatAnimator.SetBool("Yeet", true);
                                Debug.Log("Yote");
                                yeeting = true;
                                attackDurationTimer = 0;
                                actualtTimeBetweenAttacks += yeetWindow;
                                break;
                            case (1):
                                //Dont yeet
                                actualtTimeBetweenAttacks += diveWindow;
                                floatAnimator.SetBool("Dive", true);
                                attackDurationTimer = 0;
                                diving = true;
                                break;
                        }
                    }

                }
                if (yeeting)
                {
                    //                    Debug.Log("YEETING");

                    attackDurationTimer += Time.deltaTime;
                    // Debug.Log(attackDurationTimer + " " + yeetWindow);
                    //                  Debug.Log(attackDurationTimer);
                    if (attackDurationTimer >= yeetWindow)
                    {

                        Debug.Log("Yoted");
                        playerLineStr -= fishInfo.yeetAttackDamage.lineDamage;
                        yeeting = false;
                        floatAnimator.SetBool("Yeet", false);
                        attackDurationTimer = 0;
                        attackTimer = 0;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        Debug.Log("Saved");
                        yeeting = false;
                        floatAnimator.SetBool("Yeet", false);
                        attackDurationTimer = 0;
                        attackTimer = 0;
                    }
                }

                timerVal1 += 1 * Time.deltaTime;
                if (timerVal1 >= timeTilDirChange)
                {
                    timerVal1 = 0;
                    timeTilDirChange = Random.Range(1, dirChangeTime);
                    int rnd = Random.Range(-1, 2);
                    fishMoveDir = rnd;




                    //if (rnd == 1) { fishMovingRight = !fishMovingRight; }
                }

                if (playerStamina <= 0 || playerLineStr <= 0) { StartCoroutine(StopFishing()); }

                if (fishStamina <= 0 && Input.GetMouseButton(1))
                {
                    CatchFish(lineStartPosition);
                }

            }

            yield return new WaitForEndOfFrame();


        }
    }

    public void ResetStamina()
    {
        PlayerStamina = playerMaxStamina;
    }

    IEnumerator StopFishing()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
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
        playerReacted = false;
        fishing = false;
        fishingPrepared = false;
        fishOnLine = false;
    }
    public void CatchFish(Vector3 floatPos)
    {
        Debug.Log("FISH CAUTH!");
        GameObject fishObject = Instantiate(fishInfo.caughtObject);
        fishObject.transform.position = floatPos;


        StartCoroutine(StopFishing());


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



