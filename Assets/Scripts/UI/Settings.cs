using UnityEngine;
using UnityEngine.UI;



public class Settings : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider soundtrackVolume;
    [SerializeField] private Slider sfxVolume;

    private void Start()
    {
        Reset("masterVolume", masterVolume);
        Reset("soundtrackVolume", soundtrackVolume);
        Reset("sfxVolume", sfxVolume);
    }

    private void Reset(string key, Slider slider)
    {
        if (PlayerPrefs.HasKey(key))
            slider.value = PlayerPrefs.GetFloat(key);
        else
        {
            PlayerPrefs.SetFloat(key, 1f);
            PlayerPrefs.Save();
        }
    }

    public void UpdateMasterVolume()
    {
        PlayerPrefs.SetFloat("masterVolume", masterVolume.value);
        PlayerPrefs.Save();
    }

    public void UpdateSoundtrackVolume()
    {
        PlayerPrefs.SetFloat("soundtrackVolume", soundtrackVolume.value);
        PlayerPrefs.Save();
    }

    public void UpdateSFXVolume()
    {
        PlayerPrefs.SetFloat("sfxVolume", soundtrackVolume.value);
        PlayerPrefs.Save();
    }   
}
