using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> soundtracks;
    private AudioSource soundtrackSource;
    private int currentSoundtrack = -1;

    private List<AudioSource> soundPool;

    private const int poolSize = 15;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        soundtrackSource = GetComponent<AudioSource>();
        PlaySoundtrack();
        InitSoundPool();
    }

    private void InitSoundPool()
    {
        soundPool = new List<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = new GameObject();
            AudioSource source = go.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            soundPool.Add(source);
        }
    }

    private void UpdateSFXVolume()
    {
        foreach (AudioSource a in soundPool)
            a.volume = PlayerPrefs.GetFloat("masterVolume")*PlayerPrefs.GetFloat("sfxVolume");
    }

    public void PlaySFX(AudioClip sfx)
    {
        currentIndex = GetFreeAudioSource();
        soundPool[currentIndex].clip = sfx;
        currentIndex++;
    }

    private int GetFreeAudioSource()
    {
        if (currentIndex >= poolSize)
            currentIndex = 0;
        return currentIndex;
    }


    // Update is called once per frame
    void Update()
    {
        soundtrackSource.volume = PlayerPrefs.GetFloat("masterVolume")*PlayerPrefs.GetFloat("soundtrackVolume");
        UpdateSFXVolume();
    }

    public void PlaySoundtrack()
    {
        if (soundtrackSource.isPlaying)
            return;
        currentSoundtrack++;
        if (currentSoundtrack == soundtracks.Count)
            currentSoundtrack = 0;
        soundtrackSource.clip = soundtracks[currentSoundtrack];
        soundtrackSource.Play();
    }
}
