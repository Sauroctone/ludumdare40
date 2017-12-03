using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicManager : MonoBehaviour {

	public List<PlayerClass> mimicList = new List<PlayerClass>();
	public List<Transform> furnitureList = new List<Transform>();

	public GameObject mimic;

	void Start()
	{
		mimicList.Add (new PlayerClass ("Horizontal_01", "Vertical_01"));
		mimicList.Add (new PlayerClass ("Horizontal_02", "Vertical_02"));
		mimicList.Add (new PlayerClass ("Horizontal_03", "Vertical_03"));
		mimicList.Add (new PlayerClass ("Horizontal_04", "Vertical_04"));
		mimicList.Add (new PlayerClass ("Horizontal_05", "Vertical_05"));
		mimicList.Add (new PlayerClass ("Horizontal_06", "Vertical_06"));
		mimicList.Add (new PlayerClass ("Horizontal_07", "Vertical_07"));
		mimicList.Add (new PlayerClass ("Horizontal_08", "Vertical_08"));
		mimicList.Add (new PlayerClass ("Horizontal_09", "Vertical_09"));
		mimicList.Add (new PlayerClass ("Horizontal_10", "Vertical_10"));
		mimicList.Add (new PlayerClass ("Horizontal_11", "Vertical_11"));

		foreach (GameObject furniture in GameObject.FindGameObjectsWithTag("Furniture")) 
		{
			furnitureList.Add (furniture.transform);
		}
	}

	void Update()
	{
		if (mimicList.Count > 0 && furnitureList.Count > 0) 
		{
			for (int i = 0; i < mimicList.Count; i++) 
			{
				if (Input.GetAxis (mimicList [i].horizontal) != 0 || Input.GetAxis (mimicList [i].vertical) != 0) 
				{
					print ("new mimic !");
					int parentIndex = Mathf.RoundToInt (Random.Range (0, furnitureList.Count));
					Transform parent = furnitureList [parentIndex];

					GameObject newPlayer = GameObject.Instantiate (mimic, parent);
					MimicController mimicScript = newPlayer.GetComponent<MimicController> ();
					mimicScript.horizontal = mimicList [i].horizontal;
					mimicScript.vertical = mimicList [i].vertical;

					mimicList.RemoveAt (i);
					furnitureList.RemoveAt (parentIndex);
				}
			}
		}
	}
}
