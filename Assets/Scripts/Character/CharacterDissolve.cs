using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShaderData; 

[RequireComponent(typeof(Renderer))]
public class CharacterDissolve : MonoBehaviour
{
    [Header("Data")]
    private CharacterShaderData characterShaderData;

    public Texture2D dissolveTexture;
    public Color dissolveColor = Color.red;

    private Material material;
    private float dissolveProgress = 0.3f;

    void Awake()
    {
        Init();
    }

    void Init()
    {
        characterShaderData = new CharacterShaderData();
    }

    private void Start()
    {
        StartCoroutine(DissolveAfterDelay());
    }

    private IEnumerator DissolveAfterDelay()
    {
        yield return new WaitForSeconds(characterShaderData.DissolveBeforeDelay);

        material = GetComponent<Renderer>().material;
        material.shader = Shader.Find("Custom/Dissolve");
        material.SetTexture("_DissolveTex", dissolveTexture);
        material.SetColor("_DissolveColor", dissolveColor);
        AudioManager.instance.PlaySystemSFX(AudioManager.instance.systemClips[(int)AudioManager.SystemSound.Dissolve]);

        while (dissolveProgress < 1f)
        {
            dissolveProgress += Time.deltaTime / characterShaderData.DissolveTime;
            material.SetFloat("_DissolveThreshold", dissolveProgress);
            yield return null;
        }

        Destroy(gameObject);
    }
}