using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShaderData;

public class CharacterGlow : MonoBehaviour
{
    private EnemyManager enemy;

    [Header("Data")]
    private CharacterShaderData shaderData;

    [Header("Particle")]
    [SerializeField]
    private Transform glowEffectTransform;
    [SerializeField]
    private Transform flashEffectTransform;

    [Header("Coroutine")]
    private WaitForSeconds glowDelayWait;
    private WaitForSeconds glowWait;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        enemy = GetComponentInParent<EnemyManager>();
        glowDelayWait = new WaitForSeconds(shaderData.GlowDelay);
        glowWait = new WaitForSeconds(shaderData.GlowTime);
    }

    public IEnumerator Glow()
    {
        yield return glowDelayWait;
        GameObject glowObject = PoolManager.instance.GetEffect((int)PoolManager.Effect.ExtinctionGlow);
        glowObject.transform.position = glowEffectTransform.position;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDieGlow]);

        yield return glowWait;
        enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.DeadAnimation, true);

        yield return glowDelayWait;
        GameObject flashObject = PoolManager.instance.GetEffect((int)PoolManager.Effect.ExtinctionFlash);
        flashObject.transform.position = flashEffectTransform.transform.position;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDieFlash]);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDissolve]);
        StartCoroutine(GetComponent<CharacterDissolve>().DissolveFade(enemy));
    }
}