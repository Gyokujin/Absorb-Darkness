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
    private Shader dissolveShader;
    private Texture2D dissolveTexture;
    [SerializeField]
    private float dissolvePreDealy = 1.5f;
    [SerializeField]
    private float dissolveTime = 1;
    private WaitForSeconds dissolvePreWait;

    [Header("Color")]
    [SerializeField]
    private float textureBrightness = 0.23f;
    [SerializeField]
    private Vector3 lightDir = new(0.87f, 1.17f, -0.29f);
    [SerializeField]
    private Color lightColor = new(0.7372549f, 0.4392157f, 0.4392157f);
    [SerializeField]
    private float lightIntensity = 0.2f;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        material = GetComponent<Renderer>().material;
        dissolveShader = Shader.Find("Custom/CharacterDissolve");
        dissolveTexture = (Texture2D)material.GetTexture("_MainTex");
        dissolvePreWait = new WaitForSeconds(dissolvePreDealy);
    }

    public IEnumerator DissolveFade(EnemyManager enemy)
    {
        yield return dissolvePreWait;
        material.shader = dissolveShader;
        material.mainTexture = dissolveTexture;
        material.SetFloat(shaderData.TextureBrightness, textureBrightness);
        material.SetColor(shaderData.LightColor, lightColor);
        material.SetVector(shaderData.LightDirection, lightDir);
        material.SetFloat(shaderData.LightIntensity, lightIntensity);

        while (shaderData.DissolveProgress < dissolveTime)
        {
            shaderData.DissolveProgress += Time.deltaTime / shaderData.DissolveTime;
            material.SetFloat(shaderData.DissolveThresholdParameter, shaderData.DissolveProgress);
            yield return null;
        }
    }
}