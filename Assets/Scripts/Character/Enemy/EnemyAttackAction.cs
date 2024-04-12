using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy AI/Enemy Actions/Attack Action")]
public class EnemyAttackAction : EnemyAction
{
    public float recoveryTimeMin = 0.5f;
    public float recoveryTimeMax = 1.75f;
    public float attackAngleMax = 35;
    public float attackAngleMin = -35;
    public float attackDisMin = 0;
    public float attackDisMax = 3;
}