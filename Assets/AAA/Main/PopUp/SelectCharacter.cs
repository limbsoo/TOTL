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

    public Button selectButton;

    void UpdateCharacter()
    {
        portrait.sprite = playerList.lists[_selectedIndex].sprite;
        skillportrait.sprite = playerList.lists[_selectedIndex].SkillSprite;
        texts[0].text = playerList.lists[_selectedIndex].sprite.name;
        texts[1].text = playerList.lists[_selectedIndex].Stats.health.ToString();
        texts[2].text = playerList.lists[_selectedIndex].Stats.moveSpeed.ToString();

        switch (playerList.lists[_selectedIndex].Stats.playerSkillKind)
        {
            case PlayerSkillKinds.Teleport:
                texts[3].text = string.Format("�÷��̾ �ٶ󺸴� �������� {0} ��ŭ �̵��մϴ�.", playerList.lists[_selectedIndex].Stats.effectValue.ToString());
                break;

            case PlayerSkillKinds.Decoy:
                texts[3].text = string.Format("�÷��̾��� ���� ��ġ�� {0}�� �� �÷��̾� ��� �̸��� �����ִ� �н��� �����մϴ�.", playerList.lists[_selectedIndex].Stats.effectValue.ToString());
                break;

            case PlayerSkillKinds.Hide:
                texts[3].text = string.Format("���� �÷��̾ �����ϴ� ���鿡�� {0}�� �� ������ �ʰ� �մϴ�. ", playerList.lists[_selectedIndex].Stats.effectValue.ToString());
                break;
        }






        //texts[3].text = playerList.lists[_selectedIndex].Stats.moveSpeed.ToString();
        texts[4].text = playerList.lists[_selectedIndex].Stats.coolDown.ToString();

        selectButton.interactable = true;

        string unlockcond = "�ر� ���� : ����";

        switch (playerList.lists[_selectedIndex].UnlockCondition)
        {
            case CharacterUnlock.Clear10Wave:

                unlockcond = "�ر� ���� : 10 wave �̻� Ŭ����";

                if (DataManager.Instance.saveData.maxWave < 10) { selectButton.interactable = false; }
                    break;

            case CharacterUnlock.Clear20Wave:

                unlockcond = "�ر� ���� : 20 wave �̻� Ŭ����";

                if (DataManager.Instance.saveData.maxWave < 20) { selectButton.interactable = false; }
                break;
        }

        texts[5].text = unlockcond;

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

            case ButtonType.Close:
                gameObject.SetActive(false);
                break;

            default:
                Debug.LogWarning($"Unhandled button type in SelectCharacter: {buttonType}");
                break;
        }
    }


}
