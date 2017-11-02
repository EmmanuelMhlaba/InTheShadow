using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
	public static AudioManager Instance { get; private set; }
	public Sound[] sounds;

	void Awake () {
		if (Instance == null) {
			Instance = this;
			AttachAudioSources ();
		} else {
			Destroy (gameObject);
		}
	}

	private void AttachAudioSources () {
		foreach (Sound s in sounds) {
			s.source = gameObject.AddComponent<AudioSource> ();
			s.source.clip = s.clip;
			s.source.volume = s.volume;
			s.source.pitch = s.pitch;
			s.source.loop = s.loop;
			if (s.playOnWake == true) {
				s.source.Play ();
			}
		}
	}

	private Sound FindSound (string name) {
		Sound s = Array.Find (sounds, sound => sound.name == name);
		if (s == null) {
			Debug.LogWarning ("Sound: " + name + " not found.");
		}
		return s;
	}

	public void PlaySound (string name) {
		Sound s = FindSound (name);
		if (s != null) {
			s.source.Play ();
		}
	}

	public void StopSound (string name) {
		Sound s = FindSound (name);
		if (s != null) {
			s.source.Stop ();
		}
	}
}

[Serializable]
public class Sound {
	public string name;
	public AudioClip clip;
	[HideInInspector] public AudioSource source;
	[Range (0f, 1f)] public float volume;
	[Range (0.1f, 3f)] public float pitch;
	public bool playOnWake;
	public bool loop;
}
