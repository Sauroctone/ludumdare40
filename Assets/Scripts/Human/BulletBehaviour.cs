using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	Vector3 viewPos;
	bool isFlying = true;

	Rigidbody rb;
	public HumanController human;
	public GameObject splatter;

	AudioSource source;
	public AudioClip hitWood;
	public AudioClip hitMimic;
	public AudioClip boltRecup;

	void Start()
	{
		rb = GetComponent<Rigidbody> ();
		source = GameObject.Find ("SoundManager").GetComponent<AudioSource>();
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
					ScreenShakeGenerator shake = Camera.main.GetComponent<ScreenShakeGenerator> ();
					shake.ShakeScreen (0.2f, 0.2f);

					source.PlayOneShot (hitMimic);

					GameObject.Instantiate (splatter, new Vector3 (mimic.position.x, 0.01f, mimic.position.z), mimic.rotation);

					MimicManager mimics = Camera.main.GetComponent<MimicManager> ();
					mimics.mimicList.Remove (mimic.gameObject);

					if (mimics.mimicList.Count == 0) 
					{
						RoundManager manager = Camera.main.GetComponent<RoundManager> ();
						manager.round = Rounds.End;
						manager.winner = Winner.Human;
					}

					mimic.GetComponent<MimicController> ().Die ();

					if (human.ammo < 3) 
					{
						human.ammo++;
						//source.PlayDelayed (boltRecup, 0.5f);
						StartCoroutine (human.hud.UpdateArrows ());
					}
				} 

				else
					source.PlayOneShot (hitWood);
			}

			Destroy (gameObject.GetComponent<BulletBehaviour> ());
			Destroy (gameObject.GetComponent<Collider> ());
			Destroy (gameObject.GetComponent<Rigidbody> ());
		} 

		else
			Destroy (gameObject);
	}
}
