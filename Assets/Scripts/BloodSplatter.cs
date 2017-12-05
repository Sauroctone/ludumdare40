using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodSplatter : MonoBehaviour {

	public Material[] splatters;

	Renderer rend;

	public float lerpSpeed;

	void Start()
	{
		rend = GetComponent<Renderer> ();
		rend.material = splatters[Mathf.RoundToInt(Random.Range(0, splatters.Length))];
		StartCoroutine (LerpBlood ());
	}

	IEnumerator LerpBlood()
	{
		float _lerp = 0;

		while (_lerp < 1) 
		{
			rend.material.SetFloat ("_Grow", _lerp);
			_lerp += Time.deltaTime * lerpSpeed;
//			print (_lerp);

			yield return null;
		}
	}
}
