using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStatus : EnemyStatus
{
    public override void TakeDamage(int damage, CharacterStatus player)
    {
        if (enemyManager.onDie)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            // DieProcess();
        }
        else
        {
            if (enemyManager.enemyType != EnemyType.Boss)
            {
                StopCoroutine("DamageProcess");
                enemyManager.onDamage = true;
                damageAmount += damage;

                if (damageAmount >= (float)maxHealth * 0.6f) // �ѹ��� �ִ� ü���� ���� �̻��� ���ذ� ������ ���Žø� ����
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
}