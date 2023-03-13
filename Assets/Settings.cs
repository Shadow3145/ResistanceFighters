using UnityEngine;
using UnityEngine.UI;



public class Settings : MonoBehaviour
{
    [SerializeField] private Slider masterVolume;
    [SerializeField] private Slider soundtrackVolume;
    [SerializeField] private Slider sfxVolume;
    
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
