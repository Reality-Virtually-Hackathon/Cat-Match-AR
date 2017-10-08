using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance;

	//UI sounds
	public AudioClip[] positiveUI;
	public AudioClip[] neutralUI;
	public AudioClip[] negativeUI;

	//cat sounds
	public AudioClip[] happyCat;
	public AudioClip[] hungryCat;
	public AudioClip[] angryCat;

	AudioSource source;

	int previousRand = -1;
    void Awake()
    {
        instance = this;
    }
	void Start () {
		source = GetComponent<AudioSource> ();
	}
	

	void Update () {
		//for testing cat sounds
		if (Input.GetKeyDown ("q")) { //press q for random happy cat sound
			happyCatSound ();
		}

		if (Input.GetKeyDown ("w")) { //press w for random hungry cat sound
			hungryCatSound ();
		}

		if (Input.GetKeyDown ("e")) { //press e for random angry cat sound
			angryCatSound ();
		}
	}

public	void happyCatSound(){
		int randInt = Random.Range (0, happyCat.Length);
		if (randInt == previousRand) { //ensures same sound never plays twice in a row
			while (randInt == previousRand) {
				randInt = Random.Range (0, happyCat.Length);
			}
		}
		source.clip = happyCat [randInt];
		source.Play ();
		previousRand = randInt;
	}

public	void hungryCatSound(){
		int randInt = Random.Range (0, hungryCat.Length);
		if (randInt == previousRand) { 
			while (randInt == previousRand) {
				randInt = Random.Range (0, hungryCat.Length);
			}
		}
		source.clip = hungryCat [randInt];
		source.Play ();
		previousRand = randInt;
	}

public	void angryCatSound(){
		int randInt = Random.Range (0, angryCat.Length);
		if (randInt == previousRand) { 
			while (randInt == previousRand) {
				randInt = Random.Range (0, angryCat.Length);
			}
		}
		source.clip = angryCat [randInt];
		source.Play ();
		previousRand = randInt;
	}

}
