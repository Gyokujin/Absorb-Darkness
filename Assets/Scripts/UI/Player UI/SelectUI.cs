using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    public void SelectInventory()
    {
        UIManager.instance.CloseAllUI();
        UIManager.instance.inventoryUI.gameObject.SetActive(true);
        UIManager.instance.inventoryUI.OpenWeaponInventory();
    }

    public void SelectEquipment()
    {
        UIManager.instance.CloseAllUI();
        UIManager.instance.equipmentUI.gameObject.SetActive(true);
        UIManager.instance.equipmentUI.OpenEquipmentsUI();
        AudioManager.instance.PlayUISFX(AudioManager.instance.uiClips[(int)AudioManager.UISound.Click]);
    }

    public void SelectGameSystem()
    {

    }
}