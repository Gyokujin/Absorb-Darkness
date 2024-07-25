using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private PlayerManager player;

    [Header("Interact")]
    [SerializeField]
    private float checkRadius = 0.3f;
    [SerializeField]
    private float checkMaxDis = 1f;
    public Interactable itemInteractableObj;

    void Awake()
    {
        player = GetComponent<PlayerManager>();
    }

    public void CheckInteractableObject(PlayerManager playerManager)
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, checkRadius, transform.forward, out hit, checkMaxDis, PlayerCamera.instance.targetLayer))
        {
            if (hit.collider.tag == "Interactable" && hit.collider.GetComponent<Interactable>() != null)
            {
                itemInteractableObj = hit.collider.GetComponent<Interactable>();
                UIManager.instance.OpenInteractUI(itemInteractableObj.interactableText);

                if (player.playerInput.interactInput)
                {
                    itemInteractableObj.Interact(playerManager, this);

                    if (itemInteractableObj.interactType == Interactable.InteractType.Item)
                    {
                        UIManager.instance.InventoryUIUpdate();
                    }
                }
            }
        }
        else
        {
            if (UIManager.instance.itemPopUpUI != null)
            {
                UIManager.instance.CloseInteractUI();

                if (itemInteractableObj != null && player.playerInput.interactInput)
                {
                    UIManager.instance.CloseItemPopUpUI();
                    UIManager.instance.CloseMessagePopUpUI();

                    switch (itemInteractableObj.interactType)
                    {
                        case Interactable.InteractType.Item:
                            break;

                        case Interactable.InteractType.Message:
                            break;
                    }

                    AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Interact2]);
                    itemInteractableObj = null;
                }
            }
        }
    }
}