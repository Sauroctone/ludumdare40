using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour {

	void OnCollisionEnter (Collision col)
	{
		if (col.gameObject.tag == "Furniture") 
		{
			Transform mimic = col.transform.Find ("Mimic(Clone)");

			if (mimic != null && mimic.GetComponent<MimicController>().isLunging)
			{
				Die ();
			}
		}
	}

	void Die ()
	{
		RoundManager manager = Camera.main.GetComponent<RoundManager> ();
		manager.round = Rounds.End;
		manager.winner = Winner.Mimics;
		Destroy (gameObject);
	}
}
