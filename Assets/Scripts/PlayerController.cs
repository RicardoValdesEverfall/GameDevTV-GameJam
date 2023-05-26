using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    
    [SerializeField] public enum PlayerState { is2D, is3D, isDead };
    [SerializeField] public PlayerState PlayerCurrentState;
    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private Animator ConsoleAnimator;

    private MainMenuManager _mainMenuRef;
    private AudioController _audioControllerRef;
    public bool consoleStatus; //False = closed, True = open.

    [Header("3D Attributes")]
    [SerializeField] private Transform cameraTransform_3D;
    [SerializeField, Range(1, 10)] public int CameraSensitivity_3D;

    private Vector2 playerInput_3D;
    private Vector3 rotateValue_3D;
    private Vector3 cameraStartPosition;
    private Vector3 cameraPeekOffset;

    private Quaternion startingRotation_3D;

    private bool isLooking_3D;


    #region 2D ATTRIBUTES
    [Header("2D Attributes")]
    
    [SerializeField] public GameObject PlayerGameObject_2D;
    [SerializeField] private GameObject PlayerCrabBodyObj_2D;
    [SerializeField] private GameObject EnvironmentMap_2D;
    [SerializeField] private Transform CrabBody_2D;
    [SerializeField, Range(0f, 100f)] private float maxSpeed_2D;
    [SerializeField, Range(100f, 200f)] private float rotationSpeed_2D;
    [SerializeField] private float crabBodyOffset_2D;

    private List<GameObject> crabPool = new List<GameObject>();
    private List<Transform> crabPositionsList = new List<Transform>();
    private Vector2 playerInput_2D;
    private int numberOfCrabs_2D = 0;
    private int crabPoolSize = 20;

    #endregion

    void Start()
    {
        if (_mainMenuRef == null) { _mainMenuRef = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>(); }
        if (_mainMenuRef.CurrentState != MainMenuManager.MenuStates.game) { _mainMenuRef.CurrentState = MainMenuManager.MenuStates.game; }
        if (PlayerAnimator == null) { PlayerAnimator = this.GetComponent<Animator>(); }
        if (ConsoleAnimator == null) { ConsoleAnimator = GameObject.FindGameObjectWithTag("ConsoleAnimator").GetComponent<Animator>(); }
        if (PlayerGameObject_2D == null) { PlayerGameObject_2D = GameObject.FindGameObjectWithTag("Player2D"); }
        if (EnvironmentMap_2D == null) { EnvironmentMap_2D = GameObject.FindGameObjectWithTag("Environment2D"); }
        if (_audioControllerRef == null) { _audioControllerRef = this.GetComponent<AudioController>(); }

        PlayerCurrentState = PlayerState.is2D;
        startingRotation_3D = cameraTransform_3D.localRotation;
        cameraStartPosition = cameraTransform_3D.position;
        cameraPeekOffset = cameraTransform_3D.position + new Vector3(-0.2f, 0.4f, 0f);
        crabPositionsList.Add(PlayerGameObject_2D.transform);
        Cursor.lockState = CursorLockMode.Locked;

        for(int i = 0; i < crabPoolSize; i++)
        {
            GameObject crabBody = Instantiate(PlayerCrabBodyObj_2D, CrabBody_2D);
            crabBody.SetActive(false);
            crabPool.Add(crabBody);
        }
    }

    void Update()
    {
        switch (PlayerCurrentState)
        {
            case PlayerState.is2D:
                if (!consoleStatus) { HandleConsole("OpenConsole"); consoleStatus = true; }
                Handle2DCrabs();
                Handle2DInput();
                if (Input.GetMouseButton(2))
                {
                    _audioControllerRef.isLooking = true;
                    HandleRotation();
                    Handle3DInput();
                }
                else { ResetCamera(); _audioControllerRef.isLooking = false; }
                break;
            case PlayerState.is3D:
                if (consoleStatus) { HandleConsole("CloseConsole"); consoleStatus = false; }
                HandleRotation();
                Handle3DInput();
                break;
            case PlayerState.isDead:
                HandleGameOver();
                break;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerCurrentState == PlayerState.is2D)
        {
            //PlayerGameObject_2D.transform.Translate(Vector2.up * maxSpeed_2D * Time.fixedDeltaTime, Space.Self);
            PlayerGameObject_2D.transform.Rotate(Vector3.forward * playerInput_2D.x * rotationSpeed_2D * Time.fixedDeltaTime);

            Vector3 playerDir = PlayerGameObject_2D.transform.up;

            EnvironmentMap_2D.transform.Translate(-playerDir * (maxSpeed_2D * 1.25f) * Time.fixedDeltaTime, Space.World);
        }
        else { return; }
    }

    public void Handle2DInput()
    {
        playerInput_2D.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Tab) && consoleStatus)
        {
            PlayerCurrentState = PlayerState.is3D;
            HandleConsole("CloseConsole");
        }
    }

    public void Handle2DPickup(int pickUpID) //0 = add a crab, 1 = add a knife
    {   
        if (pickUpID == 0)
        {
            crabPool[numberOfCrabs_2D].SetActive(true);
            crabPositionsList.Add(crabPool[numberOfCrabs_2D].transform);
            numberOfCrabs_2D++;
        }
       
        if (pickUpID == 1)
        {

        }
    }

    private void Handle2DCrabs()
    {
        for (int i = 0; i < crabPool.Count; i++)
        {
          if (crabPool[i].activeSelf)
          {
                /*Vector3 pointToFollow = crabPositionsList[i].position - crabPool[i].transform.position;
                pointToFollow = pointToFollow / crabBodyOffset_2D;

                crabPool[i].transform.position = pointToFollow;
                crabPool[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, pointToFollow);*/


                // Vector3 pointToFollow = crabPositionsList[i].transform.
                //crabPool[i].transform.right = crabPositionsList[i].transform.right;

                Vector3 pointToFollow = crabPositionsList[i].transform.position - (transform.InverseTransformDirection(crabPositionsList[i].up * crabBodyOffset_2D));
                crabPool[i].transform.position = pointToFollow;
                crabPool[i].transform.rotation = Quaternion.Lerp(crabPool[i].transform.rotation, crabPositionsList[i].rotation, 6.8f * Time.deltaTime);/*------------------------------------------------ NEW CODE*/

            }
        }
    }

    public void Handle3DInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerCurrentState = PlayerState.is2D;
        }
    }

    private void HandleRotation()
    {
        playerInput_3D.y = Input.GetAxis("Mouse X") * CameraSensitivity_3D;
        playerInput_3D.x = Input.GetAxis("Mouse Y") * CameraSensitivity_3D;

        rotateValue_3D.y -= playerInput_3D.y;
        rotateValue_3D.x += playerInput_3D.x;

        rotateValue_3D.x = Mathf.Clamp(rotateValue_3D.x, -90f, 45f);
        rotateValue_3D.y = Mathf.Clamp(rotateValue_3D.y, -90f, 45f);

        float FOV = cameraTransform_3D.GetComponent<Camera>().fieldOfView;
        FOV = Mathf.Lerp(FOV, 80f, 8.8f * Time.deltaTime);
        cameraTransform_3D.GetComponent<Camera>().fieldOfView = FOV;

        cameraTransform_3D.position = Vector3.MoveTowards(cameraTransform_3D.position, cameraPeekOffset, 3.8f * Time.deltaTime);
        cameraTransform_3D.rotation = Quaternion.Euler(-rotateValue_3D);

        Debug.Log("Rotating Camera");
    }

    public void ResetCamera()
    {
        cameraTransform_3D.rotation = Quaternion.Lerp(cameraTransform_3D.rotation, startingRotation_3D, 1.8f * Time.deltaTime);
        cameraTransform_3D.position = Vector3.MoveTowards(cameraTransform_3D.position, cameraStartPosition, 2.8f * Time.deltaTime);

        float FOV = cameraTransform_3D.GetComponent<Camera>().fieldOfView;
        FOV = Mathf.Lerp(FOV, 60f, 0.8f * Time.deltaTime);
        cameraTransform_3D.GetComponent<Camera>().fieldOfView = FOV;

    }

    public void HandleConsole(string animToPlay)
    {
        PlayerAnimator.Play(animToPlay);
    }

    public void HandleAIWarning()
    {
        ConsoleAnimator.Play("EyesMovement");
    }

    public void HandleGameOver()
    {
        PlayerAnimator.Play("GameOver");
        _mainMenuRef.GameOver();
    }

    public void ShowSettingsMenu(int state)
    {
        _mainMenuRef.SettingsMenu(state);
    }
}
