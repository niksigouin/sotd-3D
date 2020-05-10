using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Audio")]
    public AudioMixer mainMixer;

    #region Public Methods

    public void SetVolume(float volume)
    {
        mainMixer.SetFloat("MasterVolume", volume);
    }

    #endregion
}
