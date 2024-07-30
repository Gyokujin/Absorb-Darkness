using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWallEntrance : Interactable
{
    private new Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        Entrance(player, playerBehavior);
    }

    void Entrance(PlayerManager player, PlayerBehavior playerBehavior)
    {
        collider.isTrigger = true;
        player.playerAnimator.PlayTargetAnimation("Entrance", true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.FogEntrance]);
    }

    public void Close()
    {
        collider.isTrigger = false;
        gameObject.tag = "Untagged";
    }
}