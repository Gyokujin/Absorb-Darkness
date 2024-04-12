using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    [Header("Detection")]
    public float detectionRadius = 20;
    public float detectionAngleMax = 50;
    public float detectionAngleMin = -50;

    [Header("Attack")]
    public float attackRangeMax = 1.5f;
    public float attackDelayMin = 0.5f;
    public float attackDelayMax = 2f;

    [Header("Component")]
    private Animator animator;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    void Start()
    {
        maxHealth = SetMaxHealthLevel();
        currentHealth = maxHealth;
    }

    int SetMaxHealthLevel()
    {
        maxHealth = healthLevel * healthLevelAmount;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetBool("onHit", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animator.SetTrigger("doDie");
        }
    }
}