using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    private bool unlock = false;
    private Animator animator;

    [SerializeField]
    private int lockNum;
    [SerializeField]
    private Transform interactTrasnform;

    [SerializeField]
    private string successMessage;
    [SerializeField]
    private string failMessage;

    void Start() // �θ� Ŭ������ Awake�� ���� �и��Ѵ�.
    {
        Init();
    }

    void Init()
    {
        animator = GetComponent<Animator>();
    }

    public override void Interact(PlayerManager player, PlayerBehavior playerBehavior)
    {
        if (!unlock)
        {
            base.Interact(player, playerBehavior);
            Unlock(player);
        }
    }

    void Unlock(PlayerManager player)
    {
        // �÷��̾��� InteractItemInventory�� �����Ͽ� lockNum�� ��ġ�ϴ� ���谡 �ִ��� Ȯ���Ѵ�.
        if (KeyCheck())
        {
            unlock = true;
            animator.SetTrigger("doUnlock");

            player.transform.position = interactTrasnform.position;
            player.transform.rotation = interactTrasnform.rotation;
            player.playerAnimator.PlayTargetAnimation("DoorOpen", true);
            UIManager.instance.OpenMessagePopUpUI(false, successMessage);
        }
        else
        {
            UIManager.instance.OpenMessagePopUpUI(false, failMessage);
        }
    }

    bool KeyCheck()
    {
        bool isHaving = false;

        for (int i = 0; i < InventoryManager.instance.interactItems.Count; i++)
        {
            InteractItem item = InventoryManager.instance.interactItems[i];

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