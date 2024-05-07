using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSetting : MonoBehaviour
{
    [SerializeField] private Sprite VolumeOn;
    [SerializeField] private Sprite VolumeOff;

    [SerializeField] private Image ImageComponent;

    private bool VolumeIsOn = true;

    public void ChangeVolumeState()
    {
        if(VolumeIsOn)
        {
            ImageComponent.sprite = VolumeOff;
            VolumeIsOn = false;
        }
        else
        {
            ImageComponent.sprite = VolumeOn;
            VolumeIsOn = true;
        }
    }
}
