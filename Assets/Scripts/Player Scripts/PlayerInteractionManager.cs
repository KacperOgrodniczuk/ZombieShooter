using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float interactRange;

    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentInteractable;

    void Update()
    {
        DetectInteratables();    
    }

    void DetectInteratables()
    {
        Ray ray = new Ray(PlayerCameraManager.instance.camera.transform.position, PlayerCameraManager.instance.camera.transform.forward);
        RaycastHit hit;

        currentInteractable = null;

        if (Physics.Raycast(ray, out hit, interactRange, interactableLayer))
        {
            currentInteractable = hit.collider.GetComponent<IInteractable>();
        }
    }
}
