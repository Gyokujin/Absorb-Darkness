using UnityEngine;

namespace PlayerData
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

        // RunAnimationCondition
        private float? _runAnimationCondition;
        private const float defaultRunAnimationCondition = 0.25f;
        public float runAnimationCondition
        {
            get => _runAnimationCondition ?? defaultRunAnimationCondition;
            set => _runAnimationCondition = value;
        }

        [Header("Parameter Name")]
        // Vertical Parameter
        private string _verticalParameter;
        public string verticalParameter
        {
            get => _verticalParameter == null ? "vertical" : _verticalParameter;
            set => _verticalParameter = value;
        }

        // Horizontal Parameter
        private string _horizontalParameter;
        public string horizontalParameter
        {
            get => _horizontalParameter == null ? "horizontal" : _horizontalParameter;
            set => _horizontalParameter = value;
        }

        // OnStance Parameter
        private string _onStanceParameter;
        public string onStanceParameter
        {
            get => _onStanceParameter == null ? "onStance" : _onStanceParameter;
            set => _onStanceParameter = value;
        }

        // OnUsingLeftHand Parameter
        private string _onUsingLeftHand;
        public string onUsingLeftHand
        {
            get => _onUsingLeftHand == null ? "usingLeftHand" : _onUsingLeftHand;
            set => _onUsingLeftHand = value;
        }

        // OnUsingRightHand Parameter
        private string _onUsingRightHand;
        public string onUsingRightHand
        {
            get => _onUsingRightHand == null ? "usingRightHand" : _onUsingRightHand;
            set => _onUsingRightHand = value;
        }

        // OnAttack Parameter
        private string _onAttackParameter;
        public string onAttackParameter
        {
            get => _onAttackParameter == null ? "onAttack" : _onAttackParameter;
            set => _onAttackParameter = value;
        }

        // ComboAble Parameter
        private string _comboAbleParameter;
        public string comboAbleParameter
        {
            get => _comboAbleParameter == null ? "comboAble" : _comboAbleParameter;
            set => _comboAbleParameter = value;
        }
    }
}