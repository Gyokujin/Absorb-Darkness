using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorManager
{
    private EnemyManager enemyManager;

    void Awake()
    {
        enemyManager = GetComponentInParent<EnemyManager>();
        animator = GetComponent<Animator>();
    }

    private void OnAnimatorMove()
    {
        enemyManager.rigidbody.drag = 0;
        Vector3 deltaPoistion = animator.deltaPosition;
        deltaPoistion.y = 0;
        Vector3 velocity = deltaPoistion / Time.deltaTime;
        enemyManager.rigidbody.velocity = velocity;
    }

    public void AttackEnd()
    {
        animator.SetBool("onAttack", false);
        animator.SetFloat("vertical", 0, 0.1f, Time.deltaTime);
        enemyManager.isPreformingAction = false;
    }
}