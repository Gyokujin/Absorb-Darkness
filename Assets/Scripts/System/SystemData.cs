using UnityEngine;

namespace SystemDatas
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
        [Header("Physics")]
        private float? _lookAtSmoothing;
        private const float defaultLookAtSmoothing = 25;
        public float lookAtSmoothing 
        {
            get => _lookAtSmoothing ?? defaultLookAtSmoothing;
            set => _lookAtSmoothing = value;
        }

        private float? _fallingFactor;
        private const float defaultFallingFactor = 0.1f;
        public float fallingFactor 
        {
            get => _fallingFactor ?? defaultFallingFactor;
            set => _fallingFactor = value;
        }

        private float? _fallingSpeedRatio;
        private const float defaultFallingSpeedRatio = 2;
        public float fallingSpeedRatio 
        {
            get => _fallingSpeedRatio ?? defaultFallingSpeedRatio;
            set => _fallingSpeedRatio = value;
        }

        private float? _fallingFrontForce;
        private const float defaultFallingFrontForce = 5;
        public float fallingFrontForce 
        {
            get => _fallingFrontForce ?? defaultFallingFrontForce;
            set => _fallingFrontForce = value;
        }

        private float? _fallingDownForce;
        private const float defaultFallingDownForce = 750;
        public float fallingDownForce
        {
            get => _fallingDownForce ?? defaultFallingDownForce;
            set => _fallingDownForce = value;
        }
    }
}