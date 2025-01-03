using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PopupController;

public class UIEvent : MonoBehaviour
{
    public ScenePopups PopupSet;
    protected Dictionary<PopupType, GameObject> _popups;
    private ButtonClickEvent[] _buttons;



    protected virtual void Start()
    {
        //TextManager.instance.InitText();
        RegisterButton();
    }

    protected virtual void RegisterButton()
    {
        _buttons = GetComponentsInChildren<ButtonClickEvent>();

        if (_buttons.Length != 0)
        {
            foreach (var button in _buttons)
            {
                button.OnButtonClicked += (buttonType, name) => HandleButtonEvent(buttonType, name);
            }
        }

        _popups = new Dictionary<PopupType, GameObject>();

        RectTransform rectTransform = GetComponent<RectTransform>();

        for (int i = 0; i < PopupSet.popups.Count; i++)
        {
            GameObject go = Instantiate(PopupSet.popups[i].popup);
            go.transform.SetParent(rectTransform, false);

            PopUp popUp = go.GetComponent<PopUp>();
            popUp.OnPopupEvent += HandlePopupEvent;

            go.SetActive(false);
            _popups.Add(PopupSet.popups[i].popupType, go);
        }

    }


    protected virtual void HandleButtonEvent(ButtonType buttonType, string name)
    {
        SoundManager.instance.Play("Click", SoundCatecory.Effect, false);

        ActivateEvent(buttonType, name);
        Debug.Log(string.Format("일반 버튼타입 {0}, name {1}", buttonType.ToString(), name));
    }

    protected virtual void HandlePopupEvent(PopUp popup, ButtonType buttonType, string name)
    {
        SoundManager.instance.Play("Click", SoundCatecory.Effect, false);

        ActivateEvent(buttonType, name);

        Debug.Log(string.Format("팝업 버튼타입 {0}, name {1}", buttonType.ToString(), name));

        if (buttonType == ButtonType.Close)
        {
            popup.gameObject.SetActive(false);
        }
    }


    protected virtual void ActivatePopup(PopupType PopupType)
    {
        _popups[PopupType].SetActive(true);
    }

    protected virtual void ActivateEvent(ButtonType buttonType, string name)
    {
        switch (buttonType)
        {
            case ButtonType.StartOrWarn:
                if (DataManager.Instance.HaveProgressData()) { ActivatePopup(PopupType.DataIsExist); }
                else { ActivatePopup(PopupType.SelectCharacter); }
                break;

            case ButtonType.ContinueOrWarn:

                if (DataManager.Instance.HaveProgressData())
                {
                    SceneManager.instance.LoadScene("PlayScene");
                }
                else { ActivatePopup(PopupType.DataIsNotExist); }
                break;


            case ButtonType.Open:

                foreach (PopupType popupType in _popups.Keys)
                {
                    if (name == popupType.ToString())
                    {
                        ActivatePopup(popupType);
                        break;
                    }
                }
                break;

            case ButtonType.LoadStage:
                SceneManager.instance.LoadScene("PlayScene");
                break;

            case ButtonType.LoadMain:
                SceneManager.instance.LoadScene("MainScene");
                StageManager.Sstate = StageState.Edit;
                break;

            case ButtonType.ResetData:
                DataManager.Instance.ResetProgressData();

                _popups[PopupType.DataIsExist].SetActive(false);
                ActivatePopup(PopupType.SelectCharacter);
                break;

            case ButtonType.Quit:
                Application.Quit();
                break;

            case ButtonType.UseSkill:
                StageManager.instance.OnClickSkillButton.Invoke();


                //Application.Quit();
                break;

        }

    }












}
