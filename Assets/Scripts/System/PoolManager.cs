using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;

    public enum Enemy
    {
        Bat
    }

    public enum EnemySpell
    {
        Meteor, MeteorExplosion, LightningImpact, ElectricShock, PosionMist
    }

    [Header("Enemy")]
    [SerializeField]
    private GameObject[] enemies;
    private List<GameObject>[] enemyPool;

    [Header("Spell")]
    [SerializeField]
    private GameObject[] enemySpells;
    private List<GameObject>[] enemySpellPool;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        Init();
    }

    void Init()
    {
        enemyPool = new List<GameObject>[enemies.Length];
        enemySpellPool = new List<GameObject>[enemySpells.Length];

        for (int i = 0; i < enemyPool.Length; i++)
        {
            enemyPool[i] = new List<GameObject>();
        }

        for (int i = 0; i < enemySpellPool.Length; i++)
        {
            enemySpellPool[i] = new List<GameObject>();
        }
    }

    public GameObject GetEnemy(int index)
    {
        GameObject select = null;

        foreach (GameObject enemyObj in enemyPool[index])
        {
            if (!enemyObj.activeSelf)
            {
                select = enemyObj;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(enemies[index]);
            enemyPool[index].Add(select);
        }

        return select;
    }

    public GameObject GetEnemySpell(int index)
    {
        GameObject select = null;

        foreach (GameObject spellObj in enemySpellPool[index])
        {
            if (!spellObj.activeSelf)
            {
                select = spellObj;
                select.SetActive(true);
                break;
            }
        }

        if (!select)
        {
            select = Instantiate(enemySpells[index]);
            enemySpellPool[index].Add(select);
        }

        select.transform.parent = null;
        return select;
    }

    public void Return(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.transform.parent = transform;
        obj.SetActive(false);
    }
}