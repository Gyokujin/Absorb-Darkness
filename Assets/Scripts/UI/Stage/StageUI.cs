using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [Header("Stage Info")]
    [SerializeField]
    private GameObject stageUI;
    [SerializeField]
    private Text stageName;

    [Header("Boss Info")]
    [SerializeField]
    private GameObject bossStageUI;
    [SerializeField]
    private Text bossNameText;
    [SerializeField]
    private Slider bossHPSlider;

    [Header("UI Delay")]
    [SerializeField]
    private float defeatDelay = 3f;
    [SerializeField]
    private float victoryDelay = 2.5f;

    [Header("Component")]
    private BossClearUI bossClearUI;

    void Awake()
    {
        bossClearUI = GetComponentInChildren<BossClearUI>();
    }

    public void OpenStageInfo(FieldInfo fieldInfo)
    {
        switch (fieldInfo.fieldType)
        {
            case FieldInfo.FieldType.Town:
                break;

            case FieldInfo.FieldType.Field:
                Field field = fieldInfo as Field;
                OpenStageUI(field);
                break;

            case FieldInfo.FieldType.BossField:
                break;
        }
    }

    public void OpenStageUI(Field field)
    {
        stageUI.SetActive(true);
        stageName.text = field.stageName;
    }

    public void OpenBossStageUI(string name)
    {
        bossStageUI.SetActive(true);
        bossClearUI.gameObject.SetActive(true);
        bossNameText.text = name;
        bossHPSlider.value = 1;
    }

    public void BossHPUIModify(float curHP, float maxHP)
    {
        bossHPSlider.value = curHP / maxHP;
    }

    public IEnumerator EndBossStageUI(BossItemDrop boss)
    {
        yield return new WaitForSeconds(defeatDelay);
        bossStageUI.SetActive(false);
        bossClearUI.bossItemDrop = boss;
        bossClearUI.PlayBossClear();

        yield return new WaitForSeconds(victoryDelay);
        bossClearUI.gameObject.SetActive(true);
    }
}