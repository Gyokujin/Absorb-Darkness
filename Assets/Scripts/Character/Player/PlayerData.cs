using UnityEngine;

namespace PlayerDatas
{
    public struct PlayerAnimatorData
    {
        [Header("Paramater")]
        // IdleParameterValue
        private float? _idleParameterValue;
        private const float defaultIdleParameterValue = 0;
        public float idleParameterValue
        {
            get => _idleParameterValue ?? defaultIdleParameterValue;
            set => _idleParameterValue = value;
        }

        // WalkParameterValue
        private float? _walkParameterValue;
        private const float defaultWalkParameterValue = 0.5f;
        public float walkParameterValue
        {
            get => _walkParameterValue ?? defaultWalkParameterValue;
            set => _walkParameterValue = value;
        }

        // RunParameterValue
        private float? _runParameterValue;
        private const float defaultRunParameterValue = 1;
        public float runParameterValue
        {
            get => _runParameterValue ?? defaultRunParameterValue;
            set => _runParameterValue = value;
        }

        // SprintParameterValue
        private float? _sprintParameterValue;
        private const float defaultSprintParameterValue = 2;
        public float sprintParameterValue
        {
            get => _sprintParameterValue ?? defaultSprintParameterValue;
            set => _sprintParameterValue = value;
        }

        // AnimationDampTime
        private float? _animationDampTime;
        private const float defaultAnimationDampTime = 0.1f;
        public float animationDampTime
        {
            get => _animationDampTime ?? defaultAnimationDampTime;
            set => _animationDampTime = value;
        }
    }
}