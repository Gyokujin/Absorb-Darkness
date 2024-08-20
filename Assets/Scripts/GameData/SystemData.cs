using UnityEngine;

namespace SystemData
{
    public struct CameraData
    {
        [Header("Angle")]
        // MinPivot
        private float? minPivot;
        public float MinPivot
        {
            readonly get => minPivot ?? -35;
            set => minPivot = value;
        }

        // MaxPivot
        private float? maxPivot;
        public float MaxPivot
        {
            readonly get => maxPivot ?? 35;
            set => maxPivot = value;
        }

        [Header("Camera")]
        // LookSpeed
        private float? lookSpeed;
        public float LookSpeed
        {
            readonly get => lookSpeed ?? 0.025f;
            set => lookSpeed = value;
        }

        // FollowSpeed
        private float? followSpeed;
        public float FollowSpeed
        {
            readonly get => followSpeed ?? 0.5f;
            set => followSpeed = value;
        }

        // PivotSpeed
        private float? pivotSpeed;
        public float PivotSpeed
        {
            readonly get => pivotSpeed ?? 0.01f;
            set => pivotSpeed = value;
        }

        // PlayerFollowRate
        private float? playerFollowRate;
        public float PlayerFollowRate
        {
            readonly get => playerFollowRate ?? 0.2f;
            set => playerFollowRate = value;
        }

        // Camera CollisionOffset
        private float? cameraCollisionOffset;
        public float CameraCollisionOffset
        {
            readonly get => cameraCollisionOffset ?? 0.2f;
            set => cameraCollisionOffset = value;
        }

        // Min CollisionOffset
        private float? minCollisionOffset;
        public float MinCollisionOffset
        {
            readonly get => minCollisionOffset ?? 0.2f;
            set => minCollisionOffset = value;
        }

        // UnlockedPivotPosition
        private float? unlockedPivotPositionY;
        public float UnlockedPivotPositionY
        {
            readonly get => unlockedPivotPositionY ?? 0.575f;
            set => unlockedPivotPositionY = value;
        }

        // LockedPivotPosition
        private readonly float? lockedPivotPositionY;
        public float LockedPivotPositionY
        {
            readonly get => lockedPivotPositionY ?? 1.2f;
            set => lockOnRotateMax = value;
        }

        [Header("LockOn")]
        // LockOnRadius
        private float? lockOnRadius;
        public float LockOnRadius
        {
            readonly get => lockOnRadius ?? 26;
            set => lockOnRadius = value;
        }

        // MaxLockOnDistance
        private float? maxLockOnDistance;
        public float MaxLockOnDistance
        {
            readonly get => maxLockOnDistance ?? 36;
            set => maxLockOnDistance = value;
        }

        // LockOnAngleLimit
        private float? lockOnAngleLimit;
        public float LockOnAngleLimit
        {
            readonly get => lockOnAngleLimit ?? 50;
            set => lockOnAngleLimit = value;
        }

        // LockOn RotateMax
        private float? lockOnRotateMax;
        public float LockOnRotateMax
        {
            readonly get => lockOnRotateMax ?? 30;
            set => lockOnRotateMax = value;
        }

        // CameraSphereRadius
        private float? cameraSphereRadius;
        public float CameraSphereRadius
        {
            readonly get => cameraSphereRadius ?? 0.2f;
            set => cameraSphereRadius = value;
        }
    }

    public struct PhysicsData
    {
        [Header("Ground & Air Detection States")]
        // GroundCheckDistance
        private float? groundCheckDis;
        public float GroundCheckDis
        {
            readonly get => groundCheckDis ?? 0.4f;
            set => groundCheckDis = value;
        }

        // GroundDetectionRayStart
        private float? groundDetectionRayStart;
        public float GroundDetectionRayStart
        {
            readonly get => groundDetectionRayStart ?? 0.5f;
            set => groundDetectionRayStart = value;
        }

        // DistanceBeginFallMin
        private float? distanceBeginFallMin;
        public float DistanceBeginFallMin
        {
            readonly get => distanceBeginFallMin ?? 1;
            set => distanceBeginFallMin = value;
        }

        // GroundDirRayDistance
        private float? groundDirRayDistance;
        public float GroundDirRayDistance
        {
            readonly get => groundDirRayDistance ?? 0.2f;
            set => groundDirRayDistance = value;
        }

        // KnockbackLimit
        private float? knockbackLimit;
        public float KnockbackLimit
        {
            readonly get => knockbackLimit ?? 0.6f;
            set => knockbackLimit = value;
        }

        [Header("Character Physics")]
        // LookAtSmoothing
        private float? lookAtSmoothing;
        public float LookAtSmoothing 
        {
            readonly get => lookAtSmoothing ?? 25;
            set => lookAtSmoothing = value;
        }

        // FallingFactor
        private float? fallingFactor;
        public float FallingFactor 
        {
            readonly get => fallingFactor ?? 0.1f;
            set => fallingFactor = value;
        }

        // FallingSpeedRatio
        private float? fallingSpeedRatio;
        public float FallingSpeedRatio 
        {
            readonly get => fallingSpeedRatio ?? 2;
            set => fallingSpeedRatio = value;
        }

        // FallingFrontForce
        private float? fallingFrontForce;
        public float FallingFrontForce 
        {
            readonly get => fallingFrontForce ?? 5;
            set => fallingFrontForce = value;
        }

        // FallingDownForce
        private float? fallingDownForce;
        public float FallingDownForce
        {
            readonly get => fallingDownForce ?? 750;
            set => fallingDownForce = value;
        }
    }

    public struct InteractData
    {
        [Header("Interact Check")]
        // Interact Check Radius
        private float? interactCheckRadius;
        public float InteractCheckRadius
        {
            readonly get => interactCheckRadius ?? 0.3f;
            set => interactCheckRadius = value;
        }

        // Interact Check Distance
        private float? interactCheckDis;
        public float InteractCheckDis
        {
            readonly get => interactCheckDis ?? 1;
            set => interactCheckDis = value;
        }

        // Interact Object Tag
        private string interactObjTag;
        public string InteractObjTag
        {
            readonly get => interactObjTag ?? "Interactable";
            set => interactObjTag = value;
        }
    }

    public struct InitItemData
    {
        [Header("Init Estus")]
        // Init EstusCount
        private int? initEstusCount;
        public int InitEstusCount
        {
            readonly get => initEstusCount ?? 3;
            set => initEstusCount = value;
        }
    }

    public struct LayerData
    {
        [Header("Character Layer")]
        // Player Layer
        private string playerLayer;
        public string PlayerLayer
        {
            readonly get => playerLayer ?? "Player";
            set => playerLayer = value;
        }

        // Enemy Layer
        private string enemyLayer;
        public string EnemyLayer
        {
            readonly get => enemyLayer ?? "Enemy";
            set => enemyLayer = value;
        }

        // Invincible Layer
        private string invincibleLayer;
        public string InvincibleLayer
        {
            readonly get => invincibleLayer ?? "Invincible";
            set => invincibleLayer = value;
        }

        // Ground Layer
        private string groundLayer;
        public string GroundLayer
        {
            readonly get => groundLayer ?? "Ground";
            set => groundLayer = value;
        }

        // Environment Layer
        private string environmentLayer;
        public string EnvironmentLayer
        {
            readonly get => environmentLayer ?? "Environment";
            set => environmentLayer = value;
        }
    }
}