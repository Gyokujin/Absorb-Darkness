using UnityEngine;

namespace ShaderData
{
    public struct CharacterShaderData
    {
        [Header("Color")]
        // Texture Brightness
        private string textureBrightness;
        public string TextureBrightness
        {
            readonly get => textureBrightness ?? "_TextureBrightness";
            set => textureBrightness = value;
        }

        // Light Color
        private string lightColor;
        public string LightColor
        {
            readonly get => lightColor ?? "_LightColor";
            set => lightColor = value;
        }

        // Light Direction
        private string lightDirection;
        public string LightDirection
        {
            readonly get => lightDirection ?? "_LightDirection";
            set => lightDirection = value;
        }

        // Light Intensity
        private string lightIntensity;
        public string LightIntensity
        {
            readonly get => lightIntensity ?? "_LightIntensity";
            set => lightIntensity = value;
        }

        [Header("Dissolve")]
        // Dissolve Before Delay
        private float? dissolveDelay;
        public float DissolveDelay
        {
            readonly get => dissolveDelay ?? 3;
            set => dissolveDelay = value;
        }

        // Dissolve Time
        private float? dissolveTime;
        public float DissolveTime
        {
            readonly get => dissolveTime ?? 2;
            set => dissolveTime = value;
        }

        // Dissolve Progress
        private float? dissolveProgress;
        public float DissolveProgress
        {
            readonly get =>  dissolveProgress ?? 0.3f;
            set => dissolveProgress = value;
        }

        // Dissolve Threshold
        private string dissolveThresholdParameter;
        public string DissolveThresholdParameter
        {
            readonly get => dissolveThresholdParameter ?? "_DissolveThreshold";
            set => dissolveThresholdParameter = value;
        }
    }
}