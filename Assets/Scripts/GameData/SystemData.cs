using UnityEngine;

namespace SystemData
{
    public struct CameraData
    {
        [Header("Angle")]
        // MinPivot
        private float? _minPivot;
        private const float defaultMinPivot = -35;
        public float minPivot
        {
            get => _minPivot ?? defaultMinPivot;
            set => _minPivot = value;
        }

        // MaxPivot
        private float? _maxPivot;
        private const float defaultMaxPivot = 35;
        public float maxPivot
        {
            get => _maxPivot ?? defaultMaxPivot;
            set => _maxPivot = value;
        }

        [Header("Camera")]
        // LookSpeed
        private float? _lookSpeed;
        private const float defaultLookSpeed = 0.025f;
        public float lookSpeed
        {
            get => _lookSpeed ?? defaultLookSpeed;
            set => _lookSpeed = value;
        }

        // FollowSpeed
        private float? _followSpeed;
        private const float defaultFollowSpeed = 0.5f;
        public float followSpeed
        {
            get => _followSpeed ?? defaultFollowSpeed;
            set => _followSpeed = value;
        }

        // PivotSpeed
        private float? _pivotSpeed;
        private const float defaultPivotSpeed = 0.01f;
        public float pivotSpeed
        {
            get => _pivotSpeed ?? defaultPivotSpeed;
            set => _pivotSpeed = value;
        }

        // PlayerFollowRate
        private float? _playerFollowRate;
        private const float defaultPlayerFollowRate = 0.2f;
        public float playerFollowRate
        {
            get => _playerFollowRate ?? defaultPlayerFollowRate;
            set => _playerFollowRate = value;
        }

        // CameraCollisionOffset
        private float? _cameraCollisionOffset;
        private const float defaultCameraCollisionOffset = 0.2f;
        public float cameraCollisionOffset
        {
            get => _cameraCollisionOffset ?? defaultCameraCollisionOffset;
            set => _cameraCollisionOffset = value;
        }

        // MinCollisionOffset
        private float? _minCollisionOffset;
        private const float defaultMinCollisionOffset = 0.2f;
        public float minCollisionOffset
        {
            get => _minCollisionOffset ?? defaultMinCollisionOffset;
            set => _minCollisionOffset = value;
        }

        // UnlockedPivotPosition
        private float? _unlockedPivotPositionY;
        private const float defaultUnlockedPivotPositionY = 0.575f;
        public float unlockedPivotPositionY
        {
            get => _unlockedPivotPositionY ?? defaultUnlockedPivotPositionY;
            set => _unlockedPivotPositionY = value;
        }

        // LockedPivotPosition
        private float? _lockedPivotPositionY;
        private const float defaultLockedPivotPositionY = 1.2f;
        public float lockedPivotPositionY
        {
            get => _lockedPivotPositionY ?? defaultLockedPivotPositionY;
            set => _lockOnRotateMax = value;
        }

        [Header("LockOn")]
        // LockOnRadius
        private float? _lockOnRadius;
        private const float defaultLockOnRadius = 26;
        public float lockOnRadius
        {
            get => _lockOnRadius ?? defaultLockOnRadius;
            set => _lockOnRadius = value;
        }

        // MaxLockOnDistance
        private float? _maxLockOnDistance;
        private const float defaultMaxLockOnDistance = 36;
        public float maxLockOnDistance
        {
            get => _maxLockOnDistance ?? defaultMaxLockOnDistance;
            set => _maxLockOnDistance = value;
        }

        // LockOnAngleLimit
        private float? _lockOnAngleLimit;
        private const float defaultLockOnAngleLimit = 50;
        public float lockOnAngleLimit
        {
            get => _lockOnAngleLimit ?? defaultLockOnAngleLimit;
            set => _lockOnAngleLimit = value;
        }

        // LockOnRotateMax
        private float? _lockOnRotateMax;
        private const float defaultLockOnRotateMax = 30;
        public float lockOnRotateMax
        {
            get => _lockOnRotateMax ?? defaultLockOnRotateMax;
            set => _lockOnRotateMax = value;
        }

        // CameraSphereRadius
        private float? _cameraSphereRadius;
        private const float defaultCameraSphereRadius = 0.2f;
        public float cameraSphereRadius
        {
            get => _cameraSphereRadius ?? defaultCameraSphereRadius;
            set => _cameraSphereRadius = value;
        }
    }

