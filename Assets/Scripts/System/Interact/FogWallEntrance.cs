using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogWallEntrance : Interactable
{
    private new Collider collider;

    protected override void Init()
    {
        base.Init();
        collider = GetComponent<Collider>();
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        Entrance(player);
    }

    void Entrance(PlayerManager player)
    {
        collider.isTrigger = true;
        player.playerAnimator.PlayTargetAnimation("Entrance", true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.FogEntrance]);
    }

    public void CloseGate()
    {
        collider.isTrigger = false;
        gameObject.tag = "Untagged";
    }

    public void DisappearGate()
    {

    }
}