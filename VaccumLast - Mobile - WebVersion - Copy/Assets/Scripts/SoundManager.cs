using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public bool IsMuted = false;
    private const string MuteKey = "IsMuted";
    [SerializeField] private Sprite VolumeOn;
    [SerializeField] private Sprite VolumeOff;

    [SerializeField] private Image ImageComponent;

    [SerializeField] private AudioSource ButtonSound;
    [SerializeField] private AudioSource WinSound;
    [SerializeField] private AudioSource LoseSound;
    [SerializeField] private AudioSource LevelDoneSound;


    public bool ReturnMuteState()
    {
        return IsMuted;
    }
    public void ChangeVolumeState()
    {
        if (IsMuted)
        {
            AudioListener.volume =1;
            IsMuted = false;
            SaveSoundState();
            if(ImageComponent != null)
            {
                Debug.Log("souuuund");

                ImageComponent.sprite = VolumeOff;
            }
        }
        else
        {
            AudioListener.volume = 0;
            IsMuted = true;
            SaveSoundState();
            if (ImageComponent != null)
            {
                Debug.Log("souuuund");

                ImageComponent.sprite = VolumeOn;

            }

        }
    }
    public void SetVolumeStat()
    {

        if (IsMuted)
        {
            AudioListener.volume = 0;
            if (ImageComponent != null)
            {
                ImageComponent.sprite = VolumeOn;

            }
        }
        else
        {

            AudioListener.volume = 1f;

            if (ImageComponent != null)
            {
                ImageComponent.sprite = VolumeOff;
            }
        }
    }

    private void Start()
    {
        Debug.Log("start");
        LoadSoundState();
        SetVolumeStat();
    }
    void SaveSoundState()
    {
        PlayerPrefs.SetInt(MuteKey, IsMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadSoundState()
    {
        IsMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
        Debug.Log("start" + PlayerPrefs.GetInt(MuteKey));

    }
    public void PlayButtonSound()
    {
        if (!ButtonSound.isPlaying)
        {
            ButtonSound.Play();
        }
    }

    public void PlayGameOverSound()
    {
        LoseSound.Play();
    }
    public void PlayWinSound()
    {
        WinSound.Play();

    }
    public void PlayLevelDoneSound()
    {
        LevelDoneSound.Play();
    }
}
