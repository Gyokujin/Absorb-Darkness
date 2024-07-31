using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    private bool unlock = false;
    private Animator animator;

    [SerializeField]
    private Transform interactTrasnform;

    void Start() // 부모 클래스의 Awake를 위해 분리한다.
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
        unlock = true;
        animator.SetTrigger("doUnlock");

        player.transform.position = interactTrasnform.position;
        player.transform.rotation = interactTrasnform.rotation;
        player.playerAnimator.PlayTargetAnimation("DoorOpen", true);
    }
}