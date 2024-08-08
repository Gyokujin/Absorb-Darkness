using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossClearUI : MonoBehaviour
{
    private Animator animator;

    [HideInInspector]
    public BossItemDrop bossItemDrop;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayBossClear()
    {
        animator.enabled = true;
    }

    public void ItemLootEvent()
    {
        bossItemDrop.ItemLoot();
    }
}