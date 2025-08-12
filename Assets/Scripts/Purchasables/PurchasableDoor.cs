using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DissolveEffect))]
public class PurchasableDoor : Purchasable
{
    private DissolveEffect _dissolveEffect;

    private void OnEnable()
    {
        if (_dissolveEffect == null)
            _dissolveEffect = GetComponent<DissolveEffect>();

        _dissolveEffect.onDissolveComplete += OpenDoors;
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
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _dissolveEffect.onDissolveComplete -= OpenDoors;
    }
}
