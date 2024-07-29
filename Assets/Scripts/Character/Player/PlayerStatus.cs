using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class PlayerStatus : CharacterStatus
{
    private PlayerManager player;
    private PlayerStatusData playerStatusData;
    private PlayerAnimatorData playerAnimatorData;

    [Header("Stamina")]
    private float maxStamina;
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

    // public int actionLimitStamina = 5; // 구르기, 백스텝, 공격 액션을 위해 필요한 최소한의 스테미나. 해당값 이상이면 액션 사용이 가능하다.
    [SerializeField]
    private float staminaRecoveryAmount = 0.1f;
    public float rollingStaminaAmount = 15;
    public float backStapStaminaAmount = 8;
    [SerializeField]
    private float sprintStaminaAmount = 0.1f;

    [Header("Hit")]
    public float invincibleTime;
    private float curInvincibleTime;

    [Header("Recovery")]
    [SerializeField]
    private int recoveryAmount = 50;

    [Header("Move")]
    public float sprintSpeed = 7;

    [Header("UI")]
    [SerializeField]
    private HealthBar healthBar;
    [SerializeField]
    private StaminaBar staminaBar;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponent<PlayerManager>();
        playerStatusData = new PlayerStatusData();
        playerAnimatorData = new PlayerAnimatorData();

        InitHealth();
        InitStamina();
    }

    void Update() 
    {
        if (player.onDie)
            return;

        gameObject.layer = Invincible();
    }

    void FixedUpdate()
    {
        if (curInvincibleTime > 0)
        {
            curInvincibleTime -= Time.deltaTime;
        }

        if (player.isSprinting)
        {
            TakeStamina(sprintStaminaAmount);
        }

        RecoveryStamina();
    }

    int Invincible() 
    {
        player.onDamage = player.playerAnimator.animator.GetBool(playerAnimatorData.OnDamageParameter);
        int curLayer = player.defaultLayer;

        if (player.isDodge || player.onDamage || player.onDie || curInvincibleTime > 0)
        {
            curLayer = player.invincibleLayer;
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
        maxStamina = playerStatusData.StaminaLevel * playerStatusData.StaminaLevelAmount;
        CurrentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    public void TakeDamage(int damage, bool hitStun)
    {
        if (player.onDie)
            return;

        if (player.playerBehavior.curUsingItem != null)
        {
            player.playerBehavior.EndItemUse();
        }

        currentHealth -= damage;
        healthBar.SetCurrentHealth(currentHealth);

        if (hitStun)
        {
            player.onDamage = true;
            gameObject.layer = player.invincibleLayer;
            player.playerAnimator.animator.SetBool(playerAnimatorData.OnDamageParameter, true);
            player.playerAnimator.PlayTargetAnimation(playerAnimatorData.DamageAnimation, true);

            GameObject hitEffect = PoolManager.instance.GetEffect((int)PoolManager.Effect.HitBlood);
            hitEffect.transform.position = effectTransform.position;
            player.playerAudio.PlaySFX(player.playerAudio.characterClips[(int)CharacterAudio.CharacterSound.Hit]);
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
        player.onDie = true;
        gameObject.layer = player.invincibleLayer;
        player.playerAnimator.PlayTargetAnimation(playerAnimatorData.DeadAnimation, true);
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)CharacterAudio.CharacterSound.Die]);
    }

    public void RecoveryHealth()
    {
        currentHealth = Mathf.Min(currentHealth + recoveryAmount, maxHealth);
        healthBar.SetCurrentHealth(currentHealth);

        GameObject estusEffect = PoolManager.instance.GetEffect((int)PoolManager.Effect.EstusEffect);
        estusEffect.transform.position = effectTransform.position;
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Recovery]);
    }

    void RecoveryStamina()
    {
        if (CurrentStamina < maxStamina && !player.isInteracting && !player.onDamage && !player.isDodge && !player.isSprinting)
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