using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Status")]
    [SerializeField]
    private int healthLevel = 10;
    [SerializeField]
    private int healthLevelAmount = 10;
    public int maxHealth;
    public int currentHealth;

    [SerializeField]
    private int staminaLevel = 10;
    [SerializeField]
    private int staminaLevelAmount = 10;
    [SerializeField]
    private int maxStamina;
    public int currentStamina;

    [Header("UI")]
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private StaminaBar staminaBar;

    [Header("Component")]
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
        InitStamina();
    }

    void InitHealth()
    {
        maxHealth = SetMaxHealthLevel();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void InitStamina()
    {
        maxStamina = SetMaxStaminaLevel();
        currentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    int SetMaxHealthLevel()
    {
        maxHealth = healthLevel * healthLevelAmount;
        return maxHealth;
    }

    int SetMaxStaminaLevel()
    {
        maxStamina = staminaLevel * staminaLevelAmount;
        return maxStamina;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        playerAnimator.PlayTargetAnimation("Damage", true);

        if (currentHealth <= 0)
        {
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