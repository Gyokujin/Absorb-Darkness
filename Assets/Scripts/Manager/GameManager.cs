using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIData;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    [Header("Data")]
    private StageUIData stageUIData;

    [Header("Field")]
    private FieldInfo curField;
    private Entrance curEvent;

    [Header("Coroutine")]
    private WaitForSeconds itemLootWait;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        Init();
    }

    void Init()
    {
        LockCamera(true);
        Application.targetFrameRate = 60;
        itemLootWait = new WaitForSeconds(stageUIData.BossItemLootDelay);
    }

    public void ReadFieldInfo(FieldInfo fieldInfo, Entrance entrance)
    {
        if (curField == null || curField != fieldInfo)
        {
            curField = fieldInfo;
            curEvent = entrance;
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
        BossFieldEntrance bossEvent = curEvent as BossFieldEntrance;
        bossEvent.targetGate.DisappearGate();

        BossField bossField = curField as BossField;
        Item[] bossDropItem = bossField.dropItems;
        PlayerBehavior playerBehavior = FindObjectOfType<PlayerBehavior>();

        foreach (Item item in bossDropItem)
        {
            playerBehavior.ItemLoot(item);
            AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.PickUp]);
            yield return itemLootWait;
        }
    }

    public void LockCamera(bool onLock)
    {
        if (onLock)
        {
            Cursor.visible = false; // 커서를 보이지 않게 한다.
            Cursor.lockState = CursorLockMode.Locked; // 커서를 중앙으로 고정
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}