using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossStageUI : MonoBehaviour
{
    [Header("Boss Info")]
    [SerializeField]
    private GameObject bossInfoUI;
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

    public void OpenBossStageUI(string name)
    {
        bossInfoUI.SetActive(true);
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
        bossInfoUI.SetActive(false);
        bossClearUI.bossItemDrop = boss;
        bossClearUI.PlayBossClear();

        yield return new WaitForSeconds(victoryDelay);
        bossClearUI.gameObject.SetActive(true);
    }
}