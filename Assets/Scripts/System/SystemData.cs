using UnityEngine;

namespace SystemDatas
{
    public struct SystemData
    {
        [Header("Angle")]
        public float _minPivot;
        public float _maxPivot;

        public float minPivot
        {
            get => _minPivot == 0 ? -35 : _minPivot;
            set => _minPivot = value;
        }
        
        public float maxPivot
        {
            get => _maxPivot == 0 ? 35 : _maxPivot;
            set => _maxPivot = value;
        }

        [Header("Camera")]
        public float _lookSpeed;
        public float _followSpeed;
        public float _pivotSpeed;
        public float _playerFollowRate;

        public float lookSpeed 
        {
            get => _lookSpeed == 0 ? 0.025f : _lookSpeed;
            set => _lookSpeed = value;
        }

        public float followSpeed
        {
            get => _followSpeed == 0 ? 0.5f : _followSpeed;
            set => _followSpeed = value;
        }

        public float pivotSpeed
        {
            get => _pivotSpeed == 0 ? 0.01f : _pivotSpeed;
            set => _pivotSpeed = value;
        }

        public float playerFollowRate
        {
            get => _playerFollowRate == 0 ? 0.2f : _playerFollowRate;
            set => _playerFollowRate = value;
        }
    }
}