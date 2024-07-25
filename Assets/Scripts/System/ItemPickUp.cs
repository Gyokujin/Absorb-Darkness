using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem weapon;

    public override void Interact(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        base.Interact(playerManager, playerBehavior);
        PickUpItem(playerManager, playerBehavior);
    }

    void PickUpItem(PlayerManager playerManager, PlayerBehavior playerBehavior)
    {
        playerManager.playerAnimator.PlayTargetAnimation("Pick Up", true);
        playerManager.playerInventory.weaponsInventory.Add(weapon);

        UIManager.instance.OpenItemPopUpUI(weapon.itemName, weapon.itemIcon.texture);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}