using UnityEngine;

namespace SystemData
{
    public struct CameraData
    {
        [Header("Angle")]
        // MinPivot
        private float? minPivot;
        private const float defaultMinPivot = -35;
        public float MinPivot
        {
            readonly get => minPivot ?? defaultMinPivot;
            set => minPivot = value;
        }

        // MaxPivot
        private float? maxPivot;
        private const float defaultMaxPivot = 35;
        public float MaxPivot
        {
            readonly get => maxPivot ?? defaultMaxPivot;
            set => maxPivot = value;
        }

        [Header("Camera")]
        // LookSpeed
        private float? lookSpeed;
        private const float defaultLookSpeed = 0.025f;
        public float LookSpeed
        {
            readonly get => lookSpeed ?? defaultLookSpeed;
            set => lookSpeed = value;
        }

        // FollowSpeed
        private float? followSpeed;
        private const float defaultFollowSpeed = 0.5f;
        public float FollowSpeed
        {
            readonly get => followSpeed ?? defaultFollowSpeed;
            set => followSpeed = value;
        }

        // PivotSpeed
        private float? pivotSpeed;
        private const float defaultPivotSpeed = 0.01f;
        public float PivotSpeed
        {
            readonly get => pivotSpeed ?? defaultPivotSpeed;
            set => pivotSpeed = value;
        }

        // PlayerFollowRate
        private float? playerFollowRate;
        private const float defaultPlayerFollowRate = 0.2f;
        public float PlayerFollowRate
        {
            readonly get => playerFollowRate ?? defaultPlayerFollowRate;
            set => playerFollowRate = value;
        }

        // Camera CollisionOffset
        private float? cameraCollisionOffset;
        private const float defaultCameraCollisionOffset = 0.2f;
        public float CameraCollisionOffset
        {
            readonly get => cameraCollisionOffset ?? defaultCameraCollisionOffset;
            set => cameraCollisionOffset = value;
        }

        // Min CollisionOffset
        private float? minCollisionOffset;
        private const float defaultMinCollisionOffset = 0.2f;
        public float MinCollisionOffset
        {
            readonly get => minCollisionOffset ?? defaultMinCollisionOffset;
            set => minCollisionOffset = value;
        }

        // UnlockedPivotPosition
        private float? unlockedPivotPositionY;
        private const float defaultUnlockedPivotPositionY = 0.575f;
        public float UnlockedPivotPositionY
        {
            readonly get => unlockedPivotPositionY ?? defaultUnlockedPivotPositionY;
            set => unlockedPivotPositionY = value;
        }

        // LockedPivotPosition
        private readonly float? lockedPivotPositionY;
        private const float defaultLockedPivotPositionY = 1.2f;
        public float LockedPivotPositionY
        {
            readonly get => lockedPivotPositionY ?? defaultLockedPivotPositionY;
            set => lockOnRotateMax = value;
        }

        [Header("LockOn")]
        // LockOnRadius
        private float? lockOnRadius;
        private const float defaultLockOnRadius = 26;
        public float LockOnRadius
        {
            readonly get => lockOnRadius ?? defaultLockOnRadius;
            set => lockOnRadius = value;
        }

        // MaxLockOnDistance
        private float? maxLockOnDistance;
        private const float defaultMaxLockOnDistance = 36;
        public float MaxLockOnDistance
        {
            readonly get => maxLockOnDistance ?? defaultMaxLockOnDistance;
            set => maxLockOnDistance = value;
        }

        // LockOnAngleLimit
        private float? lockOnAngleLimit;
        private const float defaultLockOnAngleLimit = 50;
        public float LockOnAngleLimit
        {
            readonly get => lockOnAngleLimit ?? defaultLockOnAngleLimit;
            set => lockOnAngleLimit = value;
        }

        // LockOn RotateMax
        private float? lockOnRotateMax;
        private const float defaultLockOnRotateMax = 30;
        public float LockOnRotateMax
        {
            readonly get => lockOnRotateMax ?? defaultLockOnRotateMax;
            set => lockOnRotateMax = value;
        }

        // CameraSphereRadius
        private float? cameraSphereRadius;
        private const float defaultCameraSphereRadius = 0.2f;
        public float CameraSphereRadius
        {
            readonly get => cameraSphereRadius ?? defaultCameraSphereRadius;
            set => cameraSphereRadius = value;
        }
    }

