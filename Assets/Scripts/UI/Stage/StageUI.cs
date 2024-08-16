using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UIData;

public class StageUI : MonoBehaviour
{
    [Header("Data")]
    private StageUIData stageUIData;

    [Header("Stage Info")]
    [SerializeField]
    private Animator stageInfoAnimator;
    [SerializeField]
    private Text stageName;

    [Header("BossStage Info")]
    [SerializeField]
    private GameObject bossStageUI;
    private BossClearUI bossClearUI;

    [SerializeField]
    private Text bossName;
    [SerializeField]
    private Slider bossHPSlider;

    [Header("Coroutine")]
    private WaitForSeconds stageInfoWait;
    private WaitForSeconds defeatWait;
    private WaitForSeconds victoryWait;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        stageUIData = new StageUIData();
        bossClearUI = GetComponentInChildren<BossClearUI>();

        stageInfoWait = new WaitForSeconds(stageUIData.StageInfoDelay);
        defeatWait = new WaitForSeconds(stageUIData.DefeatDelay);
        victoryWait = new WaitForSeconds(stageUIData.VictoryDelay);
    }

    public void OpenStageInfo(FieldInfo fieldInfo)
    {
        switch (fieldInfo.fieldType)
        {
            case FieldInfo.FieldType.Town:
                break;

            case FieldInfo.FieldType.Field:
                Field field = fieldInfo as Field;
                StartCoroutine(OpenStageUI(field));
                break;

            case FieldInfo.FieldType.BossField:
                break;
        }
    }

    IEnumerator OpenStageUI(Field field)
    {
        stageInfoAnimator.gameObject.SetActive(true);
        stageInfoAnimator.enabled = true;
        stageName.text = field.stageName;
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.FieldUI]);

        yield return stageInfoWait;
        stageInfoAnimator.enabled = false;
        stageInfoAnimator.gameObject.SetActive(false);
    }

    public void OpenBossStageUI(string name)
    {
        bossStageUI.SetActive(true);
        bossClearUI.gameObject.SetActive(true);
        bossName.text = name;
        bossHPSlider.value = 1;
    }

    public void BossHPUIModify(float curHP, float maxHP)
    {
        bossHPSlider.value = curHP / maxHP;
    }

    public IEnumerator EndBossStageUI(BossItemDrop boss)
    {
        yield return defeatWait;
        bossStageUI.SetActive(false);
        bossClearUI.bossItemDrop = boss;
        bossClearUI.PlayBossClear();

        yield return victoryWait;
        bossClearUI.gameObject.SetActive(true);
    }
}