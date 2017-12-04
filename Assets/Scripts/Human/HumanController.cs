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

	public AudioSource source;
	public AudioClip shoot;
	public AudioClip reload;

	public Animator anim;
	int frameSincePath;

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

		if (frameSincePath > 0 && anim.GetBool ("isMoving") && !navMesh.hasPath) 
		{
			anim.SetBool ("isMoving", false);
			frameSincePath = 0;
		}

		if (anim.GetBool ("isMoving") && frameSincePath < 1)
			frameSincePath++;
	}

	void SetDestination()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1000, destinationLayer)) 
		{
			navMesh.SetDestination (hit.point);
			anim.SetBool ("isMoving", true);

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

			source.PlayOneShot (shoot, 0.8f);

			ammo--;
			StartCoroutine (Reload ());
			StartCoroutine (hud.UpdateArrows ());
		}
	}

	IEnumerator Reload ()
	{
		canShoot = false;
		source.PlayOneShot (reload, 0.3f);
		reloadSlider.gameObject.SetActive (true);
		yield return new WaitForSeconds (reloadTime);
		reloadSlider.gameObject.SetActive (false);
		canShoot = true;
	}
}
