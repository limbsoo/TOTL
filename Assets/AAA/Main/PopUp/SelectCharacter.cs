using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ButtonEvent;

public class SelectCharacter : PopUp
{
    public int _selectedIndex;
    public Image portrait;

    public Image skillportrait;

    public PlayerList playerList;


    public TMP_Text[] texts;

    void UpdateCharacter()
    {
        portrait.sprite = playerList.lists[_selectedIndex].sprite;
        skillportrait.sprite = playerList.lists[_selectedIndex].SkillSprite;
        texts[0].text = playerList.lists[_selectedIndex].sprite.name;
        texts[1].text = playerList.lists[_selectedIndex].Stats.health.ToString();
        texts[2].text = playerList.lists[_selectedIndex].Stats.moveSpeed.ToString();
        texts[3].text = playerList.lists[_selectedIndex].Stats.moveSpeed.ToString();
        texts[4].text = playerList.lists[_selectedIndex].Stats.coolDown.ToString();
    }


    protected override void Start()
    {
        GameObject go = transform.Find("Texts").gameObject;

        //texts.Free();
        texts = go.GetComponentsInChildren<TMP_Text>();

        _selectedIndex = 0;

        if (playerList != null && playerList.lists.Count > 0)
        {
            //portrait.sprite = playerList.lists[_selectedIndex].sprite;

            UpdateCharacter();
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
                //portrait.sprite = playerList.lists[_selectedIndex].sprite;

                UpdateCharacter();
                break;

            case ButtonType.Right:
                _selectedIndex++;
                if (_selectedIndex > playerList.lists.Count - 1) { _selectedIndex = 0; }
                //portrait.sprite = playerList.lists[_selectedIndex].sprite;

                UpdateCharacter();
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
