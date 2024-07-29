using UnityEngine;

namespace PlayerData
{
    public struct PlayerStatusData
    {
        [Header("Stamina")]
        // Stamina Level
        private float? _staminaLevel;
        private const float defaultStaminaLevel = 10;
        public float staminaLevel
        {
            get => _staminaLevel ?? defaultStaminaLevel;
            set => _staminaLevel = value;
        }
    }

    public struct PlayerPhysicsData
    {
        [Header("Move Physics")]
        // Sprint Condition
        private float? _runCondition;
        private const float defaultRunCondition = 0.5f;
        public float runCondition
        {
            get => _runCondition ?? defaultRunCondition;
            set => _runCondition = value;
        }

        [Header("State Layer")]
        // Player DefaultLayer
        private string _playerLayer;
        public string playerLayer
        {
            get => _playerLayer == null ? "Player" : _playerLayer;
            set => _playerLayer = value;
        }

        // Player DefaultLayer
        private string _invincibleLayer;
        public string invincibleLayer
        {
            get => _invincibleLayer == null ? "Invincible" : _invincibleLayer;
            set => _invincibleLayer = value;
        }
    }

    public struct PlayerAnimatorData
    {
        [Header("Animation")]
        // Empty Animation
        private string _emptyAnimation;
        public string emptyAnimation
        {
            get => _emptyAnimation == null ? "Empty" : _emptyAnimation;
            set => _emptyAnimation = value;
        }

        // Land Animation
        private string _landAnimation;
        public string landAnimation
        {
            get => _landAnimation == null ? "Land" : _landAnimation;
            set => _landAnimation = value;
        }

        // Falling Animation
        private string _fallingAnimation;
        public string fallingAnimation
        {
            get => _fallingAnimation == null ? "Falling" : _fallingAnimation;
            set => _fallingAnimation = value;
        }

        // Damage Animation
        private string _damageAnimation;
        public string damageAnimation
        {
            get => _damageAnimation == null ? "Damage" : _damageAnimation;
            set => _damageAnimation = value;
        }

        // Dead Animation
        private string _deadAnimation;
        public string deadAnimation
        {
            get => _deadAnimation == null ? "Dead" : _deadAnimation;
            set => _deadAnimation = value;
        }

        [Header("Paramater")]
        // Idle ParameterValue
        private float? _idleParameterValue;
        private const float defaultIdleParameterValue = 0;
        public float idleParameterValue
        {
            get => _idleParameterValue ?? defaultIdleParameterValue;
            set => _idleParameterValue = value;
        }

        // Walk ParameterValue
        private float? _walkParameterValue;
        private const float defaultWalkParameterValue = 0.5f;
        public float walkParameterValue
        {
            get => _walkParameterValue ?? defaultWalkParameterValue;
            set => _walkParameterValue = value;
        }

        // Run ParameterValue
        private float? _runParameterValue;
        private const float defaultRunParameterValue = 1;
        public float runParameterValue
        {
            get => _runParameterValue ?? defaultRunParameterValue;
            set => _runParameterValue = value;
        }

        // Sprint ParameterValue
        private float? _sprintParameterValue;
        private const float defaultSprintParameterValue = 2;
        public float sprintParameterValue
        {
            get => _sprintParameterValue ?? defaultSprintParameterValue;
            set => _sprintParameterValue = value;
        }

        // Animation DampTime
        private float? _animationDampTime;
        private const float defaultAnimationDampTime = 0.1f;
        public float animationDampTime
        {
            get => _animationDampTime ?? defaultAnimationDampTime;
            set => _animationDampTime = value;
        }

        // RunAnimation Condition
        private float? _runAnimationCondition;
        private const float defaultRunAnimationCondition = 0.25f;
        public float runAnimationCondition
        {
            get => _runAnimationCondition ?? defaultRunAnimationCondition;
            set => _runAnimationCondition = value;
        }

        [Header("Parameter Name")]
        // Horizontal Parameter
        private string _horizontalParameter;
        public string horizontalParameter
        {
            get => _horizontalParameter == null ? "horizontal" : _horizontalParameter;
            set => _horizontalParameter = value;
        }

        // Vertical Parameter
        private string _verticalParameter;
        public string verticalParameter
        {
            get => _verticalParameter == null ? "vertical" : _verticalParameter;
            set => _verticalParameter = value;
        }

        // InAir Parameter
        private string _inAirParameter;
        public string inAirParameter
        {
            get => _inAirParameter == null ? "isInAir" : _inAirParameter;
            set => _inAirParameter = value;
        }

        // Interact Parameter
        private string _interactParameter;
        public string interactParameter
        {
            get => _interactParameter == null ? "isInteracting" : _interactParameter;
            set => _interactParameter = value;
        }

        // OnAttack Parameter
        private string _attackParameter;
        public string attackParameter
        {
            get => _attackParameter == null ? "isAttack" : _attackParameter;
            set => _attackParameter = value;
        }

        // ComboAble Parameter
        private string _comboAbleParameter;
        public string comboAbleParameter
        {
            get => _comboAbleParameter == null ? "isComboAble" : _comboAbleParameter;
            set => _comboAbleParameter = value;
        }

        // IsItemUse Parameter
        private string _isItemUseParameter;
        public string isItemUseParameter
        {
            get => _isItemUseParameter == null ? "isItemUse" : _isItemUseParameter;
            set => _isItemUseParameter = value;
        }

        // OnStance Parameter
        private string _onStanceParameter;
        public string onStanceParameter
        {
            get => _onStanceParameter == null ? "onStance" : _onStanceParameter;
            set => _onStanceParameter = value;
        }

        // OnDamage Parameter
        private string _onDamageParameter;
        public string onDamageParameter
        {
            get => _onDamageParameter == null ? "onDamage" : _onDamageParameter;
            set => _onDamageParameter = value;
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
    }
}