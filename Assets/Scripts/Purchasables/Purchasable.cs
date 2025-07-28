using UnityEngine;

public abstract class Purchasable : MonoBehaviour, IInteractable
{
    [SerializeField] protected int cost = 500; 
    protected bool purchased = false;


    public abstract string GetInteractionPrompt();

    public virtual void Interact(PlayerManager player)
    {
        TryPurchase(player);
    }

    void TryPurchase(PlayerManager player)
    {
        if (purchased) //if the door is already bought you can't buy it again.
            return;

        if (!player.PlayerSurvivalPointsManager.TrySpendSurvivalPoints(cost))
            return;

        purchased = true;

        OnPurchase();
    }

    protected abstract void OnPurchase();
}
