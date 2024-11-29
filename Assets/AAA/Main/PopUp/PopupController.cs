using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class PopupController : MonoBehaviour
{
    public enum PopUpStyle { Maintain, Disappear}
    public enum PopUpEffect { None, Stop }

    public PopUpStyle popUpStyle;
    public PopUpEffect popUpEffect;

    void OnEnable()
    {
        switch (popUpStyle)
        {
            case PopUpStyle.Maintain:
                break;
            case PopUpStyle.Disappear:

                StartCoroutine(Function.instance.CountDown(2f, () =>
                {
                    gameObject.SetActive(false);
                }));

                break;
        }

        if(popUpEffect == PopUpEffect.Stop)
        {
            Time.timeScale = 0f;
        }
    }

    private void OnDisable()
    {
        if (popUpEffect == PopUpEffect.Stop)
        {
            Time.timeScale = 1f;
        }
    }

}
