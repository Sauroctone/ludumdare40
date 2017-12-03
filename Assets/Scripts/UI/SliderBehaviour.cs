using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {

	public HumanController human;
	Slider slider;
	float timeSinceEnabled;

	void Start()
	{
		slider = GetComponent<Slider> ();
	}

	void OnEnable()
	{
		timeSinceEnabled = 0;
	}

	void Update()
	{
		timeSinceEnabled += Time.deltaTime;
		slider.value =  timeSinceEnabled / human.reloadTime;
	}

}
