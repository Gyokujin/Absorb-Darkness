using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectUI : MonoBehaviour
{
    [SerializeField]
    private GameObject[] selectButtons;

    public void OpenSelectUI()
    {
        foreach (GameObject button in selectButtons)
            button.SetActive(true);
    }

    public void CloseSelectUI()
    {
        foreach (GameObject button in selectButtons)
            button.SetActive(false);
    }

    public void SelectInventory()
    {
        UIManager.instance.inventoryUI.gameObject.SetActive(true);
        UIManager.instance.inventoryUI.OpenWeaponInventory();
    }

    public void SelectEquipment()
    {

    }

    public void SelectGameSystem()
    {

    }
}