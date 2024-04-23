using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    [Header("Stamina")]
    public int staminaLevel = 10;
    public int staminaLevelAmount = 10;
    public int maxStamina;
    public int currentStamina;

    [Header("Hit")]
    public float invincibleTime;
    private float curInvincibleTime;

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

    void Update()
    {
        if (curInvincibleTime > 0)
        {
            curInvincibleTime -= Time.deltaTime;
            gameObject.layer = playerManager.invincibleLayer;
        }
        else if (!playerManager.onDodge)
        {
            gameObject.layer = playerManager.defaultLayer;
        }
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

    public void TakeDamage(int damage, bool hitStun)
    {
        if (playerManager.onDie)
            return;

        gameObject.layer = playerManager.invincibleLayer;
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);

        if (hitStun)
        {
            playerAnimator.PlayTargetAnimation("Damage", true);
        }

        if (currentHealth <= 0)
        {
            playerManager.onDie = true;
            currentHealth = 0;
            playerAnimator.PlayTargetAnimation("Dead", true);
        }
        else
        {
            curInvincibleTime += invincibleTime;
        }
    }

    public void TakeStamina(int amount)
    {
        currentStamina -= amount;
        staminaBar.SetCurrentStamina(currentStamina);
    }
}