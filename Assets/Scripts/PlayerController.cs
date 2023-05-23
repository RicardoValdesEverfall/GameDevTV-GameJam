using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Header("General Attributes")]
    [SerializeField] public enum PlayerState { is2D, is3D, isDead };
    [SerializeField] public PlayerState PlayerCurrentState;
    [SerializeField] private Animator PlayerAnimator;

    private MainMenuManager _mainMenuRef;

    private bool consoleStatus; //False = closed, True = open.

    [Header("3D Attributes")]
    [SerializeField] private Transform cameraTransform_3D;
    [SerializeField, Range(1, 10)] public int CameraSensitivity_3D;

    private Vector2 playerInput_3D;
    private Vector3 rotateValue_3D;

    private Quaternion startingRotation_3D;

    private bool isLooking_3D;


    #region 2D ATTRIBUTES
    [Header("2D Attributes")]
    
    [SerializeField] public GameObject PlayerGameObject_2D;
    [SerializeField] private GameObject PlayerCrabBodyObj_2D;
    [SerializeField] private GameObject EnvironmentMap_2D;
    [SerializeField] private Transform CrabBody_2D;
    [SerializeField, Range(10f, 100f)] private float maxSpeed_2D;
    [SerializeField, Range(100f, 200f)] private float rotationSpeed_2D;
    [SerializeField, Range(5f, 10f)] private float crabBodyOffset_2D;

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
        if (PlayerGameObject_2D == null) { PlayerGameObject_2D = GameObject.FindGameObjectWithTag("Player2D"); }
        if (EnvironmentMap_2D == null) { EnvironmentMap_2D = GameObject.FindGameObjectWithTag("Environment2D"); }

        PlayerCurrentState = PlayerState.is2D;
        startingRotation_3D = cameraTransform_3D.localRotation;
        crabPositionsList.Add(PlayerGameObject_2D.transform);

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
                    HandleRotation();
                    Handle3DInput();
                }
                else { ResetCamera(); }
                break;
            case PlayerState.is3D:
                if (consoleStatus) { HandleConsole("CloseConsole"); consoleStatus = false; }
                HandleRotation();
                Handle3DInput();
                break;
            case PlayerState.isDead:
                break;
        }
    }

    private void FixedUpdate()
    {
        if (PlayerCurrentState == PlayerState.is2D)
        {
            PlayerGameObject_2D.transform.Translate(Vector2.up * maxSpeed_2D * Time.fixedDeltaTime, Space.Self);
            PlayerGameObject_2D.transform.Rotate(Vector3.forward * playerInput_2D.x * rotationSpeed_2D * Time.fixedDeltaTime);

            Vector3 playerDir = PlayerGameObject_2D.transform.up;

            EnvironmentMap_2D.transform.Translate(-playerDir * (maxSpeed_2D * 1.25f) * Time.fixedDeltaTime, Space.World);
        }
        else { return; }
    }

    public void Handle2DInput()
    {
        playerInput_2D.x = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Escape))
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
                Vector3 pointToFollow = crabPositionsList[i].position - crabPool[i].transform.position;
                pointToFollow = pointToFollow / crabBodyOffset_2D;

                crabPool[i].transform.position += pointToFollow * maxSpeed_2D * Time.deltaTime;
                crabPool[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, pointToFollow);
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
        rotateValue_3D.x -= playerInput_3D.x;

        rotateValue_3D.x = Mathf.Clamp(rotateValue_3D.x, -90f, 45f);
        rotateValue_3D.y = Mathf.Clamp(rotateValue_3D.y, -90f, 45f);

        cameraTransform_3D.rotation = Quaternion.Euler(rotateValue_3D);
    }

    public void ResetCamera()
    {
        cameraTransform_3D.rotation = Quaternion.Lerp(cameraTransform_3D.rotation, startingRotation_3D, 1.8f * Time.deltaTime);
    }

    public void HandleConsole(string animToPlay)
    {
        PlayerAnimator.Play(animToPlay);
    }

    public void ShowSettingsMenu(int state)
    {
        _mainMenuRef.SettingsMenu(state);
    }
}
