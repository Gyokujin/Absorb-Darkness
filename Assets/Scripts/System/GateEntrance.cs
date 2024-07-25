using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateEntrance : Interactable
{
    private new Collider collider;

    void Start()
    {
        collider = GetComponent<Collider>();
    }

    public override void Interact(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        base.Interact(playerManager, playerBehavior);
        Entrance(playerManager, playerBehavior);
    }

    void Entrance(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        collider.isTrigger = true;
        playerManager.playerAnimator.PlayTargetAnimation("Entrance", true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.FogEntrance]);
        // AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.FogEntrance]);
    }

    public void Close()
    {
        collider.isTrigger = false;
        gameObject.tag = "Untagged";
    }
}