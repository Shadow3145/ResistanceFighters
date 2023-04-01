using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private AudioClip sound;
    [SerializeField] [Range(0, 1)] private float volume = 1f;

    public AudioClip GetSound()
    {
        return sound;
    }

    public float GetVolume()
    {
        return volume;
    }
}
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private List<AudioClip> soundtracks;
    private AudioSource soundtrackSource;
    private int currentSoundtrack = -1;

    [SerializeField] private List<Sound> sfx;
    private List<AudioSource> soundPool;
    [SerializeField] private List<AudioSource> ambients;

    private const int poolSize = 15;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
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
        foreach (AudioSource a in ambients)
            a.volume = PlayerPrefs.GetFloat("masterVolume") * PlayerPrefs.GetFloat("sfxVolume");
    }

    public void PlaySFX(int i)
    {
        currentIndex = GetFreeAudioSource();
        Sound sound = sfx[i];
        soundPool[currentIndex].clip = sound.GetSound();
        soundPool[currentIndex].volume = sound.GetVolume()* PlayerPrefs.GetFloat("masterVolume") * PlayerPrefs.GetFloat("sfxVolume");
        soundPool[currentIndex].Play();
        currentIndex++;
    }

    private int GetFreeAudioSource()
    {
        if (currentIndex >= poolSize)
            currentIndex = 0;
        return currentIndex;
    }

    void Update()
    {
        soundtrackSource.volume = PlayerPrefs.GetFloat("masterVolume")*PlayerPrefs.GetFloat("soundtrackVolume");
        PlaySoundtrack();
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
