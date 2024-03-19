using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickUp : Interactable
{
    public WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
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
        playerAnimator.PlayTargetAnimation("Pick Up Item", true);
        playerInventory.weaponsInventory.Add(weapon);
    }
}