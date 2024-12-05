using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] SoundType soundType;
    [SerializeField] Slider _slider;

    //void Start()
    //{
    //    _slider = GetComponent<Slider>();
    //}

    public void SLIDER_ModifyVolume()
    {
        SoundManager.instance.SetVolume(soundType, _slider.value);
    }
}
