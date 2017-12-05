using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicController : MonoBehaviour {

	public string horizontal;
	public string vertical;
	float xAxis;
	float zAxis;
	Vector3 direction;
	public float speed;
	public float moveSpeed;
	public float lungeSpeed;
	public float lungeTime;
	public float lungeRecovery;
	public float lerpMov;

	Rigidbody rb;
	GameObject human;

	Quaternion playerRot;
	public float rotSpeed;

	public float bufferTime;
	bool singlePressed;
	bool isBuffering;
	bool pressedOnce;
	public bool isLunging;

	AudioSource source;
	//public AudioClip lunge;

	public Animator anim;
	Collider col;

	enum Inputs {Up, Down, Left, Right};
	Inputs lastInput;

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
			playerRot = Quaternion.Euler (0, playerRot.eulerAngles.y, 0);
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
		//on checke si une unique touche d'input est pressée

		if (Input.GetAxisRaw (horizontal) != 0 && Input.GetAxisRaw (vertical) == 0
		    || Input.GetAxisRaw (horizontal) == 0 && Input.GetAxisRaw (vertical) != 0) 
		{
			singlePressed = true;
		}

		else
			singlePressed = false;


		//Si une touche est pressée, sans buffer

		if (singlePressed && !isBuffering) 
		{
			//On save l'input
			if (Input.GetAxisRaw (horizontal) == 1)
				lastInput = Inputs.Right;
			if (Input.GetAxisRaw (horizontal) == -1)
				lastInput = Inputs.Left;
			if (Input.GetAxisRaw (vertical) == 1)
				lastInput = Inputs.Up;
			if (Input.GetAxisRaw (vertical) == -1)
				lastInput = Inputs.Down;

			pressedOnce = false;

			//On lance le buffer
			StartCoroutine(LungeBuffer());

			//print ("new input : " + lastInput);
		}

		//Si le buffer est en cours mais que la touche n'a pas été relâchée

		if (isBuffering && !pressedOnce) 
		{
			//On checke si la touche est relâchée

			if (Input.GetAxisRaw (horizontal) == 0) 
			{
				if (lastInput == Inputs.Right || lastInput == Inputs.Left) 
				{
					pressedOnce = true;
					//print ("ready for second input");
				}

			}

			if (Input.GetAxisRaw (vertical) == 0) 
			{
				if (lastInput == Inputs.Up || lastInput == Inputs.Down) 
				{
					pressedOnce = true;
					//print ("ready for second input");
				}
			}
		}

		//Si le buffer est en cours et que la touche a été relâchée

		if (isBuffering && pressedOnce && singlePressed) 
		{
			//Si la même touche est pressée

			if (Input.GetAxisRaw (horizontal) == 1 && lastInput == Inputs.Right
			    || Input.GetAxisRaw (horizontal) == -1 && lastInput == Inputs.Left
			    || Input.GetAxisRaw (vertical) == 1 && lastInput == Inputs.Up
			    || Input.GetAxisRaw (vertical) == -1 && lastInput == Inputs.Down) 
			{
				//On lunge
				StopCoroutine (LungeBuffer ());
				StartCoroutine (Lunge ());
				isBuffering = false;
				pressedOnce = false;
				//print ("second input : LUNGE");
			} 

			//Si c'est une autre touche

			else 
			{
				//On save le nouvel input
				if (Input.GetAxisRaw (horizontal) == 1)
					lastInput = Inputs.Right;
				if (Input.GetAxisRaw (horizontal) == -1)
					lastInput = Inputs.Left;
				if (Input.GetAxisRaw (vertical) == 1)
					lastInput = Inputs.Up;
				if (Input.GetAxisRaw (vertical) == -1)
					lastInput = Inputs.Down;

				pressedOnce = false;

				//On arrête le buffer
				StopCoroutine(LungeBuffer());

				//On relance le buffer
				StartCoroutine(LungeBuffer());

				//print ("new input : " + lastInput);
			}
		}


	}

	IEnumerator LungeBuffer()
	{
		isBuffering = true;
		//print ("buffering : " + isBuffering);
		yield return new WaitForSeconds (bufferTime);
		isBuffering = false;
		//print ("buffering : " +  isBuffering);
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

	public void Die()
	{
		col.material = null;
		rb.mass = 1;
		anim.SetTrigger ("dies");
		Destroy (gameObject);
	}
}
