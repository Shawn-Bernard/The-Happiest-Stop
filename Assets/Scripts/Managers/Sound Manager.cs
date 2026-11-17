using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioMixer masterMixer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void SetMixerVolume(string soundString, float volume) // Converts volume into decibles
    {
        float decibels;

        // If the slider is above 0, convert the slider number to decibels
        if (volume > 0.0001f)
        {
            decibels = Mathf.Log10(volume) * 20f;
        }
        // Otherwise (if the slider is at 0), mute the sound completely
        else
        {
            decibels = -80f;
        }

        masterMixer.SetFloat(soundString, decibels);
    }

    public void SetMasterVolume(float volume)
    {
        SetMixerVolume("Master", volume);
    }
    public void SetMusicVolume(float volume)
    {
        SetMixerVolume("Music", volume);
    }
    public void SetSoundEffectsVolume(float volume)
    {
        SetMixerVolume("Sound Effects", volume);
    }

}
