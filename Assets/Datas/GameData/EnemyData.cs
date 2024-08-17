using UnityEngine;

namespace EnemyData
{
    public struct SorceressData
    {
        [Header("LightningImpact")]
        // LightningImpact OffsetY
        private float? lightningImpactOffsetY;
        public float LightningImpactOffsetY
        {
            readonly get => lightningImpactOffsetY ?? 1.75f;
            set => lightningImpactOffsetY = value;
        }

        // LightningImpact Speed
        private float? lightningImpactSpeed;
        public float LightningImpactSpeed
        {
            readonly get => lightningImpactSpeed ?? 36;
            set => lightningImpactSpeed = value;
        }

        // LightningImpact RotateLimit
        private float? lightningImpactRotateLimit;
        public float LightningImpactRotateLimit
        {
            readonly get => lightningImpactRotateLimit ?? 50;
            set => lightningImpactRotateLimit = value;
        }

        [Header("Summon")]
        // Summon OffsetY
        private readonly float? summonOffsetY;
        public float SummonOffsetY
        {
            readonly get => summonOffsetY ?? -2;
            set => summonDelay = value;
        }

        // summonDelay
        private float? summonDelay;
        public float SummonDelay
        {
            readonly get => summonDelay ?? 1;
            set => summonDelay = value;
        }

        [Header("Meteor")]
        // Meteor FallSpeed
        private float? meteorFallSpeed;
        public float MeteorFallSpeed
        {
            readonly get => meteorFallSpeed ?? 22;
            set => meteorFallSpeed = value;
        }

        // Meteor FallDelay
        private float? meteorFallDelay;
        public float MeteorFallDelay
        {
            readonly get => meteorFallDelay ?? 0.5f;
            set => meteorFallDelay = value;
        }
    }
}