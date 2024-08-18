using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerManager player;

    [Header("Weapon")]
    public int curLeftWeaponIndex = 0;
    public int curRightWeaponIndex = 0;
    public WeaponItem curLeftWeapon;
    public WeaponItem curRightWeapon;
    public WeaponItem unarmedWeapon;

    public WeaponItem[] weaponInLeftSlots;
    public WeaponItem[] weaponInRightSlots;
    public List<WeaponItem> equipmentWeapons;

    [Header("Using Item")]
    public UsingItem curUsingItem;

    [Header("Weapon Slot")]
    public WeaponHolderSlot leftHandSlot;
    public WeaponHolderSlot rightHandSlot;
    public WeaponHolderSlot backSlot;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        player = GetComponentInParent<PlayerManager>();

        WeaponHolderSlot[] weaponHolderSlots = GetComponentsInChildren<WeaponHolderSlot>();

        foreach (WeaponHolderSlot weaponSlot in weaponHolderSlots)
        {
            if (weaponSlot.isLeftHandSlot)
                leftHandSlot = weaponSlot;
            else if (weaponSlot.isRightHandSlot)
                rightHandSlot = weaponSlot;
            else if (weaponSlot.isBackSlot)
                backSlot = weaponSlot;
        }
    }

    void Start()
    {
        InitWeaponInventory();
    }

    void InitWeaponInventory()
    {
        curLeftWeapon = weaponInLeftSlots[0];
        curRightWeapon = weaponInRightSlots[0];
        LoadWeaponSlot(curLeftWeapon, true);
        LoadWeaponSlot(curRightWeapon, false);
        UpdateEquipmentSlot();
    }

    public void LoadWeaponSlot(WeaponItem weaponItem, bool isLeft)
    {
        if (isLeft)
        {
            leftHandSlot.currentWeapon = weaponItem;
            leftHandSlot.LoadWeaponModel(weaponItem);
            player.playerCombat.LoadWeaponDamageCollider(true);
            UIManager.instance.quickSlotsUI.UpdateWeaponQuickSlotsUI(true, weaponItem);

            if (weaponItem != null)
                player.playerAnimator.animator.CrossFade(weaponItem.left_Hand_Idle, player.characterAnimatorData.AnimationFadeAmount);
            else
                player.playerAnimator.animator.CrossFade(player.characterAnimatorData.LeftArmEmpty, player.characterAnimatorData.AnimationFadeAmount);
        }
        else
        {
            if (player.playerInput.twoHandFlag)
            {
                backSlot.LoadWeaponModel(leftHandSlot.currentWeapon);
                leftHandSlot.UnloadWeaponAndDestroy();
                player.playerAnimator.animator.CrossFade(weaponItem.th_idle, player.characterAnimatorData.AnimationFadeAmount);
            }
            else
            {
                player.playerAnimator.animator.CrossFade(player.characterAnimatorData.BothArmsEmpty, player.characterAnimatorData.AnimationFadeAmount);
                backSlot.UnloadWeaponAndDestroy();

                if (weaponItem != null)
                    player.playerAnimator.animator.CrossFade(weaponItem.right_Hand_Idle, player.characterAnimatorData.AnimationFadeAmount);
                else
                    player.playerAnimator.animator.CrossFade(player.characterAnimatorData.RightArmEmpty, player.characterAnimatorData.AnimationFadeAmount);
            }

            rightHandSlot.currentWeapon = weaponItem;
            rightHandSlot.LoadWeaponModel(weaponItem);
            player.playerCombat.LoadWeaponDamageCollider(false);
            UIManager.instance.quickSlotsUI.UpdateWeaponQuickSlotsUI(false, weaponItem);
        }
    }

    public void UpdateEquipmentSlot()
    {
        equipmentWeapons.Clear();

        foreach (WeaponItem leftSlotWeapon in weaponInLeftSlots)
        {
            if (leftSlotWeapon != unarmedWeapon)
                equipmentWeapons.Add(leftSlotWeapon);
        }

        foreach (WeaponItem rightSlotWeapon in weaponInRightSlots)
        {
            if (rightSlotWeapon != unarmedWeapon)
                equipmentWeapons.Add(rightSlotWeapon);
        }
    }

    public void LoadUsingItemSlot(UsingItem usingItem)
    {
        UIManager.instance.quickSlotsUI.UpdateUsingItemUI(usingItem, usingItem.itemCount);
    }

    public void ChangeLeftWeapon()
    {
        curLeftWeaponIndex++;

        if (curLeftWeaponIndex > weaponInLeftSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            curLeftWeaponIndex = -1;
            curLeftWeapon = unarmedWeapon;
            LoadWeaponSlot(unarmedWeapon, true);
        }
        else if (weaponInLeftSlots[curLeftWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            curLeftWeapon = weaponInLeftSlots[curLeftWeaponIndex];
            LoadWeaponSlot(weaponInLeftSlots[curLeftWeaponIndex], true);
        }
        else
            curLeftWeaponIndex++;
    }

    public void ChangeRightWeapon()
    {
        curRightWeaponIndex++;

        if (curRightWeaponIndex > weaponInRightSlots.Length - 1) // index가 슬롯 범위를 초과할 경우
        {
            curRightWeaponIndex = -1;
            curRightWeapon = unarmedWeapon;
            LoadWeaponSlot(unarmedWeapon, false);
        }
        else if (weaponInRightSlots[curRightWeaponIndex] != null) // 장착할 무기가 있는 경우
        {
            curRightWeapon = weaponInRightSlots[curRightWeaponIndex];
            LoadWeaponSlot(weaponInRightSlots[curRightWeaponIndex], false);
        }
        else
            curRightWeaponIndex++;
    }
}