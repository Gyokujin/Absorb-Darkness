using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem weapon;

    public override void Interact(PlayerManager playerManager, PlayerAction playerInteract)
    {
        base.Interact(playerManager, playerInteract);
        PickUpItem(playerManager, playerInteract);
    }

    void PickUpItem(PlayerManager playerManager, PlayerAction playerInteract)
    {
        playerManager.playerAnimator.PlayTargetAnimation("Pick Up", true);
        playerManager.playerInventory.weaponsInventory.Add(weapon);

        UIManager.instance.OpenItemPopUpUI(weapon.itemName, weapon.itemIcon.texture);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.PickUp]);
        gameObject.SetActive(false);
    }
}