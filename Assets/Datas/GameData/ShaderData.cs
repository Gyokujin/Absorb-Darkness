using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShaderData
{
    public struct CharacterShaderData
    {
        [Header("Dissolve")]
        // DissolveBeforeDelay
        private float? dissolveBeforeDelay;
        public float DissolveBeforeDelay
        {
            readonly get => dissolveBeforeDelay ?? 3;
            set => dissolveBeforeDelay = value;
        }

        // DissolveTime
        private float? dissolveTime;
        public float DissolveTime
        {
            readonly get => dissolveTime ?? 2;
            set => dissolveTime = value;
        }
    }
}