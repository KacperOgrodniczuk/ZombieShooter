using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(DissolveEffect))]
[RequireComponent(typeof(NavMeshObstacle))]
public class PurchasableDoor : Purchasable
{
    private DissolveEffect _dissolveEffect;

    private NavMeshObstacle _obstacle;

    private void OnEnable()
    {
        if (_dissolveEffect == null)
            _dissolveEffect = GetComponent<DissolveEffect>();

        _dissolveEffect.onDissolveComplete += OpenDoors;

        _obstacle = GetComponent<NavMeshObstacle>();
    }

    public override string GetInteractionPrompt()
    {
        return purchased ? "" : "Cost: " + cost.ToString() + " points";
    }

    protected override void OnPurchase(PlayerManager player = null)
    {
        //Play some particle/visual effects
        _dissolveEffect.StartDissolve();
    }

    void OpenDoors()
    {
        //disable/delete the door object.
        _obstacle.carving = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _dissolveEffect.onDissolveComplete -= OpenDoors;
    }
}
