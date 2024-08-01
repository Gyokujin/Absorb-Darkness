using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponItemPickUp : Interactable
{
    [SerializeField]
    private WeaponItem dropWeaponItem;

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        PickUpWeaponItem(player, playerBehavior);
    }

    void PickUpWeaponItem(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        InventoryManager.instance.GetWeaponItem(dropWeaponItem);
        UIManager.instance.OpenItemPopUpUI(dropWeaponItem.itemName, dropWeaponItem.itemIcon.texture);

        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}