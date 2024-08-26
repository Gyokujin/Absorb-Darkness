using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFade : MonoBehaviour {

    [SerializeField]
    private float fadeTime = 0f;
    private float t = 0f;
    private new Light light;
	
	void Awake()
	{
		Init();
	}
	
	void Init()
	{
		light = GetComponent<Light>();
	}

	void Start()
	{
		StartCoroutine(Fade());
	}
	
	IEnumerator Fade()
	{
		t = 0.0f;
		
		while (t < fadeTime)
		{
			t += Time.deltaTime;
			light.intensity = Mathf.Lerp(1f, 0f, t / fadeTime);
			yield return null;
		}
	}
}
