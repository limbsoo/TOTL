using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;


public class PopUp : MonoBehaviour
{
    public PopupType popupType;

    public event Action<PopUp, ButtonType, string> OnPopupEvent; // 버튼 정보 포함

    public ButtonClickEvent[] _buttons;

    protected void OnEnable()
    {
        _buttons = GetComponentsInChildren<ButtonClickEvent>();

        if(_buttons.Length != 0)
        {
            foreach (var button in _buttons)
            {
                button.OnButtonClicked += (buttonType, name) => HandleButtonClicked(buttonType, name);
            }
        }
    }

    protected void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
        }
    }

    protected virtual void HandleButtonClicked(ButtonType buttonType, string name)
    {
        OnPopupEvent?.Invoke(this, buttonType, name);
    }

}