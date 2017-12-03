﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RoundManager : MonoBehaviour {

	public Rounds round;
	public Winner winner;

	public float startupTime;
	//public float humanTime;
	public float mimicTime;
	public float textTime;

	public string closeYourEyes;
	public string until;
	public string mimicReveal;
	public string humanInitiate;
	public string humanRound;
	public string mimicRound;
	string winText;
	public string humanWins;
	public string mimicsWin;
	public string nextHuman;
	public string dedButton;

	public Text instructions;
	public Text restartText;
	public Button humanButton;

	void Start () 
	{
		round = Rounds.Startup;
		StartCoroutine (Startup ());
		humanButton.interactable = false;
	}
	
	IEnumerator Startup()
	{
		instructions.text = closeYourEyes + System.Environment.NewLine + until;
		yield return new WaitForSeconds (4);
		instructions.text = mimicReveal;
		yield return new WaitForSeconds (startupTime - 3);
		StartCoroutine (Countdown ());
		yield return new WaitForSeconds (3);
		instructions.text = humanInitiate;
		round = Rounds.Human;
		humanButton.interactable = true;
		//StartCoroutine (HumanRound ());
		yield return new WaitForSeconds (textTime);
		instructions.text = "";
	}

	public void OnHumanClick()
	{
		StartCoroutine (HumanToMimicRound ());
	}

	IEnumerator HumanToMimicRound()
	{
		//yield return new WaitForSeconds (humanTime - 3);
		//StartCoroutine (Countdown ());
		//yield return new WaitForSeconds (3);
	//	instructions.text = closeYourEyes;
		humanButton.interactable = false;
		yield return new WaitForSeconds (1);
		instructions.text = mimicRound;
		round = Rounds.Mimics;
		StartCoroutine (MimicRound ());
		yield return new WaitForSeconds (textTime);
		instructions.text = "";
	}

	IEnumerator MimicRound()
	{
		yield return new WaitForSeconds (mimicTime - 3);
		StartCoroutine (Countdown ());
		yield return new WaitForSeconds (3);
		instructions.text = humanRound;
		round = Rounds.Human;
		humanButton.interactable = true;
		//StartCoroutine (HumanRound ());
		yield return new WaitForSeconds (textTime);
		instructions.text = "";
	}

	IEnumerator Countdown()
	{
		instructions.text = "3";
		yield return new WaitForSeconds (1);
		instructions.text = "2";
		yield return new WaitForSeconds (1);
		instructions.text = "1";
	}

	void Update()
	{
		if (round == Rounds.End) 
		{
			StopAllCoroutines ();
			humanButton.interactable = false;
			restartText.gameObject.SetActive (true);

			if (winner == Winner.Human) 
			{
				instructions.text = humanWins;
			}

			if (winner == Winner.Mimics) 
			{
				humanButton.GetComponentInChildren<Text> ().text = dedButton;
				instructions.text = mimicsWin + System.Environment.NewLine + nextHuman;
			}

			if (Input.GetKeyUp (KeyCode.Space)) 
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}
}