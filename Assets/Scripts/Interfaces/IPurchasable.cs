using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPurchasable
{
    int Cost { get; }
    void TryPurchase(PlayerManager player);
}
