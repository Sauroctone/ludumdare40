using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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

	void Start ()
	{
		navMesh = GetComponent<NavMeshAgent> ();
	}

	void Update()
	{
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

			ammo--;
			if (ammo > 0)
				StartCoroutine (Reload ());
		}
	}

	IEnumerator Reload ()
	{
		canShoot = false;
		yield return new WaitForSeconds (reloadTime);
		canShoot = true;
	}
}
