using System.Collections;
using System.Collections.Generic;
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
            readonly get => defeatDelay ?? 5;
            set => defeatDelay = value;
        }

        // Victory Delay
        private float? victoryDelay;
        public float VictoryDelay
        {
            readonly get => victoryDelay ?? 6f;
            set => victoryDelay = value;
        }
    }

    public struct MessageUIData
    {
        [Header("Interact Text")]
        // Interact Item Text
        private readonly string itemInteractText;
        public string ItemInteractText
        {
            readonly get => itemInteractText ?? "아이템을 획득한다 E";
            set => ItemInteractText = value;
        }

        // Interact Message Text
        private string messageInteractText;
        public string MessageInteractText
        {
            readonly get => messageInteractText ?? "메시지를 확인한다 E";
            set => messageInteractText = value;
        }

        // Interact LockDoor Text
        private string lockDoorInteractText;
        public string LockDoorInteractText
        {
            readonly get => lockDoorInteractText ?? "문을 연다 E";
            set => lockDoorInteractText = value;
        }

        // Interact FogWall Text
        private string fogWallInteractText;
        public string FogWallInteractText
        {
            readonly get => fogWallInteractText ?? "안개 속으로 들어간다 E";
            set => fogWallInteractText = value;
        }
    }
}