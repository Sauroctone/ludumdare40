using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HumanController : MonoBehaviour {

	NavMeshAgent navMesh;
	Vector3 targetPos;

	public Transform bulletSpawner;
	public GameObject bulletPrefab;
	GameObject bulletInst;

	public float bulletSpeed;
	public float ammo;
	bool canShoot = true;
	public float reloadTime;

	public LayerMask destinationLayer;

	public ArrowUI hud;
	public Slider reloadSlider;
	public GameObject projector;
	GameObject projInstance;

	void Start ()
	{
		navMesh = GetComponent<NavMeshAgent> ();
	}

	void Update()
	{
		//print (transform.position);

		if (Input.GetMouseButtonDown (0))
		{
			SetDestination ();
		}

		if (Input.GetMouseButtonDown (1) && canShoot && ammo > 0) 
		{
			Shoot ();
		}
	}

	void SetDestination()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1000, destinationLayer)) 
		{
			navMesh.SetDestination (hit.point);

			if (projInstance != null)
				Destroy (projInstance);
			
			projInstance = GameObject.Instantiate (projector, new Vector3(hit.point.x, projector.transform.position.y, hit.point.z), projector.transform.rotation) as GameObject;
		}
	}

	void Shoot()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit)) 
		{
			navMesh.ResetPath ();
		
			transform.LookAt (new Vector3 (hit.point.x, transform.position.y, hit.point.z));
				
			bulletInst = GameObject.Instantiate (bulletPrefab, bulletSpawner.position, bulletSpawner.rotation) as GameObject;
			bulletInst.GetComponent<Rigidbody> ().AddForce (bulletInst.transform.forward * bulletSpeed, ForceMode.Impulse);
			bulletInst.GetComponent<BulletBehaviour> ().human = GetComponent<HumanController> ();

			ammo--;
			StartCoroutine (Reload ());
			StartCoroutine (hud.UpdateArrows ());
		}
	}

	IEnumerator Reload ()
	{
		canShoot = false;
		reloadSlider.gameObject.SetActive (true);
		yield return new WaitForSeconds (reloadTime);
		reloadSlider.gameObject.SetActive (false);
		canShoot = true;
	}
}
