using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int healthLevel = 10;
    [SerializeField]
    private int healthLevelAmount = 10;
    private int maxHealth;
    private int currentHealth;

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