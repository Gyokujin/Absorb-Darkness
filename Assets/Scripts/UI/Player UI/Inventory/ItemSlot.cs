using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private Image slotIcon;
    [SerializeField]
    private Text slotCount;
    [SerializeField]
    private Text slotName;
    [SerializeField]
    private Text slotInfo;

    public void ItemSlotUpdate(Sprite icon, int count, string name, string info)
    {
        slotIcon.sprite = icon;
        slotCount.text = count.ToString();
        slotName.text = name;
        slotInfo.text = info;
    }
}