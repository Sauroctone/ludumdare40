using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanDeath : MonoBehaviour {

	public GameObject particle;
	public AudioSource source;
	public AudioClip death;

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
		ScreenShakeGenerator shake = Camera.main.GetComponent<ScreenShakeGenerator> ();
		shake.ShakeScreen (0.2f, 0.2f);
		RoundManager manager = Camera.main.GetComponent<RoundManager> ();
		manager.round = Rounds.End;
		manager.winner = Winner.Mimics;
		Instantiate (particle, transform.position, particle.transform.rotation);
		source.PlayOneShot (death);
		Destroy (gameObject);
	}
}
