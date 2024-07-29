using UnityEngine;

namespace PlayerData
{
    public struct PlayerStatusData
    {
        [Header("Stamina")]
        // StaminaLevel
        private int? staminaLevel;
        private const int defaultStaminaLevel = 10;
        public int StaminaLevel
        {
            readonly get => staminaLevel ?? defaultStaminaLevel;
            set => staminaLevel = value;
        }

        // StaminaLevel Amount
        private int? staminaLevelAmount;
        private const int defaultStaminaLevelAmount = 10;
        public int StaminaLevelAmount
        {
            readonly get => staminaLevelAmount ?? defaultStaminaLevelAmount;
            set => staminaLevelAmount = value;
        }

        // Action LimitStamina
        private int? actionLimitStamina;
        private const int defaultActionLimitStamina = 5;
        public int ActionLimitStamina
        {
            readonly get => actionLimitStamina ?? defaultActionLimitStamina;
            set => actionLimitStamina = value;
        }
    }

    public struct PlayerPhysicsData
    {
        [Header("Move Physics")]
        // Sprint Condition
        private float? runCondition;
        private const float defaultRunCondition = 0.5f;
        public float RunCondition
        {
            readonly get => runCondition ?? defaultRunCondition;
            set => runCondition = value;
        }

        // Land Requirement
        private float? landRequirement;
        private const float defaultLandRequirement = 0.5f;
        public float LandRequirement
        {
            readonly get => landRequirement ?? defaultLandRequirement;
            set => landRequirement = value;
        }

        [Header("State Layer")]
        // Player DefaultLayer
        private string playerLayer;
        public string PlayerLayer
        {
            readonly get => playerLayer ?? "Player";
            set => playerLayer = value;
        }

        // Player DefaultLayer
        private string invincibleLayer;
        public string InvincibleLayer
        {
            readonly get => invincibleLayer ?? "Invincible";
            set => invincibleLayer = value;
        }
    }

    public struct PlayerAnimatorData
    {
        [Header("Animation")]
        // Empty Animation
        private string emptyAnimation;
        public string EmptyAnimation
        {
            readonly get => emptyAnimation ?? "Empty";
            set => emptyAnimation = value;
        }

        // Land Animation
        private string landAnimation;
        public string LandAnimation
        {
            readonly get => landAnimation ?? "Land";
            set => landAnimation = value;
        }

        // Falling Animation
        private string fallingAnimation;
        public string FallingAnimation
        {
            readonly get => fallingAnimation ?? "Falling";
            set => fallingAnimation = value;
        }

        // Rolling Animation
        private string rollingAnimation;
        public string RollingAnimation
        {
            readonly get => rollingAnimation ?? "Rolling";
            set => rollingAnimation = value;
        }

        // Backstep Animation
        private string backstepAnimation;
        public string BackstepAnimation
        {
            readonly get => backstepAnimation ?? "Backstep";
            set => backstepAnimation = value;
        }

        // Damage Animation
        private string damageAnimation;
        public string DamageAnimation
        {
            readonly get => damageAnimation ?? "Damage";
            set => damageAnimation = value;
        }

        // Dead Animation
        private string deadAnimation;
        public string DeadAnimation
        {
            readonly get => deadAnimation ?? "Dead";
            set => deadAnimation = value;
        }

        [Header("Paramater")]
        // Idle ParameterValue
        private float? idleParameterValue;
        private const float defaultIdleParameterValue = 0;
        public float IdleParameterValue
        {
            readonly get => idleParameterValue ?? defaultIdleParameterValue;
            set => idleParameterValue = value;
        }

        // Walk ParameterValue
        private float? walkParameterValue;
        private const float defaultWalkParameterValue = 0.5f;
        public float WalkParameterValue
        {
            readonly get => walkParameterValue ?? defaultWalkParameterValue;
            set => walkParameterValue = value;
        }

        // Run ParameterValue
        private float? runParameterValue;
        private const float defaultRunParameterValue = 1;
        public float RunParameterValue
        {
            readonly get => runParameterValue ?? defaultRunParameterValue;
            set => runParameterValue = value;
        }

        // Sprint ParameterValue
        private float? sprintParameterValue;
        private const float defaultSprintParameterValue = 2;
        public float SprintParameterValue
        {
            readonly get => sprintParameterValue ?? defaultSprintParameterValue;
            set => sprintParameterValue = value;
        }

        // Animation DampTime
        private float? animationDampTime;
        private const float defaultAnimationDampTime = 0.1f;
        public float AnimationDampTime
        {
            readonly get => animationDampTime ?? defaultAnimationDampTime;
            set => animationDampTime = value;
        }

        // RunAnimation Condition
        private float? runAnimationCondition;
        private const float defaultRunAnimationCondition = 0.25f;
        public float RunAnimationCondition
        {
            readonly get => runAnimationCondition ?? defaultRunAnimationCondition;
            set => runAnimationCondition = value;
        }

        [Header("Parameter Name")]
        // Horizontal Parameter
        private string horizontalParameter;
        public string HorizontalParameter
        {
            readonly get => horizontalParameter ?? "horizontal";
            set => horizontalParameter = value;
        }

        // Vertical Parameter
        private string verticalParameter;
        public string VerticalParameter
        {
            readonly get => verticalParameter ?? "vertical";
            set => verticalParameter = value;
        }

        // InAir Parameter
        private string inAirParameter;
        public string InAirParameter
        {
            readonly get => inAirParameter ?? "isInAir";
            set => inAirParameter = value;
        }

        // Interact Parameter
        private string interactParameter;
        public string InteractParameter
        {
            readonly get => interactParameter ?? "isInteracting";
            set => interactParameter = value;
        }

        // OnAttack Parameter
        private string attackParameter;
        public string AttackParameter
        {
            readonly get => attackParameter ?? "isAttack";
            set => attackParameter = value;
        }

        // ComboAble Parameter
        private string comboAbleParameter;
        public string ComboAbleParameter
        {
            readonly get => comboAbleParameter ?? "isComboAble";
            set => comboAbleParameter = value;
        }

        // IsItemUse Parameter
        private string isItemUseParameter;
        public string IsItemUseParameter
        {
            readonly get => isItemUseParameter ?? "isItemUse";
            set => isItemUseParameter = value;
        }

        // OnStance Parameter
        private string onStanceParameter;
        public string OnStanceParameter
        {
            readonly get => onStanceParameter ?? "onStance";
            set => onStanceParameter = value;
        }

        // OnDamage Parameter
        private string onDamageParameter;
        public string OnDamageParameter
        {
            readonly get => onDamageParameter ?? "onDamage";
            set => onDamageParameter = value;
        }

        // OnUsingLeftHand Parameter
        private string onUsingLeftHand;
        public string OnUsingLeftHand
        {
            readonly get => onUsingLeftHand ?? "usingLeftHand";
            set => onUsingLeftHand = value;
        }

        // OnUsingRightHand Parameter
        private string onUsingRightHand;
        public string OnUsingRightHand
        {
            readonly get => onUsingRightHand ?? "usingRightHand";
            set => onUsingRightHand = value;
        }
    }
}