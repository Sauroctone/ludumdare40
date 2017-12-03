using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	Vector3 viewPos;
	bool isFlying = true;

	Rigidbody rb;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
	}

	void Update () 
	{
		if (isFlying) 
		{
			viewPos = Camera.main.WorldToViewportPoint (transform.position);

			if (viewPos.x > 1 || viewPos.x < 0 || viewPos.y > 1 || viewPos.y < 0)
				Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision col)
	{
		rb.isKinematic = true;
		transform.parent = col.transform;
		isFlying = false;

		if (col.gameObject.tag == "Furniture") 
		{
			Transform mimic = col.transform.Find ("Mimic(Clone)");

			if (mimic != null)
			{
				//tache de sang
				//changer son mesh
				Destroy (mimic.gameObject);
				print ("ded mimic");
			}
		}

		Destroy (gameObject.GetComponent<BulletBehaviour> ());
		Destroy (gameObject.GetComponent<Collider> ());
		Destroy (gameObject.GetComponent<Rigidbody> ());
	}
}
