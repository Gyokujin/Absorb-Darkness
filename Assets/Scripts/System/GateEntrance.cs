using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateEntrance : Interactable
{
    private int closeTag = 0;
    private new Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public override void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        base.Interact(playerManager, playerInteract);
        Entrance(playerManager, playerInteract);
    }

    void Entrance(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        collider.isTrigger = true;
        playerManager.playerAnimator.PlayTargetAnimation("Entrance", true);
    }

    public void Close()
    {
        collider.isTrigger = false;
        gameObject.tag = "Untagged";
    }
}