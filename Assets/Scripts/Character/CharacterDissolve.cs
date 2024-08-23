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
    private Material material;

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
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.EnemyDissolve]);

        while (shaderData.DissolveProgress < dissolveTime)
        {
            shaderData.DissolveProgress += Time.deltaTime / shaderData.DissolveTime;
            material.SetFloat(shaderData.DissolveThresholdParameter, shaderData.DissolveProgress);
            yield return null;
        }
    }
}