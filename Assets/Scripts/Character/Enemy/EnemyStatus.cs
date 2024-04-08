using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    [Header("Detection")]
    public float detectionRadius = 20;
    public float detectionAngleMax = 50;
    public float detectionAngleMin = -50;
    public float viewableAngle;

    [Header("Attack")]
    public float attackRangeMax = 1.5f;

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