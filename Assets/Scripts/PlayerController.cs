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
    [SerializeField] private BackgroundScroller[] BackgroundScroller_2D;
    [SerializeField, Range(0f, 100f)] private float maxSpeed_2D;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration_2D;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration_2D;

    [SerializeField, Range(10f, 100f)] private int jumpHeight_2D;
    [SerializeField, Range(0f, 30f)] private float upwardMultiplier_2D;
    [SerializeField, Range(0f, 30f)] private float downwardMultiplier_2D;
    [SerializeField, Range(0f, 3f)] private int maxAirJumps_2D;
    [SerializeField] private float defaultGravityScale_2D;

    private Vector2 playerDir_2D;
    private Vector2 playerInput_2D;

    private Vector2 velocity_2D;
    private Vector2 desiredVelocity_2D;

    private Rigidbody2D playerBody_2D;
    private Ground environmentGround_2D;

    private float maxSpeedChange_2D;
    private float acceleration_2D;
    private bool _onGround_2D;
    private bool desiredJump;
    private int jumpPhase_2D;
    #endregion

    void Start()
    {
        if (_mainMenuRef == null) { _mainMenuRef = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>(); }
        if (_mainMenuRef.CurrentState != MainMenuManager.MenuStates.game) { _mainMenuRef.CurrentState = MainMenuManager.MenuStates.game; }
        if (PlayerAnimator == null) { PlayerAnimator = this.GetComponent<Animator>(); }
        PlayerCurrentState = PlayerState.is2D;
        startingRotation_3D = cameraTransform_3D.localRotation;

        if (PlayerGameObject_2D == null) { PlayerGameObject_2D = GameObject.FindGameObjectWithTag("Player2D"); }
        //if (BackgroundScroller_2D == null) { BackgroundScroller_2D = GameObject.FindGameObjectWithTag("BackgroundController").GetComponent<BackgroundScroller>(); }
        if (playerBody_2D == null) { playerBody_2D = PlayerGameObject_2D.GetComponent<Rigidbody2D>(); }
        if (environmentGround_2D == null) { environmentGround_2D = PlayerGameObject_2D.GetComponent<Ground>(); }
    }

    void Update()
    {
        switch (PlayerCurrentState)
        {
            case PlayerState.is2D:
                if (!consoleStatus) { HandleConsole("OpenConsole"); consoleStatus = true; }

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
            
        }
        else { return; }
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

    public void Handle2DInput()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PlayerCurrentState = PlayerState.is3D;
            HandleConsole("CloseConsole");
        }
    }

    public void Handle3DInput()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PlayerCurrentState = PlayerState.is2D;
        }
    }

    public void HandleConsole(string animToPlay)
    {
        PlayerAnimator.Play(animToPlay);
    }

    public void ResetCamera()
    {
        cameraTransform_3D.rotation = Quaternion.Lerp(cameraTransform_3D.rotation, startingRotation_3D, 1.8f * Time.deltaTime);
    }

    public void ShowSettingsMenu(int state)
    {
        _mainMenuRef.SettingsMenu(state);
    }
}
