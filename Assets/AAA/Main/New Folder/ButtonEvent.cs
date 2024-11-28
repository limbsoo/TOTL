using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ButtonEvent : MonoBehaviour
{

    private Button btn;

    public enum ButtonType
    {
        ShowPopUP,
        Close,
        CheckSaveAndShowPopUP,
        EraseSaveData
    }


    public ButtonType buttonType;
    public List<PopupType> popupTypes;



    public void OnEnable()
    {
        btn = GetComponent<Button>();
    }











    //public CondtionType conditionType;






    //public List<PopupType> popupTypes;


    //public ButtonType buttonType;







    //public void SetListener(Action<ButtonEventData> callback)
    //{
    //    if (btn != null)
    //    {
    //        btn.onClick.AddListener(() => callback(new ButtonEventData(buttonType, popupTypes)));
    //    }
    //}

    // 클릭 이벤트를 설정하는 메서드
    //public void SetListener(Action<ButtonType> callback)
    //{
    //    if (btn != null)
    //    {
    //        btn.onClick.AddListener(() => callback(buttonType));
    //    }
    //}



    //public Action ClickEvent;

    //void Start()
    //{
    //    btn = GetComponent<Button>();
    //    btn.onClick.AddListener(ClickEvent);
    //}

    //public void SetListner(Action ab)
    //{
    //    btn = GetComponent<Button>();

    //    if (listener != null) button.onClick.AddListener(() =>
    //    {
    //        ab();
    //    });

    //    btn.onClick.AddListener(ab);
    //}



    //public void ButtonAction()
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
















    //public class ButtonInfo
    //{
    //    public enum ButtonType
    //    {
    //        ShowPopUP,
    //        //CheckAndShowPopUP,
    //        //Confirm,
    //        Close
    //    }

    //    public enum CondtionType
    //    {
    //        None,
    //        HaveSaveData
    //    }


    //    public ButtonInfo.ButtonType ButtonType { get; private set; }
    //    public List<PopupType> PopupTypes { get; private set; }

    //    public ButtonEventData(ButtonInfo.ButtonType buttonType, List<PopupType> popupTypes)
    //    {
    //        ButtonType = buttonType;
    //        PopupTypes = popupTypes;
    //    }
    //}












    //private Button btn;

    //public ButtonType buttonType;
    //public List<PopupType> popupTypes;




    //void Awake()
    //{
    //    // Button 컴포넌트 초기화
    //    btn = GetComponent<Button>();
    //}

    //public void SetListener(Action<ButtonEventData> callback)
    //{
    //    if (btn != null)
    //    {
    //        btn.onClick.AddListener(() => callback(new ButtonEventData(buttonType, popupTypes)));
    //    }
    //}

    //    // 클릭 이벤트를 설정하는 메서드
    //    //public void SetListener(Action<ButtonType> callback)
    //    //{
    //    //    if (btn != null)
    //    //    {
    //    //        btn.onClick.AddListener(() => callback(buttonType));
    //    //    }
    //    //}



    //    //public Action ClickEvent;

    //    //void Start()
    //    //{
    //    //    btn = GetComponent<Button>();
    //    //    btn.onClick.AddListener(ClickEvent);
    //    //}

    //    //public void SetListner(Action ab)
    //    //{
    //    //    btn = GetComponent<Button>();

    //    //    if (listener != null) button.onClick.AddListener(() =>
    //    //    {
    //    //        ab();
    //    //    });

    //    //    btn.onClick.AddListener(ab);
    //    //}



    //    //public void ButtonAction()
    //    //{

    //    //}



    //    //// Start is called before the first frame update
    //    //void Start()
    //    //{

    //    //}

    //    //// Update is called once per frame
    //    //void Update()
    //    //{

    //    //}
}


//public class ButtonEventData
//{
//    public ButtonInfo.ButtonType ButtonType { get; private set; }
//    public List<PopupType> PopupTypes { get; private set; }

//    public ButtonEventData(ButtonInfo.ButtonType buttonType, List<PopupType> popupTypes)
//    {
//        ButtonType = buttonType;
//        PopupTypes = popupTypes;
//    }
//}


//public class ButtonInfo : MonoBehaviour
//{
//    public ButtonType _buttonType { get; private set; }

//    public CondtionType _condtionType { get; private set; }

//    public List<PopupType> _popupTypes { get; private set; }

//    public ButtonInfo(ButtonType buttonType, CondtionType conditionType, List<PopupType> popupTypes)
//    {
//        _buttonType = buttonType;
//        _condtionType = conditionType;
//        _popupTypes = popupTypes;
//    }


//}
