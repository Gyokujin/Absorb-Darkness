using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : AnimatorManager
{
    private EnemyMove enemyMove;

    void Awake()
    {
        animator = GetComponent<Animator>();
        enemyMove = GetComponentInParent<EnemyMove>();
    }

    private void OnAnimatorMove()
    {
        enemyMove.rigidbody.drag = 0;
        Vector3 deltaPoistion = animator.deltaPosition;
        deltaPoistion.y = 0;
        Vector3 velocity = deltaPoistion / Time.deltaTime;
        enemyMove.rigidbody.velocity = velocity;
    }
}