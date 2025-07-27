using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PurchasableDoor : MonoBehaviour, IPurchasable
{
    [SerializeField] private int cost = 500;

    public int Cost => cost;
    Animator animator;

    void Start()
    { 
        
    }

    public void TryPurchase(PlayerManager player)
    {
        if (!player.PlayerSurvivalPointsManager.TrySpendSurvivalPoints(cost))
            return;
 
    
        //TODO; copy the zombie death code
        //Dissolve the object.
        //Play some particle/visual effects
    }
}
