using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem weapon;

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        base.Interact(player, playerBehavior);
        PickUpItem(player, playerBehavior);
    }

    void PickUpItem(PlayerManager player, PlayerBehavior playerBehavior)
    {
        player.playerAnimator.PlayTargetAnimation("PickUp", true);
        player.playerInventory.weaponsInventory.Add(weapon);

        UIManager.instance.OpenItemPopUpUI(weapon.itemName, weapon.itemIcon.texture);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}