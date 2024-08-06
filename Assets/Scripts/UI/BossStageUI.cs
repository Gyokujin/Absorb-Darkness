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
    private Animator bossClearUI;
    [SerializeField]
    private Text bossNameText;
    [SerializeField]
    private Slider bossHPSlider;

    public void OpenBossStageUI(string name)
    {
        bossInfoUI.SetActive(true);
        bossNameText.text = name;
        bossHPSlider.value = 1;
    }

    public void BossHPUIModify(float curHP, float maxHP)
    {
        bossHPSlider.value = curHP / maxHP;
    }

    public IEnumerator EndBossStageUI(BossItemDrop bossItemDrop)
    {
        bossInfoUI.SetActive(false);

        yield return new WaitForSeconds(2.5f);
        bossClearUI.gameObject.SetActive(true);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Victory]);

        yield return new WaitForSeconds(5.5f);
        bossItemDrop.ItemLoot();
    }
}