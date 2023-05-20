using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //[Header("General Attributes")]
    [SerializeField] public enum PlayerState { is2D, is3D, isDead };
    [SerializeField] public PlayerState PlayerCurrentState;

    private MainMenuManager _mainMenuRef;

    [Header("3D Attributes")]
    [SerializeField] private Transform cameraTransform_3D;

    private Vector2 playerInput_3D;
    private Vector3 rotateValue_3D;

    private Quaternion startingRotation_3D;

    #region 2D ATTRIBUTES
    [Header("2D Attributes")]
    
    [SerializeField] public GameObject PlayerGameObject_2D;
    [SerializeField, Range(0f, 100f)] private float maxSpeed_2D;
    [SerializeField, Range(0f, 100f)] private float maxAcceleration_2D;
    [SerializeField, Range(0f, 100f)] private float maxAirAcceleration_2D;

    [SerializeField, Range(0f, 100f)] private float jumpHeight_2D;
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
    private int jumpPhase_2D;
    #endregion

    void Start()
    {
        if (_mainMenuRef == null) { _mainMenuRef = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>(); }
        if (_mainMenuRef.CurrentState != MainMenuManager.MenuStates.game) { _mainMenuRef.CurrentState = MainMenuManager.MenuStates.game; }

        PlayerCurrentState = PlayerState.is3D;
        startingRotation_3D = cameraTransform_3D.localRotation;

        if (PlayerGameObject_2D == null) { PlayerGameObject_2D = GameObject.FindGameObjectWithTag("Player2D"); }
        if (playerBody_2D == null) { playerBody_2D = PlayerGameObject_2D.GetComponent<Rigidbody2D>(); }
        if (environmentGround_2D == null) { environmentGround_2D = PlayerGameObject_2D.GetComponent<Ground>(); }
    }

    void Update()
    {
        switch (PlayerCurrentState)
        {
            case PlayerState.is2D:
                Handle2DInput();
                ResetCamera();
                break;
            case PlayerState.is3D:
                HandleRotation();
                Handle3DInput();
                break;
        }
        
        if (Input.GetKeyDown(KeyCode.Escape)) { ShowSettingsMenu(1); }
    }

    private void FixedUpdate()
    {
        if (PlayerCurrentState == PlayerState.is2D)
        {
            _onGround_2D = environmentGround_2D.OnGround;
            if (_onGround_2D) { jumpPhase_2D = 0; }

            velocity_2D = playerBody_2D.velocity;
            acceleration_2D = environmentGround_2D ? maxAcceleration_2D : maxAirAcceleration_2D;
            maxSpeedChange_2D = acceleration_2D * Time.deltaTime;
            velocity_2D.x = Mathf.MoveTowards(velocity_2D.x, desiredVelocity_2D.x, maxSpeedChange_2D);

            if (playerBody_2D.velocity.y > 0) { playerBody_2D.gravityScale = upwardMultiplier_2D; }
            else if (playerBody_2D.velocity.y > 0) { playerBody_2D.gravityScale = downwardMultiplier_2D; }
            else if (playerBody_2D.velocity.y == 0) { playerBody_2D.gravityScale = defaultGravityScale_2D; }



            playerBody_2D.velocity = velocity_2D;
        }
        else { return; }
    }

    private void HandleRotation()
    {
        playerInput_3D.y = Input.GetAxis("Mouse X");
        playerInput_3D.x = Input.GetAxis("Mouse Y");

        rotateValue_3D = new Vector3(playerInput_3D.x, playerInput_3D.y * -1, 0);
        cameraTransform_3D.eulerAngles = cameraTransform_3D.eulerAngles - rotateValue_3D;
    }

    public void Handle2DInput()
    {
        playerInput_2D.x = Input.GetAxisRaw("Horizontal");

        playerDir_2D.x = playerInput_2D.x;
        desiredVelocity_2D = new Vector2(playerDir_2D.x, 0f) * Mathf.Max(maxSpeed_2D - environmentGround_2D.Friction, 0f);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (_onGround_2D || jumpPhase_2D < maxAirJumps_2D)
            {
                jumpPhase_2D++;
                float _jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpHeight_2D);
                if (velocity_2D.y > 0)
                {
                    _jumpSpeed = Mathf.Max(_jumpSpeed - velocity_2D.y, 0f);
                }

                Debug.Log(_jumpSpeed);
                velocity_2D.y += _jumpSpeed;
            }
        }
    }

    public void Handle3DInput()
    {

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
