using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private FieldInfo curField;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        Application.targetFrameRate = 60;
        LockCamera(true);
    }

    public void ReadFieldInfo(FieldInfo fieldInfo)
    {
        if (curField == null || curField != fieldInfo)
        {
            curField = fieldInfo;
            UIManager.instance.OpenStageUI(curField);

            switch (curField.fieldType)
            {
                case FieldInfo.FieldType.Town:
                    break;

                case FieldInfo.FieldType.Field:
                    break;

                case FieldInfo.FieldType.BossField:
                    BossField bossField = fieldInfo as BossField;
                    AudioManager.instance.PlayBGM(bossField.bossBgm);
                    break;
            }
        }
    }

    public void EndBossBattle()
    {
        AudioManager.instance.MuteBGM();
        StartCoroutine(EndBossBattleProcess());
    }

    IEnumerator EndBossBattleProcess()
    {
        yield return StartCoroutine(UIManager.instance.stageUI.EndBossStageUI());
        BossField bossField = curField as BossField;
        Item[] bossDropItem = bossField.dropItems;

        foreach (Item item in bossDropItem)
        {
            InventoryManager.instance.ItemLoot(item);
        }
    }

    public void LockCamera(bool onLock)
    {
        if (onLock)
        {
            Cursor.visible = false; // Ŀ���� ������ �ʰ� �Ѵ�.
            Cursor.lockState = CursorLockMode.Locked; // Ŀ���� �߾����� ����
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}