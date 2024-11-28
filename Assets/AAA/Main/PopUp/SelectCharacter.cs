using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ButtonEvent;

public class SelectCharacter : PopUp
{
    int _selectedIndex;
    public Image portrait;
    public PlayerList playerList;


    void Start()
    {
        _selectedIndex = 0;

        if (playerList != null && playerList.lists.Count > 0)
        {
            portrait.sprite = playerList.lists[_selectedIndex].sprite;
        }
    }


    void OnEnable()
    {
        base.OnEnable();

        foreach (var button in _buttons)
        {
            button.OnButtonClicked += (buttonType, name) => HandleButtonClicked(buttonType, name);
        }
    }

    void OnDisable()
    {
        foreach (var button in _buttons)
        {
            button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
        }

        base.OnDisable();
    }

    protected override void HandleButtonClicked(ButtonType buttonType, string name)
    {
        base.HandleButtonClicked(buttonType, name);

        switch (buttonType)
        {
            case ButtonType.Left:
                _selectedIndex--;
                if (_selectedIndex < 0) { _selectedIndex = playerList.lists.Count - 1; }
                portrait.sprite = playerList.lists[_selectedIndex].sprite;
                break;

            case ButtonType.Right:
                _selectedIndex++;
                if (_selectedIndex > playerList.lists.Count - 1) { _selectedIndex = 0; }
                portrait.sprite = playerList.lists[_selectedIndex].sprite;
                break;

            //case ButtonType.LoadScene:
            //    base.OnPopupEvent?.Invoke(this, buttonType, name);
            //    break;

            default:
                Debug.LogWarning($"Unhandled button type in SelectCharacter: {buttonType}");
                break;
        }
    }
}
