using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : CharacterStatus
{
    private PlayerManager player;

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
        InitHealth();
        InitStamina();
    }

    void InitHealth()
    {
        maxHealth = player.playerStatusData.HealthLevel * player.playerStatusData.HealthLevelAmount;
        CurrentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void InitStamina()
    {
        maxStamina = player.playerStatusData.StaminaLevel * player.playerStatusData.StaminaLevelAmount;
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
            TakeStamina(player.playerStatusData.SprintStaminaAmount);

        RecoveryStamina();
    }

    int Invincible() 
    {
        player.onDamage = player.playerAnimator.animator.GetBool(player.characterAnimatorData.OnDamageParameter);
        int curLayer = player.playerCombat.defaultLayer;

        if (player.isDodge || player.onDamage || player.onDie || curInvincibleTime > 0)
            curLayer = player.playerCombat.invincibleLayer;

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
            gameObject.layer = player.playerCombat.invincibleLayer;
            player.playerAnimator.animator.SetBool(player.characterAnimatorData.OnDamageParameter, true);
            player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.HitAnimation, true);

            GameObject hitEffect = PoolManager.instance.GetEffect((int)PoolManager.Effect.HitBlood);
            hitEffect.transform.position = effectTransform.position;
            player.playerAudio.PlaySFX(player.playerAudio.characterClips[(int)CharacterAudio.CharacterSound.Hit]);
        }

        if (CurrentHealth <= 0)
            DieProcess();
        else
            curInvincibleTime += player.playerPhysicsData.InvincibleTime;
    }

    void DieProcess()
    {
        player.onDie = true;
        gameObject.layer = player.playerCombat.invincibleLayer;
        player.playerAnimator.PlayTargetAnimation(player.characterAnimatorData.DeadAnimation, true);
        player.playerAudio.PlaySFX(player.playerAudio.characterClips[(int)CharacterAudio.CharacterSound.Dead]);
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
            CurrentStamina += player.playerStatusData.StaminaRecoveryAmount;
            staminaBar.SetCurrentStamina(CurrentStamina);
        }
    }

    public void TakeStamina(float amount)
    {
        CurrentStamina -= amount;
        staminaBar.SetCurrentStamina(CurrentStamina);
    }
}