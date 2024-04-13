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
    private WaitForSeconds hitWait;
    private WaitForSeconds smashWait;

    [Header("Component")]
    private Animator animator;
    private new Collider collider;
    private EnemyManager enemyManager;
    [SerializeField]
    private PursueTargetState pursueTargetState;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        hitWait = new WaitForSeconds(hitTime);
        smashWait = new WaitForSeconds(hitTime * 1.5f);
        animator = GetComponentInChildren<Animator>();
        collider = GetComponent<Collider>();
        enemyManager = GetComponent<EnemyManager>();
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
        currentHealth -= damage;

        if (currentHealth > 0 && !enemyManager.onDie)
        {
            StartCoroutine("DamageProcess", player);
        }
        else
        {
            StopCoroutine("DamageProcess");
            currentHealth = 0;
            enemyManager.onDie = true;
            collider.enabled = false;
            animator.SetTrigger("doDie");
        }
    }

    IEnumerator DamageProcess(CharacterStatus player)
    {
        animator.SetTrigger("doHit");
        yield return hitWait;
        enemyManager.currentTarget = player;
        enemyManager.isPreformingAction = false;
    }
}