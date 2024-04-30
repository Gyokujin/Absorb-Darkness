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
    private new Rigidbody rigidbody;
    private Animator animator;
    protected EnemyManager enemyManager;
    private CharacterAudio characterAudio;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        rigidbody = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
        characterAudio = GetComponent<CharacterAudio>();
        hitWait = new WaitForSeconds(hitTime);
        knockbackWait = new WaitForSeconds(knockbackTime);
    }

    void Start()
    {
        maxHealth = SetMaxHealthLevel();
        currentHealth = maxHealth;
    }

    //void Update()
    //{
    //    enemyManager.onDamage = animator.GetBool("onDamage");
    //}

    int SetMaxHealthLevel()
    {
        maxHealth = healthLevel * healthLevelAmount;
        return maxHealth;
    }

    public virtual void TakeDamage(int damage, CharacterStatus player)
    {
        if (enemyManager.onDie)
            return;

        currentHealth -= damage;
        AudioManager.instance.PlayActionSFX(AudioManager.instance.actionClips[(int)PlayerActionSound.Attack2]);

        if (currentHealth <= 0)
        {
            DieProcess();
        }
        else
        {
            if (enemyManager.enemyType != EnemyType.Boss)
            {
                StopCoroutine("DamageProcess");
                enemyManager.onDamage = true;
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
        }
    }

    IEnumerator DamageProcess(CharacterStatus player)
    {
        animator.SetTrigger("doHit");
        characterAudio.PlaySFX(characterAudio.audioClips[(int)CharacterSound.Hit]);

        yield return hitWait;
        enemyManager.currentTarget = player;
        enemyManager.isPreformingAction = false;
    }

    IEnumerator KnockbackProcess(CharacterStatus player)
    {
        Vector3 attackDir = Vector3.Normalize(transform.position - player.transform.position); // 몬스터 넉백
        attackDir.y = 0;
        transform.rotation = Quaternion.LookRotation(-attackDir);
        enemyManager.rigidbody.velocity = Vector3.zero;
        enemyManager.rigidbody.AddForce(attackDir * knockbackPower, ForceMode.Impulse);

        animator.SetTrigger("doKnockback");
        characterAudio.PlaySFX(characterAudio.audioClips[(int)CharacterSound.Hit]);

        yield return knockbackWait; // 플레이어 재추적
        enemyManager.currentTarget = player;
        enemyManager.isPreformingAction = false;
    }

    protected void DieProcess()
    {
        currentHealth = 0;
        rigidbody.isKinematic = true;
        enemyManager.onDie = true;
        enemyManager.collider.enabled = false;
        enemyManager.blockerCollider.enabled = false;

        foreach (DamageCollider attackCollider in enemyManager.attackColliders)
        {
            attackCollider.CloseDamageCollider();
        }

        animator.SetTrigger("doDie");
        characterAudio.PlaySFX(characterAudio.audioClips[(int)CharacterSound.Die]);
    }
}