    public struct PhysicsData
    {
        [Header("Ground & Air Detection States")]
        // GroundCheckDistance
        private float? _groundCheckDis;
        private const float defaultGroundCheckDis = 0.4f;
        public float groundCheckDis
        {
            get => _groundCheckDis ?? defaultGroundCheckDis;
            set => _groundCheckDis = value;
        }

        // GroundDetectionRayStart
        private float? _groundDetectionRayStart;
        private const float defaultGroundDetectionRayStart = 0.5f;
        public float groundDetectionRayStart
        {
            get => _groundDetectionRayStart ?? defaultGroundDetectionRayStart;
            set => _groundDetectionRayStart = value;
        }

        // DistanceBeginFallMin
        private float? _distanceBeginFallMin;
        private const float defaultDistanceBeginFallMin = 1;
        public float distanceBeginFallMin
        {
            get => _distanceBeginFallMin ?? defaultDistanceBeginFallMin;
            set => _distanceBeginFallMin = value;
        }

        // GroundDirRayDistance
        private float? _groundDirRayDistance;
        private const float defaultGroundDirRayDistance = 0.2f;
        public float groundDirRayDistance
        {
            get => _groundDirRayDistance ?? defaultGroundDirRayDistance;
            set => _groundDirRayDistance = value;
        }

        [Header("Character Physics")]
        // LookAtSmoothing
        private float? _lookAtSmoothing;
        private const float defaultLookAtSmoothing = 25;
        public float lookAtSmoothing 
        {
            get => _lookAtSmoothing ?? defaultLookAtSmoothing;
            set => _lookAtSmoothing = value;
        }

        // FallingFactor
        private float? _fallingFactor;
        private const float defaultFallingFactor = 0.1f;
        public float fallingFactor 
        {
            get => _fallingFactor ?? defaultFallingFactor;
            set => _fallingFactor = value;
        }

        // FallingSpeedRatio
        private float? _fallingSpeedRatio;
        private const float defaultFallingSpeedRatio = 2;
        public float fallingSpeedRatio 
        {
            get => _fallingSpeedRatio ?? defaultFallingSpeedRatio;
            set => _fallingSpeedRatio = value;
        }

        // FallingFrontForce
        private float? _fallingFrontForce;
        private const float defaultFallingFrontForce = 5;
        public float fallingFrontForce 
        {
            get => _fallingFrontForce ?? defaultFallingFrontForce;
            set => _fallingFrontForce = value;
        }

        // FallingDownForce
        private float? _fallingDownForce;
        private const float defaultFallingDownForce = 750;
        public float fallingDownForce
        {
            get => _fallingDownForce ?? defaultFallingDownForce;
            set => _fallingDownForce = value;
        }
    }

    public struct InteractData
    {
        [Header("Interact Check")]
        // Interact Check Radius
        private float? _interactCheckRadius;
        private const float defaultInteractCheckRadius = 0.3f;
        public float interactCheckRadius
        {
            get => _interactCheckRadius ?? defaultInteractCheckRadius;
            set => _interactCheckRadius = value;
        }

        // Interact Check Distance
        private float? _interactCheckDis;
        private const float defaultInteractCheckDis = 1;
        public float interactCheckDis
        {
            get => _interactCheckDis ?? defaultInteractCheckDis;
            set => _interactCheckDis = value;
        }

        // Interact Object Tag
        private string _interactObjTag;
        public string interactObjTag
        {
            get => _interactObjTag == null ? "Interactable" : _interactObjTag;
            set => _interactObjTag = value;
        }

        [Header("Interact Text")]
        // Interact Item Text
        private string _itemInteractText;
        public string itemInteractText
        {
            get => _itemInteractText == null ? "아이템을 획득한다 E" : itemInteractText;
            set => itemInteractText = value;
        }

        // Interact Message Text
        private string _messageInteractText;
        public string messageInteractText
        {
            get => _messageInteractText == null ? "메시지를 확인한다 E" : messageInteractText;
            set => messageInteractText = value;
        }

        // Interact Gate Text
        private string _gateInteractText;
        public string gateInteractText
        {
            get => _gateInteractText == null ? "안개 속으로 들어간다 E" : gateInteractText;
            set => gateInteractText = value;
        }
    }
}