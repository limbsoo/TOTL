using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ButtonEvent;
using static PopupController;

public class MainSceneUIManager : UIEvent
{
    public static MainSceneUIManager instance { get; private set; }

    //public ScenePopups PopupSet;

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        //if (this.name == "MainScene")
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //}
    }


    //public Dictionary<PopupType, GameObject> dicPopups;

    //private ButtonClickEvent[] _buttons;


    private void Start()
    {
        base.Start();

        SoundManager.instance.StopAllSound();
        SoundManager.instance.Play("MainScene", SoundCatecory.BGM, true);






        ////두번이벤트확인
        //_buttons = GetComponentsInChildren<ButtonClickEvent>();

        //if (_buttons.Length != 0)
        //{
        //    foreach (var button in _buttons)
        //    {
        //        button.OnButtonClicked += (buttonType, name) => HandleButtonEvent(buttonType, name);
        //    }
        //}


        //dicPopups = new Dictionary<PopupType, GameObject>();
        //RectTransform rectTransform = GetComponent<RectTransform>();

        //for (int i = 0; i < PopupSet.popups.Count; i++)
        //{
        //    GameObject go = Instantiate(PopupSet.popups[i].popup);
        //    go.transform.SetParent(rectTransform, false);

        //    PopUp popUp = go.GetComponent<PopUp>();
        //    popUp.OnPopupEvent += HandlePopupEvent;

        //    go.SetActive(false);
        //    dicPopups.Add(PopupSet.popups[i].popupType, go);
        //}

    }

    //public void ActivatePopup(PopupType PopupType)
    //{
    //    dicPopups[PopupType].SetActive(true);
    //}


    //public void RegisterPopup(Popup popup)
    //{
    //    if (!_activePopups.Contains(popup))
    //    {
    //        _activePopups.Add(popup);
    //        popup.OnPopupEvent += HandlePopupEvent;
    //    }
    //}

    //public void UnregisterPopup(Popup popup)
    //{
    //    if (_activePopups.Contains(popup))
    //    {
    //        _activePopups.Remove(popup);
    //        popup.OnPopupEvent -= HandlePopupEvent;
    //    }
    //}


    //private void HandleButtonEvent(ButtonType buttonType, string name)
    //{
    //    ActivateEvent(buttonType, name);
    //    Debug.Log(string.Format("일반 버튼타입 {0}, name {1}", buttonType.ToString(), name));
    //}


    //private void HandlePopupEvent(PopUp popup, ButtonType buttonType, string name)
    //{
    //    SoundManager.instance.Play("Click", SoundCatecory.Effect, false);

    //    ActivateEvent(buttonType, name);

    //    Debug.Log(string.Format("팝업 버튼타입 {0}, name {1}", buttonType.ToString(), name));

    //    if (buttonType == ButtonType.Close)
    //    {
    //        popup.gameObject.SetActive(false);
    //    }
    //}



    //void ActivateEvent(ButtonType buttonType, string name)
    //{



    //    switch (buttonType)
    //    {
    //        case ButtonType.StartOrWarn:
    //            if (DataManager.Instance.HaveSaveData()) { ActivatePopup(PopupType.DataIsExist); }
    //            else { ActivatePopup(PopupType.SelectCharacter); }
    //            break;

    //        case ButtonType.ContinueOrWarn:

    //            if (DataManager.Instance.HaveSaveData()) 
    //            {
    //                SceneManager.instance.LoadScene("PlayScene");
    //            }
    //            else { ActivatePopup(PopupType.DataIsNotExist); }
    //            break;


    //        case ButtonType.Open:

    //            foreach(PopupType popupType in dicPopups.Keys) 
    //            {
    //                if(name == popupType.ToString())
    //                {
    //                    ActivatePopup(popupType); 
    //                    break;
    //                }
    //            }
    //            break;

    //        case ButtonType.LoadStage:
    //            SceneManager.instance.LoadScene("PlayScene");
    //            break;


    //    }

    //}























    //public int playerIdx;

    //public void StartGame()
    //{
    //    //test.text = "캐릭터 선택";

    //    DataManager.Instance.InitSelectCharacter(playerIdx);
    //    //나중에 게임모드도

    //    //test.text = "신 로드 시작";

    //    SceneManager.instance.LoadScene("PlayScene");
    //}

    //public void LoadGame()
    //{
    //    SceneManager.instance.LoadScene("PlayScene");
    //}

    //public void ResetData()
    //{
    //    DataManager.Instance.ResetStage();
    //    //gameObject.transform.parent.gameObject.SetActive(false);
    //}


    //private void OnEnable()
    //{
    //    ButtonController.OnLoadScene += LoadGame;
    //}

    //private void OnDisable()
    //{
    //    ButtonController.OnLoadScene -= LoadGame;

    //    //EventManager.instance.OnStageEnd -= EndStage;
    //}


    //private void Start()
    //{

    //}




    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
