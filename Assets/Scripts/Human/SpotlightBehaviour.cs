using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotlightBehaviour : MonoBehaviour {

	public Transform player;
	public float followLerp;

	void Start()
	{
		transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
	}

	void Update()
	{
		transform.position = new Vector3(Mathf.Lerp(transform.position.x, player.position.x, followLerp), transform.position.y, Mathf.Lerp(transform.position.z, player.position.z, followLerp));
	}
}
