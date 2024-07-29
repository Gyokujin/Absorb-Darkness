using UnityEngine;

namespace PlayerData
{
    public struct PlayerStatusData
    {
        [Header("Stamina")]
        // StaminaLevel
        private int? staminaLevel;
        public int StaminaLevel
        {
            readonly get => staminaLevel ?? 10;
            set => staminaLevel = value;
        }

        // StaminaLevel Amount
        private int? staminaLevelAmount;
        public int StaminaLevelAmount
        {
            readonly get => staminaLevelAmount ?? 10;
            set => staminaLevelAmount = value;
        }

        // Stamina RecoveryAmount
        private float? staminaRecoveryAmount;
        public float StaminaRecoveryAmount
        {
            readonly get => staminaRecoveryAmount ?? 0.55f;
            set => staminaRecoveryAmount = value;
        }

        [Header("Action Condition")]
        // Action LimitStamina
        private float? actionLimitStamina;
        public float ActionLimitStamina
        {
            readonly get => actionLimitStamina ?? 5;
            set => actionLimitStamina = value;
        }

        // RollingStaminaAmount
        private float? rollingStaminaAmount;
        public float RollingStaminaAmount
        {
            readonly get => rollingStaminaAmount ?? 20;
            set => rollingStaminaAmount = value;
        }

        // BackstapStaminaAmount
        private float? backstapStaminaAmount;
        public float BackstapStaminaAmount
        {
            readonly get => backstapStaminaAmount ?? 12.5f;
            set => backstapStaminaAmount = value;
        }

        // SprintStaminaAmount
        private float? sprintStaminaAmount;
        public float SprintStaminaAmount
        {
            readonly get => sprintStaminaAmount ?? 0.5f;
            set => sprintStaminaAmount = value;
        }
    }

    public struct PlayerPhysicsData
    {
        [Header("Move")]
        // Sprint Condition
        private float? runCondition;
        public float RunCondition
        {
            readonly get => runCondition ?? 0.5f;
            set => runCondition = value;
        }

        // Land Requirement
        private float? landRequirement;
        public float LandRequirement
        {
            readonly get => landRequirement ?? 0.5f;
            set => landRequirement = value;
        }

        // Sprint Speed
        private float? sprintSpeed;
        public float SprintSpeed
        {
            readonly get => sprintSpeed ?? 9;
            set => sprintSpeed = value;
        }

        [Header("Battle")]
        // InvincibleTime
        private float? invincibleTime;
        public float InvincibleTime
        {
            readonly get => invincibleTime ?? 0.3f;
            set => invincibleTime = value;
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
        public float IdleParameterValue
        {
            readonly get => idleParameterValue ?? 0;
            set => idleParameterValue = value;
        }

        // Walk ParameterValue
        private float? walkParameterValue;
        public float WalkParameterValue
        {
            readonly get => walkParameterValue ?? 0.5f;
            set => walkParameterValue = value;
        }

        // Run ParameterValue
        private float? runParameterValue;
        public float RunParameterValue
        {
            readonly get => runParameterValue ?? 1;
            set => runParameterValue = value;
        }

        // Sprint ParameterValue
        private float? sprintParameterValue;
        public float SprintParameterValue
        {
            readonly get => sprintParameterValue ?? 2;
            set => sprintParameterValue = value;
        }

        // Animation DampTime
        private float? animationDampTime;
        public float AnimationDampTime
        {
            readonly get => animationDampTime ?? 0.1f;
            set => animationDampTime = value;
        }

        // RunAnimation Condition
        private float? runAnimationCondition;
        public float RunAnimationCondition
        {
            readonly get => runAnimationCondition ?? 0.25f;
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