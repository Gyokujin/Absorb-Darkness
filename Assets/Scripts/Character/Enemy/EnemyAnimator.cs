using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorManager
{
    private EnemyManager enemyManager;
    private EnemyStatus enemyStatus;
    private EnemyAudio enemyAudio;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        animator = GetComponent<Animator>();
        enemyManager = GetComponentInParent<EnemyManager>();
        enemyStatus = GetComponentInParent<EnemyStatus>();
        enemyAudio = GetComponentInParent<EnemyAudio>();
    }

    void OnAnimatorMove()
    {
        if (enemyManager.onHit || enemyManager.onDie)
            return;

        enemyManager.rigidbody.drag = 0;
        Vector3 deltaPoistion = animator.deltaPosition;
        deltaPoistion.y = 0;
        Vector3 velocity = deltaPoistion / Time.deltaTime;
        enemyManager.rigidbody.velocity = velocity;
    }

    public void AttackProcess()
    {
        enemyAudio.PlaySFX(enemyAudio.attackClip);
    }

    public void AttackDelay()
    {
        animator.SetBool("onAttack", false);
        animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        Invoke("AttackEnd", enemyManager.currentRecoveryTime);
    }

    void AttackEnd()
    {
        enemyManager.isPreformingAction = false;
    }

    public void HitEnd()
    {
        enemyManager.onHit = false;
        enemyStatus.damageAmount = 0;
    }
}