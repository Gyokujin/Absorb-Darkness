using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : CharacterStatus
{
    [Header("Status")]
    public float detectionRadius = 20;
    public float detectionAngleMax = 50;
    public float detectionAngleMin = -50;
    public float attackRangeMax = 1.5f;

    [Header("Damage")]
    [SerializeField]
    private float hitTime = 1f;
    [SerializeField]
    private float knockbackTime = 2.5f;
    [SerializeField]
    private float knockbackPower = 50;
    public int damageAmount; // 누적 데미지. 이 수치가 3/5 이상이 되면 넉백을 한다.
    private WaitForSeconds hitWait;
    private WaitForSeconds knockbackWait;

    [Header("Component")]
    private Animator animator;
    private new Rigidbody rigidbody;
    private new Collider collider;
    private EnemyManager enemyManager;
    private EnemyAudio enemyAudio;
    [SerializeField]
    private PursueTargetState pursueTargetState;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        rigidbody = GetComponent<Rigidbody>();
        enemyManager = GetComponent<EnemyManager>();
        enemyAudio = GetComponent<EnemyAudio>();

        hitWait = new WaitForSeconds(hitTime);
        knockbackWait = new WaitForSeconds(knockbackTime);
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

    public void TakeDamage(int damage, CharacterStatus player)
    {
        StopCoroutine("DamageProcess");
        enemyManager.onHit = true;
        currentHealth -= damage;

        if (currentHealth > 0 && !enemyManager.onDie)
        {
            damageAmount += damage;

            if (damageAmount >= (float)maxHealth * 0.6f) // 한번에 최대 체력의 절반 이상의 피해가 들어오면 스매시를 실행
            {
                StartCoroutine("KnockbackProcess", player);
            }
            else
            {
                StartCoroutine("DamageProcess", player);
            }
        }
        else
        {
            DieProcess();
        }
    }

    IEnumerator DamageProcess(CharacterStatus player)
    {
        animator.SetTrigger("doHit");
        enemyAudio.PlaySFX(enemyAudio.hitClip);

        yield return hitWait;
        enemyManager.currentTarget = player;
        enemyManager.isPreformingAction = false;
    }

    IEnumerator KnockbackProcess(CharacterStatus player)
    {
        Vector3 attackDir = Vector3.Normalize(transform.position - player.transform.position); // 몬스터 넉백
        attackDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-attackDir);
        rigidbody.velocity = Vector3.zero;
        rigidbody.AddForce(attackDir * knockbackPower, ForceMode.Impulse);

        animator.SetTrigger("doKnockback");
        enemyAudio.PlaySFX(enemyAudio.hitClip);

        yield return knockbackWait; // 플레이어 재추적
        enemyManager.currentTarget = player;
        enemyManager.isPreformingAction = false;
    }

    void DieProcess()
    {
        currentHealth = 0;
        enemyManager.onDie = true;
        collider.enabled = false;
        animator.SetTrigger("doDie");
        enemyAudio.PlaySFX(enemyAudio.dieClip);
    }
}