using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour {

	public RaycastHit hit;
	Ray ray;
	public Transform mouseTransform;
	int layerMask;

	void Start()
	{
		layerMask = LayerMask.GetMask ("Floor");
	}

	void Update() 
	{
		ray = Camera.main.ScreenPointToRay (Input.mousePosition);

		if (Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) 
		{
			mouseTransform.position = new Vector3 (hit.point.x, hit.point.y, hit.point.z);
		}
	}
}
