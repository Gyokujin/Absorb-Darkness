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

        // LightningImpact Speed
        private float? lightningImpactSpeed;
        public float LightningImpactSpeed
        {
            readonly get => lightningImpactSpeed ?? 36;
            set => lightningImpactSpeed = value;
        }

        // LightningImpact RetentionTime
        private float? lightningImpactRetentionTime;
        public float LightningImpactRetentionTime
        {
            readonly get => lightningImpactRetentionTime ?? 3;
            set => lightningImpactRetentionTime = value;
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

        // Meteor RetentionTime
        private float? meteorRetentionTime;
        public float MeteorRetentionTime
        {
            readonly get => meteorRetentionTime ?? 10;
            set => meteorRetentionTime = value;
        }

        // Explosion RetentionTime
        private float? explosionRetentionTime;
        public float ExplosionRetentionTime
        {
            readonly get => explosionRetentionTime ?? 0.05f;
            set => explosionRetentionTime = value;
        }
    }
}