using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShaderData; 

[RequireComponent(typeof(Renderer))]
public class CharacterDissolve : MonoBehaviour
{
    [Header("Data")]
    private CharacterShaderData shaderData;
    private Material material;

    [Header("Dissolve")]
    [SerializeField]
    private Shader dissolveShader;
    [SerializeField]
    private Texture2D dissolveTexture;
    [SerializeField]
    private float dissolveTime = 1;

    [Header("Color")]
    [SerializeField]
    private float textureBrightness;
    [SerializeField]
    private Vector3 lightDir;
    [SerializeField]
    private Color lightColor;
    [SerializeField]
    private float lightIntensity;

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
        material = GetComponent<Renderer>().material;
        glowDelayWait = new WaitForSeconds(shaderData.GlowDelay);
        glowWait = new WaitForSeconds(shaderData.GlowTime);
    }

    public IEnumerator DissolveFade(EnemyManager enemy)
    {
        yield return glowDelayWait;
        GameObject glowObject = PoolManager.instance.GetEffect((int)PoolManager.Effect.ExtinctionGlow);
        glowObject.transform.position = glowEffectTransform.position;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDieGlow]);

        material.shader = dissolveShader;
        material.mainTexture = dissolveTexture;
        material.SetFloat(shaderData.TextureBrightness, textureBrightness);
        material.SetColor(shaderData.LightColor, lightColor);
        material.SetVector(shaderData.LightDirection, lightDir);
        material.SetFloat(shaderData.LightIntensity, lightIntensity);

        yield return glowWait;
        enemy.enemyAnimator.PlayTargetAnimation(enemy.characterAnimatorData.DeadAnimation, true);

        yield return glowDelayWait;
        GameObject flashObject = PoolManager.instance.GetEffect((int)PoolManager.Effect.ExtinctionFlash);
        flashObject.transform.position = flashEffectTransform.transform.position;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDieFlash]);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDissolve]);

        while (shaderData.DissolveProgress < dissolveTime)
        {
            shaderData.DissolveProgress += Time.deltaTime / shaderData.DissolveTime;
            material.SetFloat(shaderData.DissolveThresholdParameter, shaderData.DissolveProgress);
            yield return null;
        }
    }
}