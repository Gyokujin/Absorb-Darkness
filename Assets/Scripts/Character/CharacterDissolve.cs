using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShaderData; 

[RequireComponent(typeof(Renderer))]
public class CharacterDissolve : MonoBehaviour
{
    [Header("Data")]
    private CharacterShaderData shaderData;

    [Header("Shader Property")]
    [SerializeField]
    private Shader dissolveShader;
    [SerializeField]
    private Texture2D dissolveTexture;
    [SerializeField]
    private float dissolveTime = 1;

    [Header("Coroutine")]
    private WaitForSeconds dissolveWait;

    [Header("Component")]
    private Material material;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        shaderData = new CharacterShaderData();
        material = GetComponent<Renderer>().material;
        dissolveWait = new WaitForSeconds(shaderData.DissolveDelay);
    }

    public IEnumerator DissolveFade()
    {
        yield return dissolveWait;
        material.shader = dissolveShader;
        material.mainTexture = dissolveTexture;
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Dissolve]);

        while (shaderData.DissolveProgress < dissolveTime)
        {
            shaderData.DissolveProgress += Time.deltaTime / shaderData.DissolveTime;
            material.SetFloat(shaderData.DissolveThresholdParameter, shaderData.DissolveProgress);
            yield return null;
        }
    }
}