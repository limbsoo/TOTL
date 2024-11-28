using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonClickEvent : MonoBehaviour
{
    public event Action<ButtonType, string> OnButtonClicked;
    public ButtonType buttonType;
    protected Button _button;

    private void OnEnable()
    {
        _button = GetComponent<Button>();

        if (_button != null)
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke(buttonType, gameObject.name));
        }
    }

    private void OnDisable()
    {
        if (_button != null)
        {
            _button.onClick.RemoveAllListeners();
        }
    }
}
