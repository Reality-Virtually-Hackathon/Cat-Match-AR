using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {


	public AudioMixerSnapshot DefaultMX; //default layer
	public AudioMixerSnapshot AffectionMX; //Affection layer (w/ piano)

	public float defaultToAffectionTime; //fade time from default to affection
	public float affectionToDefaultTime; //fade time from affection to default

	bool friendlyCat = false;
	// Use this for initialization
	void Awake () {
		
		ToDefaultLayer ();

	}
	
	// Update is called once per frame
	void Update () {

		//spacebar to toggle affection state
		if (Input.GetKeyDown ("space")) {
			if (!friendlyCat) {
				friendlyCat = true;
				ToAffectionLayer ();
			} else if (friendlyCat) {
				friendlyCat = false;
				ToDefaultLayer ();
			}
		}
	}


	//transitions to default layer
	void ToDefaultLayer(){
		DefaultMX.TransitionTo (affectionToDefaultTime);
	}
	//transitions to affection layer
	void ToAffectionLayer(){
		AffectionMX.TransitionTo (defaultToAffectionTime);
	}
}
