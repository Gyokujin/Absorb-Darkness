using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("Interact")]
    [SerializeField]
    private float checkRadius = 0.3f;
    [SerializeField]
    private float checkMaxDis = 1f;
    public Interactable itemInteractableObj;

    [Header("Component")]
    private PlayerInput playerInput;
    private PlayerCamera playerCamera;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerCamera = FindObjectOfType<PlayerCamera>();
    }

    public void CheckInteractableObject(PlayerManager playerManager)
    {
        RaycastHit hit;

        if (Physics.SphereCast(transform.position, checkRadius, transform.forward, out hit, checkMaxDis, playerCamera.targetLayer))
        {
            if (hit.collider.tag == "Interactable" && hit.collider.GetComponent<Interactable>() != null)
            {
                itemInteractableObj = hit.collider.GetComponent<Interactable>();
                UIManager.instance.OpenInteractUI(itemInteractableObj.interactableText);

                if (playerInput.interactInput)
                {
                    itemInteractableObj.Interact(playerManager, this);

                    if (itemInteractableObj.interactType == InteractType.Item)
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

                if (itemInteractableObj != null && playerInput.interactInput)
                {
                    switch (itemInteractableObj.interactType)
                    {
                        case InteractType.Item:
                            UIManager.instance.CloseItemPopUpUI();
                            break;

                        case InteractType.Message:
                            UIManager.instance.CloseMessagePopUpUI();
                            break;
                    }

                    AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)SystemSound.Interact2]);
                    itemInteractableObj = null;
                }
            }
        }
    }
}