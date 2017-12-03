using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicAnimation : MonoBehaviour {

	Rigidbody rb;
	//public Transform transformPar;
	Quaternion originRot;

	public float resetSpeed;
	public float rotateSpeed;
	float rotDir = 1;

	bool isMoving;

	void Start ()
	{
		rb = GetComponentInParent<Rigidbody> ();
		originRot = transform.rotation;
	}

	void Update () 
	{
		if (!isMoving && rb.velocity != new Vector3 (0f, rb.velocity.y, 0f)) 
		{
			isMoving = true; 
			StartCoroutine(RotateZAxis());
		}

		if (rb.velocity != new Vector3 (0f, rb.velocity.y, 0f))
		{
			if (isMoving) 
			{
				StopCoroutine (RotateZAxis ());
				isMoving = false;
			}

			transform.rotation = Quaternion.Slerp (transform.rotation, originRot, resetSpeed * Time.deltaTime);
		}
	}

	IEnumerator RotateZAxis ()
	{
		Quaternion newRot = Quaternion.Euler (0f, 0f, rotDir * Random.Range (10f, 15f));

		while (transform.rotation.y - rotDir > 0.1f) 
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, newRot, rotateSpeed * Time.deltaTime);
			yield return null;
		}

		rotDir = -rotDir;

		StartCoroutine (RotateZAxis ());
	}
}
