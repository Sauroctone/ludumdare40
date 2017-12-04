using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShakeGenerator : MonoBehaviour {

	float shakeTimer;
	float shakeAmount;
	Vector3 originPos;

	void Start ()
	{
		originPos = transform.position;
	}

	void Update ()
	{
		if (shakeTimer > 0) 
		{
			Vector2 shakeVector = Random.insideUnitCircle * shakeAmount;
			transform.position = new Vector3 (transform.position.x + shakeVector.x, transform.position.y, transform.position.z + shakeVector.y); 
			shakeTimer -= Time.deltaTime;
		} 

		else 
		{
			transform.position = originPos;
		}
	}

	public void ShakeScreen (float shakeTmr, float shakeAmt)
	{
		shakeTimer = shakeTmr;
		shakeAmount = shakeAmt;
	}
}
