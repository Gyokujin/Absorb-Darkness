using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        PickUpItem(playerManager);
    }

    void PickUpItem(PlayerManager playerManager)
    {
        PlayerInventory playerInventory;
        PlayerMove playerMove;
        PlayerAnimator playerAnimator;

        playerInventory = playerManager.GetComponent<PlayerInventory>();
        playerMove = playerManager.GetComponent<PlayerMove>();
        playerAnimator = playerManager.GetComponentInChildren<PlayerAnimator>();

        playerMove.rigidbody.velocity = Vector3.zero; // 상호작용 전에 이동을 끝낸다.
        playerAnimator.PlayTargetAnimation("Pick Up", true);
        playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableObj.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableObj.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.itemInteractableObj.SetActive(true);
        gameObject.SetActive(false);
    }
}