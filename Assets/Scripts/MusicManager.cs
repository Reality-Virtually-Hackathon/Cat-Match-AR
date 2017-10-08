using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour {

    public static MusicManager instance;
	public AudioMixerSnapshot DefaultMX; //default layer
	public AudioMixerSnapshot AffectionMX; //Affection layer (w/ piano)

	public float defaultToAffectionTime; //fade time from default to affection
	public float affectionToDefaultTime; //fade time from affection to default

	bool friendlyCat = false;
	// Use this for initialization
	void Awake () {
		
		ToDefaultLayer ();
        instance = this;

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
public	void ToDefaultLayer(){
		DefaultMX.TransitionTo (affectionToDefaultTime);
	}
	//transitions to affection layer
public	void ToAffectionLayer(){
		AffectionMX.TransitionTo (defaultToAffectionTime);
	}
}