    public struct PhysicsData
    {
        [Header("Ground & Air Detection States")]
        // GroundCheckDistance
        private float? groundCheckDis;
        private const float defaultGroundCheckDis = 0.4f;
        public float GroundCheckDis
        {
            readonly get => groundCheckDis ?? defaultGroundCheckDis;
            set => groundCheckDis = value;
        }

        // GroundDetectionRayStart
        private float? groundDetectionRayStart;
        private const float defaultGroundDetectionRayStart = 0.5f;
        public float GroundDetectionRayStart
        {
            readonly get => groundDetectionRayStart ?? defaultGroundDetectionRayStart;
            set => groundDetectionRayStart = value;
        }

        // DistanceBeginFallMin
        private float? distanceBeginFallMin;
        private const float defaultDistanceBeginFallMin = 1;
        public float DistanceBeginFallMin
        {
            readonly get => distanceBeginFallMin ?? defaultDistanceBeginFallMin;
            set => distanceBeginFallMin = value;
        }

        // GroundDirRayDistance
        private float? groundDirRayDistance;
        private const float defaultGroundDirRayDistance = 0.2f;
        public float GroundDirRayDistance
        {
            readonly get => groundDirRayDistance ?? defaultGroundDirRayDistance;
            set => groundDirRayDistance = value;
        }

        [Header("Character Physics")]
        // LookAtSmoothing
        private float? lookAtSmoothing;
        private const float defaultLookAtSmoothing = 25;
        public float LookAtSmoothing 
        {
            readonly get => lookAtSmoothing ?? defaultLookAtSmoothing;
            set => lookAtSmoothing = value;
        }

        // FallingFactor
        private float? fallingFactor;
        private const float defaultFallingFactor = 0.1f;
        public float FallingFactor 
        {
            readonly get => fallingFactor ?? defaultFallingFactor;
            set => fallingFactor = value;
        }

        // FallingSpeedRatio
        private float? fallingSpeedRatio;
        private const float defaultFallingSpeedRatio = 2;
        public float FallingSpeedRatio 
        {
            readonly get => fallingSpeedRatio ?? defaultFallingSpeedRatio;
            set => fallingSpeedRatio = value;
        }

        // FallingFrontForce
        private float? fallingFrontForce;
        private const float defaultFallingFrontForce = 5;
        public float FallingFrontForce 
        {
            readonly get => fallingFrontForce ?? defaultFallingFrontForce;
            set => fallingFrontForce = value;
        }

        // FallingDownForce
        private float? fallingDownForce;
        private const float defaultFallingDownForce = 750;
        public float FallingDownForce
        {
            readonly get => fallingDownForce ?? defaultFallingDownForce;
            set => fallingDownForce = value;
        }

        [Header("Physics Layer")]
        // Ground Layer
        private string groundLayer;
        public string GroundLayer
        {
            readonly get => groundLayer ?? "Ground";
            set => groundLayer = value;
        }
    }

    public struct InteractData
    {
        [Header("Interact Check")]
        // Interact Check Radius
        private float? interactCheckRadius;
        private const float defaultInteractCheckRadius = 0.3f;
        public float InteractCheckRadius
        {
            readonly get => interactCheckRadius ?? defaultInteractCheckRadius;
            set => interactCheckRadius = value;
        }

        // Interact Check Distance
        private float? interactCheckDis;
        private const float defaultInteractCheckDis = 1;
        public float InteractCheckDis
        {
            readonly get => interactCheckDis ?? defaultInteractCheckDis;
            set => interactCheckDis = value;
        }

        // Interact Object Tag
        private string interactObjTag;
        public string InteractObjTag
        {
            readonly get => interactObjTag ?? "Interactable";
            set => interactObjTag = value;
        }

        [Header("Interact Text")]
        // Interact Item Text
        private readonly string itemInteractText;
        public string ItemInteractText
        {
            get => itemInteractText == null ? "아이템을 획득한다 E" : ItemInteractText;
            set => ItemInteractText = value;
        }

        // Interact Message Text
        private readonly string messageInteractText;
        public string MessageInteractText
        {
            get => messageInteractText == null ? "메시지를 확인한다 E" : MessageInteractText;
            set => MessageInteractText = value;
        }

        // Interact Gate Text
        private readonly string gateInteractText;
        public string GateInteractText
        {
            get => gateInteractText == null ? "안개 속으로 들어간다 E" : GateInteractText;
            set => GateInteractText = value;
        }
    }
}