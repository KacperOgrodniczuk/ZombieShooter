using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// I don't even know if this needs to be a monobehaviour, will test this out some other time.
public class PlayerAnimationEvents : MonoBehaviour
{
    // A wrapper class to pass functions for animation events to.
    
    [SerializeField]
    private PlayerManager playerManager;


    public void EndReload()
    { 
        playerManager.PlayerActions.EndReload();
    }
}
