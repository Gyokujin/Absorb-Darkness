using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnemyData;

public class AmbushState : EnemyState
{
    [Header("Data")]
    private EnemyAnimationData animationData;

    [Header("Ambush")]
    private bool isSleeping = true;
    [SerializeField]
    private float detectionRadius = 2;

    void Start() // 부모 클래스의 초기화를 위해 Start
    {
        Init();
    }

    void Init()
    {
        animationData = new EnemyAnimationData();
    }

    public override EnemyState Tick(EnemyManager enemyManager, EnemyStatus enemyStatus, EnemyAnimator enemyAnimator)
    {
        if (isSleeping && !enemyManager.isInteracting)
            enemyAnimator.PlayTargetAnimation(animationData.SleepAnimation, true);

        Collider[] colliders = Physics.OverlapSphere(enemyManager.transform.position, detectionRadius, enemyManager.detectionLayer);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.GetComponent<CharacterStatus>() != null)
            {
                CharacterStatus characterStatus = colliders[i].transform.GetComponent<CharacterStatus>();
                Vector3 targetDir = characterStatus.transform.position - enemyManager.transform.position;
                float viewableAngle = Vector3.Angle(targetDir, enemyManager.transform.forward);

                if (viewableAngle > enemyStatus.detectionAngleMin && viewableAngle < enemyStatus.detectionAngleMax)
                {
                    enemyManager.currentTarget = characterStatus;
                    isSleeping = false;
                    enemyAnimator.PlayTargetAnimation(animationData.WakeAnimation, true);
                }
            }
        }

        if (enemyManager.currentTarget != null)
            return enemyManager.pursueTargetState;
        else
            return this;
    }
}