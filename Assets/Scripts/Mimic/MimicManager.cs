using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicManager : MonoBehaviour {

	public List<PlayerClass> availableList = new List<PlayerClass>();
	public List<GameObject> mimicList = new List<GameObject>();
	public List<Transform> furnitureList = new List<Transform>();

	public GameObject mimic;
	public float newMass;

	public AudioSource source;
	RoundManager manager;
	public PhysicMaterial slippery;

	void Start()
	{
		availableList.Add (new PlayerClass ("Horizontal_01", "Vertical_01"));
		availableList.Add (new PlayerClass ("Horizontal_02", "Vertical_02"));
		availableList.Add (new PlayerClass ("Horizontal_03", "Vertical_03"));
		availableList.Add (new PlayerClass ("Horizontal_04", "Vertical_04"));
		availableList.Add (new PlayerClass ("Horizontal_05", "Vertical_05"));
		availableList.Add (new PlayerClass ("Horizontal_06", "Vertical_06"));
		availableList.Add (new PlayerClass ("Horizontal_07", "Vertical_07"));
		availableList.Add (new PlayerClass ("Horizontal_08", "Vertical_08"));
		availableList.Add (new PlayerClass ("Horizontal_09", "Vertical_09"));
		availableList.Add (new PlayerClass ("Horizontal_10", "Vertical_10"));
		availableList.Add (new PlayerClass ("Horizontal_11", "Vertical_11"));

		foreach (GameObject furniture in GameObject.FindGameObjectsWithTag("Furniture")) 
		{
			furnitureList.Add (furniture.transform);
		}

		manager = GetComponent<RoundManager> ();
	}

	void Update()
	{
		if (manager.round != Rounds.End && availableList.Count > 0 && furnitureList.Count > 0) 
		{
			for (int i = 0; i < availableList.Count; i++) 
			{
				if (Input.GetAxis (availableList [i].horizontal) != 0 || Input.GetAxis (availableList [i].vertical) != 0) 
				{
					print ("new mimic !");
					int parentIndex = Mathf.RoundToInt (Random.Range (0, furnitureList.Count));
					Transform parent = furnitureList [parentIndex];
					parent.GetComponent<Collider> ().material = slippery;
					parent.GetComponent<Rigidbody> ().mass = newMass;

					GameObject newPlayer = GameObject.Instantiate (mimic, parent);
					mimicList.Add (newPlayer);
					MimicController mimicScript = newPlayer.GetComponent<MimicController> ();
					mimicScript.horizontal = availableList [i].horizontal;
					mimicScript.vertical = availableList [i].vertical;
					mimicScript.moveSpeed = availableList [i].moveSpeed;
					mimicScript.lungeSpeed = availableList [i].lungeSpeed;
					//mimicScript.source = source;

					availableList.RemoveAt (i);
					furnitureList.RemoveAt (parentIndex);
				}
			}
		}
	}
}
