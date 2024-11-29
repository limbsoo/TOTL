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

    protected ButtonClickEvent[] _buttons;

    void Awake()
    {
        _buttons = GetComponentsInChildren<ButtonClickEvent>();

    }

    protected virtual void Start()
    {

        if (_buttons.Length != 0)
        {
            foreach (var button in _buttons)
            {
                button.OnButtonClicked += (buttonType, name) => HandleButtonClicked(buttonType, name);
            }
        }
    }

    protected virtual void OnDestroy()
    {
        foreach (var button in _buttons)
        {
            button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
        }
    }

    //protected virtual void OnEnable()
    //{

    //    if(_buttons.Length != 0)
    //    {
    //        foreach (var button in _buttons)
    //        {
    //            button.OnButtonClicked += (buttonType, name) => HandleButtonClicked(buttonType, name);
    //        }
    //    }
    //}

    //protected virtual void OnDisable()
    //{
    //    foreach (var button in _buttons)
    //    {
    //        button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
    //    }
    //}

    protected virtual void HandleButtonClicked(ButtonType buttonType, string name)
    {
        OnPopupEvent?.Invoke(this, buttonType, name);
    }

}