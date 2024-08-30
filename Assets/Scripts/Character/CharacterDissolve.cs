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
    private float particleOffsetX;
    [SerializeField]
    private float particleOffsetY;
    [SerializeField]
    private float particleOffsetZ;

    [Header("Coroutine")]
    private WaitForSeconds dissolveWait;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        material = GetComponent<Renderer>().material;
        dissolveWait = new WaitForSeconds(shaderData.DissolveDelay);
    }

    public IEnumerator DissolveFade(Transform effectTransform)
    {
        yield return dissolveWait;
        GameObject glowObject = PoolManager.instance.GetEffect((int)PoolManager.Effect.ExtinctionGlow);

        Quaternion rotation = Quaternion.Euler(0, effectTransform.eulerAngles.y, 0);
        Vector3 forwardVector = rotation * Vector3.forward;
        Vector3 targetPos = new(forwardVector.x * particleOffsetX, particleOffsetY, forwardVector.z * particleOffsetZ);

        glowObject.transform.position = effectTransform.position + targetPos;

        //glowObject.transform.position = new Vector3(effectTransform.position.x + forwardVector.x * particleOffsetDis,
        //                                            )
        //    effectTransform.position + (forwardVector * particleOffsetDis); // effectTransform.position + forward * particleOffset;

        material.shader = dissolveShader;
        material.mainTexture = dissolveTexture;
        material.SetFloat(shaderData.TextureBrightness, textureBrightness);
        material.SetColor(shaderData.LightColor, lightColor);
        material.SetVector(shaderData.LightDirection, lightDir);
        material.SetFloat(shaderData.LightIntensity, lightIntensity);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDissolve]);

        while (shaderData.DissolveProgress < dissolveTime)
        {
            shaderData.DissolveProgress += Time.deltaTime / shaderData.DissolveTime;
            material.SetFloat(shaderData.DissolveThresholdParameter, shaderData.DissolveProgress);
            yield return null;
        }
    }
}