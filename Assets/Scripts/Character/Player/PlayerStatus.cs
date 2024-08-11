using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerData;

public class PlayerStatus : CharacterStatus
{
    private PlayerManager player;

    [Header("Data")]
    private PlayerStatusData playerStatusData;
    private PlayerPhysicsData playerPhysicsData;
    private PlayerAnimatorData playerAnimatorData;

    [Header("Stamina")]
    private float maxStamina;
    private float currentStamina;
    public float CurrentStamina
    {
        get
        {
            return currentStamina;
        }
        set
        {
            if (value > 0)
                currentStamina = value;
            else
                currentStamina = 0;
        }
    }

    [Header("Battle")]
    private float curInvincibleTime;

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
        playerPhysicsData = new PlayerPhysicsData();
        playerAnimatorData = new PlayerAnimatorData();

        InitHealth();
        InitStamina();
    }

    void InitHealth()
    {
        maxHealth = playerStatusData.HealthLevel * playerStatusData.HealthLevelAmount;
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void InitStamina()
    {
        maxStamina = playerStatusData.StaminaLevel * playerStatusData.StaminaLevelAmount;
        CurrentStamina = maxStamina;
        staminaBar.SetMaxStamina(maxStamina);
    }

    void Update() 
    {
        InvincibleCheck();
        StaminaCheck();
    }

    void InvincibleCheck()
    {
        if (player.onDie)
            return;

        if (curInvincibleTime > 0)
            curInvincibleTime -= Time.deltaTime;
        
        gameObject.layer = Invincible();
    }

    void StaminaCheck()
    {
        if (player.isSprinting)
            TakeStamina(playerStatusData.SprintStaminaAmount);

        RecoveryStamina();
    }

    int Invincible() 
    {
        player.onDamage = player.playerAnimator.animator.GetBool(playerAnimatorData.OnDamageParameter);
        int curLayer = player.defaultLayer;

        if (player.isDodge || player.onDamage || player.onDie || curInvincibleTime > 0)
            curLayer = player.invincibleLayer;

        return curLayer;
    }

    public void TakeDamage(int damage, bool hitStun)
    {
        if (player.onDie)
            return;

        if (player.playerBehavior.curUsingItem != null)
            player.playerBehavior.EndItemUse();

        CurrentHealth -= damage;
        healthBar.SetCurrentHealth(CurrentHealth);

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

        if (CurrentHealth <= 0)
            DieProcess();
        else
            curInvincibleTime += playerPhysicsData.InvincibleTime;
    }

    void DieProcess()
    {
        player.onDie = true;
        gameObject.layer = player.invincibleLayer;
        player.playerAnimator.PlayTargetAnimation(playerAnimatorData.DeadAnimation, true);
        player.playerAudio.PlaySFX(player.playerAudio.characterClips[(int)CharacterAudio.CharacterSound.Die]);
    }

    public void RecoveryHP(int amount)
    {
        CurrentHealth = Mathf.Min(CurrentHealth + amount, maxHealth);
        healthBar.SetCurrentHealth(CurrentHealth);

        GameObject estusEffect = PoolManager.instance.GetEffect((int)PoolManager.Effect.EstusEffect);
        estusEffect.transform.position = effectTransform.position;
        player.playerAudio.PlaySFX(player.playerAudio.playerClips[(int)PlayerAudio.PlayerSound.Recovery]);
    }

    void RecoveryStamina()
    {
        if (player.isInteracting || player.onDamage || player.isDodge || player.isSprinting)
            return;

        if (CurrentStamina < maxStamina)
        {
            CurrentStamina += playerStatusData.StaminaRecoveryAmount;
            staminaBar.SetCurrentStamina(CurrentStamina);
        }
    }

    public void TakeStamina(float amount)
    {
        CurrentStamina -= amount;
        staminaBar.SetCurrentStamina(CurrentStamina);
    }
}