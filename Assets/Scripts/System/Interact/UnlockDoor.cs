using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnlockDoor : Interactable
{
    private bool unlock = false;
    private Animator animator;

    [SerializeField]
    private Transform interactTrasnform;

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
        unlock = true;
        animator.SetTrigger("doUnlock");

        player.transform.position = interactTrasnform.position;
        player.transform.rotation = interactTrasnform.rotation;
        player.playerAnimator.PlayTargetAnimation("DoorOpen", true);
    }
}