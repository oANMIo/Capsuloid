using UnityEngine;
using UnityEngine.UI;


public class AudioManager : MonoBehaviour
{
    private static readonly string FirstPLay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private static readonly string SoundEffectsPref = "SoundEffectsPref";
    private int firstPlayInt;
    public Slider musicSlider, soundEffectsSlider;
    private float musicFloat, soundEffectsFloat;
    public AudioSource musicAudio;
    public AudioSource[] soundEffectsAudio;

    void Start()
    {
        firstPlayInt = PlayerPrefs.GetInt(FirstPLay);

        if (firstPlayInt == 0)
        {
            musicFloat = .25f;
            soundEffectsFloat = .75f;
            musicSlider.value = musicFloat;
            soundEffectsSlider.value = soundEffectsFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsFloat);
            PlayerPrefs.SetInt(FirstPLay, -1);
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
            soundEffectsFloat = PlayerPrefs.GetFloat(SoundEffectsPref);
            soundEffectsSlider.value = soundEffectsFloat;
        }
    }
    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
        PlayerPrefs.SetFloat(SoundEffectsPref, soundEffectsSlider.value);
    }

    void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
        {
            SaveSoundSettings();
        }
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;

        for (int i = 0; i < soundEffectsAudio.Length; i++)
        {
            soundEffectsAudio[i].volume = soundEffectsSlider.value;
        }
    }



}
