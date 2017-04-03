using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SfxManager : MonoBehaviour {

    [SerializeField]
    bool doDebug;

    [SerializeField]
    string sourceFolderPath;

    public bool Mute;


    static SfxManager instance;
    private static AudioSource player;
    private static AudioSource musicPlayer;
    private static Dictionary<string, AudioClip> sfxs;

	// Use this for initialization
	void Start () {

        sfxs = new Dictionary<string, AudioClip>();

        if (!instance)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("Only one SfxManager should be present in the same scene");
        }

        player = GetComponent<AudioSource>();
        
        if (!player)
        {
            player = gameObject.AddComponent<AudioSource>();
            
        }
        
        LoadSfxs();        
	}


    private void LoadSfxs()
    {

        var loadedSfxs = Resources.LoadAll(sourceFolderPath, typeof(AudioClip));

        foreach (AudioClip sfx in loadedSfxs)
        {
            sfxs.Add(sfx.name, sfx);
        }

        int loadedSfx = sfxs.Count;

        if (doDebug)
        {
            Debug.Log(String.Format("Loaded {0} audio files. ", loadedSfx));
        }
    }

    public static void PlaySfx(string sfx,  float delay = 0f, float volume = 1f)
    {
        if (!instance.Mute)
        {
            player.volume = volume;
            player.clip = sfxs[sfx];
            player.PlayDelayed(delay);
        }
    }

    public static void PlaySfx(string sfx, GameObject location , float delay = 0f, float volume = 1f)
    {
        if (!instance.Mute)
        {
            var locationPlayer = location.GetComponent<AudioSource>();
            
            if (locationPlayer == null)
            {//If GameObject does not have a AudioSource we will give it one.
                locationPlayer = location.AddComponent<AudioSource>();
                locationPlayer.playOnAwake = false;
            }
            locationPlayer.volume = volume;
            locationPlayer.clip = sfxs[sfx];
            locationPlayer.PlayDelayed(delay);
        }
    }
}
