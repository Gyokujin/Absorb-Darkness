using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;
using SystemData;

public class FogWallEntrance : Interactable
{
    [Header("Data")]
    private PlayerAnimatorData playerAnimatorData;
    private GameObjectData gameObjectData;

    [Header("GameObject Tag")]
    private string untaggedTag;

    [Header("Component")]
    private new Collider collider;
    private new ParticleSystem particleSystem;

    protected override void Init()
    {
        base.Init();
        untaggedTag = gameObjectData.UntaggedTag;

        collider = GetComponent<Collider>();
        particleSystem = GetComponent<ParticleSystem>();
    }

    public override void Interact(PlayerManager player)
    {
        base.Interact(player);
        Entrance(player);
    }

    void Entrance(PlayerManager player)
    {
        collider.isTrigger = true;
        player.playerAnimator.PlayTargetAnimation(playerAnimatorData.EntranceAnimation, true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.FogEntrance]);
    }

    public void CloseGate()
    {
        collider.isTrigger = false;
        gameObject.tag = untaggedTag;
    }

    public void DisappearGate()
    {
        collider.isTrigger = true;
        particleSystem.Stop();
    }
}