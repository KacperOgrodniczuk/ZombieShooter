using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class PlayerInteractionManager : MonoBehaviour
{
    public PlayerManager playerManager { get; private set; }

    [Header("Interaction Settings")]
    public float interactRange = 1;

    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;
    private string prompt = "";


    private void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
    }

    void Update()
    {
        DetectInteratables();    

        UpdateUIPrompt();

        //if press button and not performing action interact with a thing
        if (PlayerInputManager.instance.interactInput && currentInteractable != null)
        {
            currentInteractable.Interact(playerManager);
        }
    }

    void DetectInteratables()
    {
        Ray ray = new Ray(PlayerCameraManager.instance.mainCamera.transform.position, PlayerCameraManager.instance.mainCamera.transform.forward);
        RaycastHit hit;

        //Debug
        Debug.DrawRay(PlayerCameraManager.instance.mainCamera.transform.position, PlayerCameraManager.instance.mainCamera.transform.forward * interactRange, Color.red);


        currentInteractable = null;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
    }

    void UpdateUIPrompt()
    {
        if (currentInteractable != null)
            prompt = currentInteractable.GetInteractionPrompt();
        else
            prompt = "";

        playerManager.UIManager.UpdateInteractPromptUI(prompt);
    }

}
