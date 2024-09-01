using UnityEngine;

namespace UIData
{
    public struct StageUIData
    {
        [Header("Stage")]
        // StageInfo Delay
        private float? stageInfoDelay;
        public float StageInfoDelay
        {
            readonly get => stageInfoDelay ?? 4;
            set => stageInfoDelay = value;
        }

        [Header("Boss Stage")]
        // Defeat Delay
        private float? defeatDelay;
        public float DefeatDelay
        {
            readonly get => defeatDelay ?? 9;
            set => defeatDelay = value;
        }

        // Victory Delay
        private float? victoryDelay;
        public float VictoryDelay
        {
            readonly get => victoryDelay ?? 6.5f;
            set => victoryDelay = value;
        }

        // BossItem Loot Delay
        private float? bossItemLootDelay;
        public float BossItemLootDelay
        {
            readonly get => bossItemLootDelay ?? 2;
            set => bossItemLootDelay = value;
        }
    }

    public struct MessageUIData
    {
        [Header("Interact Text")]
        // Interact DropItem Text
        private readonly string dropItemInteractText;
        public string DropItemInteractText
        {
            readonly get => dropItemInteractText ?? "�������� ȹ���Ѵ� E";
            set => DropItemInteractText = value;
        }

        // Interact RuneLetter Text
        private string runeLetterInteractText;
        public string RuneLetterInteractText
        {
            readonly get => runeLetterInteractText ?? "�޽����� Ȯ���Ѵ� E";
            set => runeLetterInteractText = value;
        }

        // Interact LockDoor Text
        private string lockDoorInteractText;
        public string LockDoorInteractText
        {
            readonly get => lockDoorInteractText ?? "���� ���� E";
            set => lockDoorInteractText = value;
        }

        // Interact FogWall Text
        private string fogWallInteractText;
        public string FogWallInteractText
        {
            readonly get => fogWallInteractText ?? "�Ȱ� ������ ���� E";
            set => fogWallInteractText = value;
        }

        // Interact UnlockDoor Success Text
        private string unlockDoorInteractSuccessText;
        public string UnlockDoorInteractSuccessText
        {
            readonly get => unlockDoorInteractSuccessText ?? "���踦 ����Ͽ����ϴ�.";
            set => unlockDoorInteractSuccessText = value;
        }

        // Interact UnlockDoor Fail Text
        private string unlockDoorInteractFailText;
        public string UnlockDoorInteractFailText
        {
            readonly get => unlockDoorInteractFailText ?? "�ʿ��� ���谡 �����ϴ�.";
            set => unlockDoorInteractFailText = value;
        }
    }
}