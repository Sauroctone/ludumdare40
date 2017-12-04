using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class HumanController : MonoBehaviour {

	NavMeshAgent navMesh;
	Vector3 targetPos;
	Vector3 shootTarget;

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

	public float extraRotSpeed;

	public LayerMask shootLayer;

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

		ExtraRotation ();
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

	void ExtraRotation()
	{
		Vector3 lookrotation = navMesh.steeringTarget - transform.position;
		if (lookrotation != Vector3.zero)
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(lookrotation), extraRotSpeed * Time.deltaTime);
	}

	void Shoot()
	{
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast (ray, out hit, 1000, shootLayer)) 
		{
			//Stop movement
			navMesh.ResetPath ();

			//Trigger anims
			anim.SetTrigger ("shoots");
			anim.SetBool ("isMoving", false);
			frameSincePath = 0;

			//Turn to target
			transform.LookAt (new Vector3 (hit.point.x, transform.position.y, hit.point.z));

			//Instantiate bullet
			bulletInst = GameObject.Instantiate (bulletPrefab, bulletSpawner.position, bulletSpawner.rotation) as GameObject;

			if (hit.collider.tag == "Furniture") 
			{
				shootTarget = (hit.transform.position - transform.position);
				shootTarget = new Vector3 (shootTarget.x, 0f, shootTarget.z).normalized;	 
			}

			else 
			{
				//Get target 
				shootTarget = (hit.point - transform.position);
				shootTarget = new Vector3 (shootTarget.x, 0f, shootTarget.z).normalized;
			}

			//Add force in direction of target
			bulletInst.GetComponent<Rigidbody> ().AddForce (shootTarget * bulletSpeed, ForceMode.Impulse);

			//assign humancontroller to bullet
			bulletInst.GetComponent<BulletBehaviour> ().human = GetComponent<HumanController> ();

			//Sound
			source.PlayOneShot (shoot, 0.8f);

			//Reload
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
