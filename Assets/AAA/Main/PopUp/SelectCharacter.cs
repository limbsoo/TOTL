using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ButtonEvent;

public class SelectCharacter : PopUp
{
    public int _selectedIndex;
    public Image portrait;
    public PlayerList playerList;


    protected override void Start()
    {
        _selectedIndex = 0;

        if (playerList != null && playerList.lists.Count > 0)
        {
            portrait.sprite = playerList.lists[_selectedIndex].sprite;
        }


        _buttons = GetComponentsInChildren<ButtonClickEvent>();

        foreach (var button in _buttons)
        {
            button.OnButtonClicked += (buttonType, name) => HandleButtonClicked(buttonType, name);
        }
    }

    protected override void OnDestroy()
    {
        foreach (var button in _buttons)
        {
            button.OnButtonClicked -= (buttonType, name) => HandleButtonClicked(buttonType, name);
        }

        //base.OnDisable();
    }

    protected override void HandleButtonClicked(ButtonType buttonType, string name)
    {
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

            case ButtonType.LoadStage:
                DataManager.Instance.SelectPlayer(_selectedIndex);
                base.HandleButtonClicked(buttonType, name);
                break;

            default:
                Debug.LogWarning($"Unhandled button type in SelectCharacter: {buttonType}");
                break;
        }
    }


}
