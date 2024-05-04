using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    [Header("Stamina")]
    public int staminaLevel = 10;
    public int staminaLevelAmount = 10;
    public float maxStamina;
    private float currentStamina;
    [HideInInspector]
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            if (value > 0)
            {
                currentStamina = value;
            }
            else
            {
                currentStamina = 0;
            }
        }
    }
    public int actionLimitStamina = 5; // 구르기, 백스텝, 공격 액션을 위해 필요한 최소한의 스테미나. 해당값 이상이면 액션 사용이 가능하다.
    [SerializeField]
    private float staminaRecoveryAmount = 0.1f;
    public float rollingStaminaAmount = 15;
    public float backStapStaminaAmount = 8;
    [SerializeField]
    private float sprintStaminaAmount = 0.1f;

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
        if (playerManager.onDie)
            return;

        gameObject.layer = Invincible();
        
    }

    void FixedUpdate()
    {
        if (curInvincibleTime > 0)
        {
            curInvincibleTime -= Time.deltaTime;
        }

        if (playerManager.isSprinting)
        {
            TakeStamina(sprintStaminaAmount);
        }

        RecoveryStamina();
    }

    int Invincible() 
    {
        playerManager.onDamage = playerAnimator.animator.GetBool("onDamage");
        int curLayer = playerManager.defaultLayer;

        if (playerManager.onDodge || playerManager.onDamage || playerManager.onDie || curInvincibleTime > 0)
        {
            curLayer = playerManager.invincibleLayer;
        }

        return curLayer;
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
        CurrentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    public void TakeDamage(int damage, bool hitStun)
    {
        if (playerManager.onDie)
            return;
        
        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);
        AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Hit]);

        if (hitStun)
        {
            playerManager.onDamage = true;
            gameObject.layer = playerManager.invincibleLayer;
            playerAnimator.animator.SetBool("onDamage", true);
            playerAnimator.PlayTargetAnimation("Damage", true);
        }

        if (currentHealth <= 0)
        {
            DieProcess();
        }
        else
        {
            curInvincibleTime += invincibleTime;
        }
    }

    void DieProcess()
    {
        playerManager.onDie = true;
        gameObject.layer = playerManager.invincibleLayer;
        currentHealth = 0;
        playerAnimator.PlayTargetAnimation("Dead", true);
        AudioManager.instance.PlayPlayerActionSFX(AudioManager.instance.playerActionClips[(int)PlayerActionSound.Die]);
    }

    void RecoveryStamina()
    {
        if (CurrentStamina < maxStamina && !playerManager.isInteracting && !playerManager.onDamage && !playerManager.onDodge && !playerManager.isSprinting)
        {
            CurrentStamina += staminaRecoveryAmount;
            staminaBar.SetCurrentStamina(CurrentStamina);
        }
    }

    public void TakeStamina(float amount)
    {
        CurrentStamina -= amount;
        staminaBar.SetCurrentStamina(CurrentStamina);
    }
}