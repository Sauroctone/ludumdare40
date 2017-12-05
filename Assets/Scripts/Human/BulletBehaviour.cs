using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
			/*rb.isKinematic = true;
			transform.parent = col.transform;
			isFlying = false;*/

			if (col.gameObject.tag == "Furniture") 
			{
				Transform mimic = col.transform.Find ("Mimic(Clone)");

				if (mimic != null) 
				{
					//Screenshake
					ScreenShakeGenerator shake = Camera.main.GetComponent<ScreenShakeGenerator> ();
					shake.ShakeScreen (0.1f, 0.2f);

					//Sound
					source.PlayOneShot (hitMimic);

					//Blood
					Instantiate (splatter, new Vector3 (mimic.position.x, 0.01f, mimic.position.z), mimic.rotation);

					//Remove from list
					MimicManager mimics = Camera.main.GetComponent<MimicManager> ();
					mimics.mimicList.Remove (mimic.gameObject);

					//Win condition
					if (mimics.mimicList.Count == 0) 
					{
						RoundManager manager = Camera.main.GetComponent<RoundManager> ();
						manager.round = Rounds.End;
						manager.winner = Winner.Human;
					}

					//Kill mimic
					mimic.GetComponent<MimicController> ().Die ();

					//Gain arrow
					if (human.ammo < 3) 
					{
						human.ammo++;
						//source.PlayDelayed (boltRecup, 0.5f);
						StartCoroutine (human.hud.UpdateArrows ());
					}
				} 

				else 
				{
					source.PlayOneShot (hitWood);
					col.gameObject.GetComponent<NavMeshObstacle> ().enabled = false;
					col.gameObject.GetComponent<Rigidbody> ().isKinematic = true;
					col.collider.enabled = false;

					Transform brokenMesh = col.transform.Find("Mesh_Broken");
					Transform mesh = col.transform.Find ("Mesh");
					brokenMesh.gameObject.SetActive (true);
					mesh.gameObject.SetActive (false);
					Vector3 force = rb.velocity.normalized;
					force.y = 0;
					brokenMesh.GetComponent<Destructible> ().direction = force;

					//Remove from list
					MimicManager mimics = Camera.main.GetComponent<MimicManager> ();
					mimics.furnitureList.Remove (col.transform);
				}
			}
		} 

		Destroy (gameObject);
	}
}
