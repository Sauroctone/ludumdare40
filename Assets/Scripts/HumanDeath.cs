using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour {

	void Die ()
	{
		print ("ded human");
		Destroy (gameObject);
	}

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Furniture") 
		{
			Transform mimic = col.transform.Find ("Mimic(Clone)");

			if (mimic != null && mimic.GetComponent<MimicController>().isLunging)
			{
				print ("ded human");
				Destroy (gameObject);
			}
		}
	}
}
