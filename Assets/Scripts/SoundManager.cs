using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public AudioSource sfxSource;
	public AudioSource musicSource;

	public void PlayClip(AudioClip clip) {
		sfxSource.clip = clip;
		sfxSource.Play ();
	}
}
