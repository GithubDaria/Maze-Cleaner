using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffSoundButtonManager : MonoBehaviour
{
    private bool IsMuted = false;

    [SerializeField] private GameObject OnSwitch;
    [SerializeField] private GameObject OffSwitch;

    [SerializeField] private SoundManager sound;


    private void Start()
    {
        IsMuted = sound.ReturnMuteState();
        if (IsMuted)
        {
            OffSwitch.SetActive(true);
            OnSwitch.SetActive(false);
        }
        else
        {
            OffSwitch.SetActive(false);
            OnSwitch.SetActive(true);
        }
    }
    public void SwitchSoundState()
    {
        IsMuted = sound.ReturnMuteState();
        if (!IsMuted)
        {
            OffSwitch.SetActive(true);
            OnSwitch.SetActive(false);
            IsMuted = true;
        }
        else
        {
           


            OffSwitch.SetActive(false);
            OnSwitch.SetActive(true);
            IsMuted = false;
        }
    }
}
