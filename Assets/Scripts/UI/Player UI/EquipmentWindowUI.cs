using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindowUI : MonoBehaviour
{
    public bool leftHandSlot01Selected;
    public bool leftHandSlot02Selected;
    public bool rightHandSlot01Selected;
    public bool rightHandSlot02Selected;

    [SerializeField]
    private HandEquipmentSlotUI[] handEquipmentSlotUI;

    void Awake()
    {
        handEquipmentSlotUI = GetComponentsInChildren<HandEquipmentSlotUI>(); // 최초 1회 장비창을 업데이트 한다.
    }

    public void OpenEquipmentsUI()
    {
        LoadWeaponsOnEquipmentScreen(UIManager.instance.playerInventory);
    }

    public void LoadWeaponsOnEquipmentScreen(PlayerInventory playerInventory)
    {
        for (int i = 0; i < handEquipmentSlotUI.Length; i++)
        {
            if (handEquipmentSlotUI[i].rightHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[0]);
            }
            else if (handEquipmentSlotUI[i].rightHandSlot02)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInRightSlots[1]);
            }
            else if (handEquipmentSlotUI[i].leftHandSlot01)
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[0]);
            }
            else
            {
                handEquipmentSlotUI[i].AddItem(playerInventory.weaponInLeftSlots[1]);
            }
        }
    }

    public void SelectLeftHandSlot01()
    {
        leftHandSlot01Selected = true;
    }

    public void SelectLeftHandSlot02()
    {
        leftHandSlot02Selected = true;
    }

    public void SelectRightHandSlot01()
    {
        rightHandSlot01Selected = true;
    }

    public void SelectRightHandSlot02()
    {
        rightHandSlot02Selected = true;
    }
}