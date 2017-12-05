using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerClass {

	public string horizontal;
	public string vertical;
	public float moveSpeed = 0.5f;
	public float lungeSpeed = 5;

	public PlayerClass (string hor, string ver)
	{
		horizontal = hor;
		vertical = ver;
	}
}
