using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] public enum PlayerState { is2D, is3D, isDead };
    [SerializeField] public PlayerState PlayerCurrentState;

    private MainMenuManager _mainMenuRef;

    [SerializeField] private Transform cameraTransform_3D;

    private Vector2 playerInput_3D;
    private Vector3 rotateValue_3D;

    private Quaternion startingRotation_3D;

    void Start()
    {
        //if (_mainMenuRef == null) { _mainMenuRef = GameObject.FindGameObjectWithTag("MainMenu").GetComponent<MainMenuManager>(); } IMPLEMENT ONCE GAME COMPLETE
        PlayerCurrentState = PlayerState.is3D;

        startingRotation_3D = cameraTransform_3D.localRotation;
    }

    void Update()
    {
        switch (PlayerCurrentState)
        {
            case PlayerState.is2D:
                break;
            case PlayerState.is3D:
                HandleRotation();
                break;
        }
        //if (Input.GetKeyDown(KeyCode.Escape)) { ShowSettingsMenu(1); } IMPLEMENT ONCE GAME COMPLETE
    }

    private void HandleRotation()
    {
        playerInput_3D.y = Input.GetAxis("Mouse X");
        playerInput_3D.x = Input.GetAxis("Mouse Y");

        rotateValue_3D = new Vector3(playerInput_3D.x, playerInput_3D.y * -1, 0);
        cameraTransform_3D.eulerAngles = cameraTransform_3D.eulerAngles - rotateValue_3D;
    }


    public void ShowSettingsMenu(int state)
    {
        _mainMenuRef.SettingsMenu(state);
    }
}
