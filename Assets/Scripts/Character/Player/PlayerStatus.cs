using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    [Header("Move")]
    public float sprintSpeed = 7;

    [Header("UI")]
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private StaminaBar staminaBar;

    [Header("Component")]
    private PlayerManager playerManager;
    private PlayerAnimator playerAnimator;

    void Awake()
    {
        playerManager = GetComponent<PlayerManager>();
        playerAnimator = GetComponentInChildren<PlayerAnimator>();
    }

    void Start()
    {
        InitHealth();
        InitStamina();
    }

    void InitHealth()
    {
        maxHealth = healthLevel * healthLevelAmount;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void InitStamina()
    {
        maxStamina = staminaLevel * staminaLevelAmount;
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    public void TakeDamage(int damage)
    {
        if (playerManager.onDie)
            return;

        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimator.PlayTargetAnimation("Damage", true);

        if (currentHealth <= 0)
        {
            playerManager.onDie = true;
            currentHealth = 0;
            playerAnimator.PlayTargetAnimation("Dead", true);
        }
    }

    public void TakeStamina(int amount)
    {
        currentStamina -= amount;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}