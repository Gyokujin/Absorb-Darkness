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

    public IEnumerator DissolveFade()
    {
        yield return dissolveWait;
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