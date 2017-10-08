using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class KittyHappyManager : MonoBehaviour {
	public float CurrentHappy { get; set; }
	public float MaxHappy { get; set; }

	public Slider happybar;



	// Use this for initialization
	void Start () {
		MaxHappy = 100f;
		CurrentHappy = 0f;

		happybar.value = CalculateHealth();
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown(buttonName:"Pet"))
			DealHappy(1);
	}

	void DealHappy(float happyValue)
	{
		CurrentHappy += happyValue;

		if (CurrentHappy >= 100)
			Win();

	}

	float CalculateHealth()
	{
		return CurrentHappy * MaxHappy;
	}

	void Win()
	{
		CurrentHappy = 100;
		Debug.Log ("You Won!!");
	}
}