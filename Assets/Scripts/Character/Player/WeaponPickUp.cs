using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickUp : Interactable
{
    [Header("Weapon")]
    [SerializeField]
    private WeaponItem weapon;

    public override void Interact(PlayerManager playerManager)
    {
        base.Interact(playerManager);
        PickUpItem(playerManager);
    }

    void PickUpItem(PlayerManager playerManager)
    {
        playerManager.playerMove.rigidbody.velocity = Vector3.zero; // ��ȣ�ۿ� ���� �̵��� ������.
        playerManager.playerAnimator.PlayTargetAnimation("Pick Up", true);
        playerManager.playerInventory.weaponsInventory.Add(weapon);
        playerManager.itemInteractableObj.GetComponentInChildren<Text>().text = weapon.itemName;
        playerManager.itemInteractableObj.GetComponentInChildren<RawImage>().texture = weapon.itemIcon.texture;
        playerManager.itemInteractableObj.SetActive(true);
        gameObject.SetActive(false);
    }
}