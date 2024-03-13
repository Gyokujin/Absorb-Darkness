using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int healthLevel = 10;
    [SerializeField]
    private int healthLevelAmount = 10;
    public int maxHealth;
    public int currentHealth;

    [Header("Component")]
    [SerializeField]
    private HealthBar healthBar;
    private PlayerAnimator playerAnimator;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
        InitHealth();
    }

    void InitHealth()
    {
        maxHealth = SetMaxHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    int SetMaxHealthLevel()
    {
        maxHealth = healthLevel * healthLevelAmount;
        return maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimator.PlayTargetAnimation("Damage01", true);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            playerAnimator.PlayTargetAnimation("Dead01", true);
        }
    }
}