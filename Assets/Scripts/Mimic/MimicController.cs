using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicController : MonoBehaviour {

	public string horizontal;
	public string vertical;
	float xAxis;
	float zAxis;
	Vector3 direction;
	float speed;
	public float moveSpeed;
	public float lungeSpeed;
	public float lungeTime;
	public float lungeRecovery;
	public float lerpMov;

	Rigidbody rb;
	GameObject human;

	Quaternion playerRot;
	public float rotSpeed;

	bool isBuffering;
	bool holdingDown;
	bool tappedOnce;
	public bool isLunging;

	public float bufferTime;

	AudioSource source;
	//public AudioClip lunge;

	public Animator anim;
	Collider col;

	void Start ()
	{
		rb = GetComponentInParent<Rigidbody> ();
		source = GetComponent<AudioSource> ();
		speed = moveSpeed;
		human = GameObject.FindGameObjectWithTag ("Human");
		anim = GetComponentInParent <Animator> ();
		col = GetComponentInParent <Collider> ();
	}

	void Update () 
	{
		if (!isLunging) 
		{
			GetIsometricInput ();
			if (human != null)
				CheckLungeInput ();

			direction = new Vector3 (xAxis, 0f, zAxis);
		}


		if (direction != Vector3.zero) 
		{
			playerRot = Quaternion.LookRotation (direction);
		}
			
		transform.parent.rotation = Quaternion.Slerp (transform.parent.rotation, playerRot, rotSpeed * Time.deltaTime);

	}
		
	void FixedUpdate()
	{
		direction = direction.normalized * speed;
		direction.y = rb.velocity.y;

		rb.velocity = Vector3.Lerp(rb.velocity, direction, lerpMov);
		//print (direction);
	}

	void GetIsometricInput()
	{
		//Droite
		if (Input.GetAxisRaw (horizontal) > 0 && Input.GetAxisRaw (vertical) == 0) 
		{
			xAxis = 1;
			zAxis = -1;
		}

		//Gauche
		if (Input.GetAxisRaw (horizontal) < 0 && Input.GetAxisRaw (vertical) == 0) 
		{
			xAxis = -1;
			zAxis = 1;
		}

		//Haut
		if (Input.GetAxisRaw (horizontal) == 0 && Input.GetAxisRaw (vertical) > 0) 
		{
			xAxis = 1;
			zAxis = 1;
		}

		//Bas
		if (Input.GetAxisRaw (horizontal) == 0 && Input.GetAxisRaw (vertical) < 0) 
		{
			xAxis = -1;
			zAxis = -1;
		}

		//Diag haut-droite
		if (Input.GetAxisRaw (horizontal) > 0 && Input.GetAxisRaw (vertical) > 0) 
		{
			xAxis = 1;
			zAxis = 0;
		}

		//Diag haut-gauche
		if (Input.GetAxisRaw (horizontal) < 0 && Input.GetAxisRaw (vertical) > 0) 
		{
			xAxis = 0;
			zAxis = 1;
		}

		//Diag bas-droite
		if (Input.GetAxisRaw (horizontal) > 0 && Input.GetAxisRaw (vertical) < 0) 
		{
			xAxis = 0;
			zAxis = -1;
		}

		//Diag bas-gauche
		if (Input.GetAxisRaw (horizontal) < 0 && Input.GetAxisRaw (vertical) < 0) 
		{
			xAxis = -1;
			zAxis = 0;
		}

		//Pas d'input
		if (Input.GetAxisRaw (horizontal) == 0 && Input.GetAxisRaw (vertical) == 0) 
		{
			xAxis = 0;
			zAxis = 0;
		}
	}

	void CheckLungeInput()
	{
		if (!isBuffering) 
		{
			if (Input.GetAxisRaw (vertical) > 0) 
			{
				holdingDown = true;
				StartCoroutine (LungeBuffer ());
			}
		} 

		else 
		{
			if (holdingDown && Input.GetAxisRaw (vertical) == 0) 
			{
				holdingDown = false;
				tappedOnce = true;
			}

			if (tappedOnce && Input.GetAxisRaw (vertical) > 0) 
			{
				tappedOnce = false;
				StopCoroutine (LungeBuffer());
				StartCoroutine (Lunge ());
				isLunging = true;
			}
		}
	}

	IEnumerator Lunge ()
	{

		isLunging = true;
		yield return null;

		rb.velocity = Vector3.zero;
		direction = (human.transform.position - transform.parent.position);
		speed = lungeSpeed;

		anim.SetTrigger ("lunges");

		source.pitch = Random.Range (0.8f, 1.2f);
		source.Play ();

		yield return new WaitForSeconds (lungeTime);

		speed = 0f;

		yield return new WaitForSeconds (lungeRecovery);

		speed = moveSpeed;
		direction = Vector3.zero;
		isLunging = false;

	}

	IEnumerator LungeBuffer ()
	{
		isBuffering = true;
		yield return new WaitForSeconds (bufferTime);
		isBuffering = false;
		tappedOnce = false;
	}

	public void Die()
	{
		col.material = null;
		rb.mass = 1;
		anim.SetTrigger ("dies");
		Destroy (gameObject);
	}
}
