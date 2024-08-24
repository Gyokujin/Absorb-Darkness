using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    private EnemyManager enemy;

    [Header("Status")]
    public float detectionRadius = 20;
    public float detectionAngleMax = 50;
    public float detectionAngleMin = -50;
    public float attackRangeMax = 1.5f;
    public float currentRecoveryTime = 0;

    [Header("Damage")]
    [SerializeField]
    private float hitTime = 1f;
    [SerializeField]
    private float knockbackTime = 2.5f;
    [SerializeField]
    private float knockbackPower = 50;
    public int damageAmount; // 누적 데미지. 이 수치가 3/5 이상이 되면 넉백을 한다.

    [Header("Coroutine")]
    private WaitForSeconds hitWait;
    private WaitForSeconds knockbackWait;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemy = GetComponent<EnemyManager>();
        hitWait = new WaitForSeconds(hitTime);
        knockbackWait = new WaitForSeconds(knockbackTime);
    }

    void Start()
    {
        InitHealth();
    }

    protected override void InitHealth()
    {
        maxHealth = healthLevel * healthLevelAmount;
        CurrentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage, CharacterStatus player)
    {
        if (enemy.onDie)
            return;

        CurrentHealth -= damage;

        if (enemy.enemyType == EnemyManager.EnemyType.Boss)
            UIManager.instance.stageUI.BossHPUIModify(CurrentHealth, maxHealth);

        GameObject hitEffect = PoolManager.instance.GetEffect((int)PoolManager.Effect.HitBlood);
        hitEffect.transform.position = effectTransform.position;

        if (CurrentHealth <= 0)
            DieProcess();
        else
        {
            if (enemy.enemyType != EnemyManager.EnemyType.Boss)
            {
                StopCoroutine(nameof(DamageProcess));
                enemy.onDamage = true;
                damageAmount += damage;

                if (damageAmount >= maxHealth * enemy.physicsData.KnockbackLimit) // 한번에 최대 체력의 절반 이상의 피해가 들어오면 스매시를 실행
                    StartCoroutine(nameof(KnockbackProcess), player);
                else
                    StartCoroutine(nameof(DamageProcess), player);
            }
        }
    }

    IEnumerator DamageProcess(CharacterStatus player)
    {
        enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.HitAnimation, true);
        enemy.enemyAudio.PlaySFX(enemy.enemyAudio.characterClips[(int)CharacterAudio.CharacterSound.Hit]);

        yield return hitWait;
        enemy.currentTarget = player;
        enemy.isPreformingAction = false;
    }

    IEnumerator KnockbackProcess(CharacterStatus player)
    {
        Vector3 attackDir = Vector3.Normalize(transform.position - player.transform.position); // 몬스터 넉백
        attackDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-attackDir);
        enemy.rigidbody.velocity = Vector3.zero;
        enemy.rigidbody.AddForce(attackDir * knockbackPower, ForceMode.Impulse);

        enemy.enemyAnimator.animator.SetTrigger(enemy.characterAnimatorData.KnockbackParameter);
        enemy.enemyAudio.PlaySFX(enemy.enemyAudio.characterClips[(int)CharacterAudio.CharacterSound.Hit]);

        yield return knockbackWait; // 플레이어 재추적
        enemy.currentTarget = player;
        enemy.isPreformingAction = false;
    }

    protected void DieProcess()
    {
        enemy.onDie = true;
        enemy.collider.enabled = false;
        enemy.blockerCollider.enabled = false;
        enemy.rigidbody.isKinematic = true;

        enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.DeadAnimation, true);
        enemy.enemyAudio.PlaySFX(enemy.enemyAudio.characterClips[(int)CharacterAudio.CharacterSound.Dead]);

        foreach (WeaponDamageCollider attackCollider in enemy.attackColliders)
            attackCollider.CloseDamageCollider();

        if (enemy.enemyType == EnemyManager.EnemyType.Boss)
            GameManager.instance.EndBossBattle();

        if (GetComponentInChildren<CharacterDissolve>() != null)
        {
            CharacterDissolve characterDissolve = GetComponentInChildren<CharacterDissolve>();
            StartCoroutine(characterDissolve.DissolveFade());
        }
    }
}