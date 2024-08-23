using UnityEngine;

namespace ShaderData
{
    public struct CharacterShaderData
    {
        [Header("Dissolve")]
        // DissolveBeforeDelay
        private float? dissolveDelay;
        public float DissolveDelay
        {
            readonly get => dissolveDelay ?? 3;
            set => dissolveDelay = value;
        }

        // DissolveTime
        private float? dissolveTime;
        public float DissolveTime
        {
            readonly get => dissolveTime ?? 2;
            set => dissolveTime = value;
        }

        // DissolveProgress
        private float? dissolveProgress;
        public float DissolveProgress
        {
            readonly get =>  dissolveProgress ?? 0.3f;
            set => dissolveProgress = value;
        }

        // DissolveThreshold
        private string dissolveThresholdParameter;
        public string DissolveThresholdParameter
        {
            readonly get => dissolveThresholdParameter ?? "_DissolveThreshold";
            set => dissolveThresholdParameter = value;
        }
    }
}