using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUI : MonoBehaviour {

	public HumanController human;
	float maxAmmo;
	public GameObject[] arrows;

	public IEnumerator UpdateArrows()
	{
		for (int i = 0; i < arrows.Length; i++) 
		{
			if (human.ammo > i) 
			{
				if (!arrows [i].activeSelf)
					arrows [i].SetActive (true);
			} 

			else 
			{
				if (arrows [i].activeSelf)
					arrows [i].SetActive (false);
			}
		}

		yield break;
	}
}
