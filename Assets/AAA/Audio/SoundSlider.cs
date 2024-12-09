using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] SoundCatecory soundCatecory;
    [SerializeField] Slider _slider;


    //private void Awake()
    //{
    //    _slider = GetComponent<Slider>();
    //}

    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void SLIDER_ModifyVolume()
    {
        SoundManager.instance.SetVolume(soundCatecory, _slider.value);
    }
}
