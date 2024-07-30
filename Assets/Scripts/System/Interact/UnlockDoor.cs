using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    public override void Interact(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        base.Interact(playerManager, playerBehavior);
        Unlock(playerManager, playerBehavior);
    }

    void Unlock(PlayerManager player, PlayerBehavior playerBehavior)
    {

    }
}