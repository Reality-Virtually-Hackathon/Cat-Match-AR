using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pet : MonoBehaviour {

	public Slider happySlider;
	public float pets;
	public float petsPerClick = 1.0f;


	// Use this for initialization
	void Start () {
		
	}
	
//	// Update is called once per frame
//	void Update () {
//		happySlider = pets;
//		
//	}

	public void clicked (){
		pets += petsPerClick;

	}
}
