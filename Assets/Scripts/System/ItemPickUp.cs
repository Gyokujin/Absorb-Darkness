using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem weapon;

    public override void Interact(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        base.Interact(playerManager, playerInteract);
        PickUpItem(playerManager, playerInteract);
    }

    void PickUpItem(PlayerManager playerManager, PlayerInteract playerInteract)
    {
        playerManager.playerAnimator.PlayTargetAnimation("Pick Up", true);
        playerManager.playerInventory.weaponsInventory.Add(weapon);

        UIManager.instance.OpenItemPopUpUI(weapon.itemName, weapon.itemIcon.texture);
        gameObject.SetActive(false);
    }
}