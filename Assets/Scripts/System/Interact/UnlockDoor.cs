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

    void Start() // 부모 클래스의 Awake를 위해 분리한다.
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
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
        // 플레이어의 InteractItemInventory에 접근하여 lockNum과 일치하는 열쇠가 있는지 확인한다.
        if (KeyCheck(player))
        {
            unlock = true;
            animator.SetTrigger("doUnlock");
            gameObject.tag = "Untagged";

            player.transform.position = interactTrasnform.position;
            player.transform.rotation = interactTrasnform.rotation;
            player.playerAnimator.PlayTargetAnimation("DoorOpen", true);
            AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.DoorOpen]);
            // UIManager.instance.OpenMessagePopUpUI(false, successMessage);
        }
        else
        {
            // UIManager.instance.OpenMessagePopUpUI(false, failMessage);
        }
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