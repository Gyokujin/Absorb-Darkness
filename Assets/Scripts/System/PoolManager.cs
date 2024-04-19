using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance = null;

    public enum EnemySpell
    {
        Meteor, MeteorExplosion, LightningImpact, ElectricShock, PosionMist
    }

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

        enemySpellPool = new List<GameObject>[enemySpells.Length];

        for (int i = 0; i < enemySpellPool.Length; i++)
        {
            enemySpellPool[i] = new List<GameObject>();
        }
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
            select = Instantiate(enemySpells[index], transform);
            enemySpellPool[index].Add(select);
        }

        return select;
    }

    public void Return(GameObject obj)
    {
        obj.transform.position = Vector3.zero;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(false);
    }
}