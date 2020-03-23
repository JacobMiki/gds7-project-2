using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Image _sound;
    [SerializeField] private Image _music;

    [SerializeField] private Sprite _musicOn;
    [SerializeField] private Sprite _musicOff;

    [SerializeField] private Sprite _soundOn;
    [SerializeField] private Sprite _soundOff;

    [SerializeField] private AudioMixer _mixer;

    // Start is called before the first frame update
    void Start()
    {
        ToggleSound();
        ToggleSound();

        ToggleMusic();
        ToggleMusic();

    }

    public void ToggleSound()
    {
        if (PlayerPrefs.HasKey("mute_sound"))
        {
            PlayerPrefs.DeleteKey("mute_sound");
            _sound.sprite = _soundOn;
            _mixer.SetFloat("sfxVol", 0f);
        }
        else
        {
            PlayerPrefs.SetInt("mute_sound", 1);
            _mixer.SetFloat("sfxVol", -80f);
            _sound.sprite = _soundOff;
        }

        PlayerPrefs.Save();
    }

    public void ToggleMusic()
    {
        if (PlayerPrefs.HasKey("mute_music"))
        {
            PlayerPrefs.DeleteKey("mute_music");
            _music.sprite = _musicOn;
            _mixer.SetFloat("musicVol", 0f);

        }
        else
        {
            PlayerPrefs.SetInt("mute_music", 1);
            _music.sprite = _musicOff;
            _mixer.SetFloat("musicVol", -80f);
        }

        PlayerPrefs.Save();
    }
}
