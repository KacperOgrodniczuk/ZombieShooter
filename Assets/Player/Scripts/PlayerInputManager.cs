using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;

    PlayerControls playerControls;

    public Vector2 movementInput;
    public Vector2 mouseInput;
    public bool leftClick;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();      //Gotta make sure player contols exist and then pass some values baby

            //value passthrough
            playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
            playerControls.PlayerCamera.MouseDelta.performed += i => mouseInput = i.ReadValue<Vector2>();

            playerControls.PlayerActions.LeftClick.performed += i => leftClick = i.ReadValueAsButton();
        }

        playerControls.Enable();
    }
}
