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
    private GameObject bossInfoUI;
    [SerializeField]
    private Text bossName;
    [SerializeField]
    private Slider bossHPSlider;
    [SerializeField]
    private Animator bossClearAnimator;

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
                BossField bossField = fieldInfo as BossField;
                bossInfoUI.SetActive(true);
                bossName.text = bossField.bossName;
                bossHPSlider.value = 1;
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

    public void BossHPUIModify(float curHP, float maxHP)
    {
        bossHPSlider.value = curHP / maxHP;
    }

    public IEnumerator EndBossStageUI()
    {
        yield return defeatWait;
        bossInfoUI.SetActive(false);
        bossClearAnimator.gameObject.SetActive(true);
        bossClearAnimator.enabled = true;
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Victory]);

        yield return victoryWait;
        bossClearAnimator.enabled = false;
        bossClearAnimator.gameObject.SetActive(false);
    }
}