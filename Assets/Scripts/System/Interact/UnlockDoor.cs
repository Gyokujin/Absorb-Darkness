using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemData;
using PlayerData;

public class UnlockDoor : Interactable
{
    [Header("Data")]
    private PlayerAnimatorData playerAnimatorData;
    private GameObjectData gameObjectData;

    [Header("Unlock")]
    [SerializeField]
    private int lockNum;
    private bool unlock;
    [SerializeField]
    private Transform interactTrasnform;

    [Header("Component")]
    private Animator animator;

    void Start() // �θ� Ŭ������ Awake�� ���� �и��Ѵ�.
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        interactMesssage = UIManager.instance.messageUIData.LockDoorInteractText;
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerManager player)
    {
        if (!unlock)
        {
            base.Interact(player);
            Unlock(player);
        }
    }

    void Unlock(PlayerManager player)
    {
        // �÷��̾��� InteractItemInventory�� �����Ͽ� lockNum�� ��ġ�ϴ� ���谡 �ִ��� Ȯ���Ѵ�.
        if (KeyCheck(player))
        {
            unlock = true;
            animator.SetTrigger(gameObjectData.DoUnlockParameter);
            gameObject.tag = gameObjectData.UntaggedTag;

            player.transform.SetPositionAndRotation(interactTrasnform.position, interactTrasnform.rotation);
            player.playerAnimator.PlayTargetAnimation(playerAnimatorData.DoorOpenAnimation, true);
            AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.DoorOpen]);
            UIManager.instance.messageUI.UpdateGameSystem(UIManager.instance.messageUIData.UnlockDoorInteractSuccessText);
        }
        else
        {
            UIManager.instance.messageUI.UpdateGameSystem(UIManager.instance.messageUIData.UnlockDoorInteractFailText);
            AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Interact1]);
        }

        UIManager.instance.OpenMessageUI(MessageUI.MessageType.GameSystem);
    }

    bool KeyCheck(PlayerManager player)
    {
        bool isHaving = false;

        for (int i = 0; i < player.playerInventory.interactItems.Count; i++)
        {
            InteractItem item = player.playerInventory.interactItems[i];

            if (item.interactItemType == InteractItem.InteractItemType.Key)
            {
                if (item is Key key)
                {
                    if (key.keyNumber == lockNum)
                    {
                        isHaving = true;
                        break;
                    }
                }
            }
        }

        return isHaving;
    }
}