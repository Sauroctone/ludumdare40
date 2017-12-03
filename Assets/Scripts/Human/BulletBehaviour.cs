using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	Vector3 viewPos;
	bool isFlying = true;

	Rigidbody rb;
	public HumanController human;

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
		if (col.gameObject.tag != "BottomWall") 
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
					MimicManager mimics = Camera.main.GetComponent<MimicManager>();
					mimics.mimicList.Remove(mimic.gameObject);

					if (mimics.mimicList.Count == 0) 
					{
						RoundManager manager = Camera.main.GetComponent<RoundManager> ();
						manager.round = Rounds.End;
						manager.winner = Winner.Human;
					}

					Destroy (mimic.gameObject);
					print ("ded mimic");

					if (human.ammo < 3) 
					{
						human.ammo++;
						StartCoroutine (human.hud.UpdateArrows ());
					}
				}
			}

			Destroy (gameObject.GetComponent<BulletBehaviour> ());
			Destroy (gameObject.GetComponent<Collider> ());
			Destroy (gameObject.GetComponent<Rigidbody> ());
		} 

		else
			Destroy (gameObject);
	}
}
