using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour {

	public List<Rigidbody> rbList = new List<Rigidbody> ();
	public Vector3 direction;
	public float speed;
	public AudioClip[] clipArray;

	void Start()
	{
		GetComponent<AudioSource>().PlayOneShot(clipArray [Random.Range(0, clipArray.Length)], 0.5f);

		foreach (Rigidbody rb in rbList) 
		{
			rb.AddForce (direction * speed, ForceMode.Impulse);
		}
	}
}
