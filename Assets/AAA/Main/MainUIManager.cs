using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUIManager : MonoBehaviour
{
    public List<GameObject> Buttons;
















    ////public static MainSceneManager instance { get; private set; }





    ////private void Awake()
    ////{
    ////    if (instance == null)
    ////    {
    ////        instance = this;
    ////        DontDestroyOnLoad(gameObject);
    ////    }

    ////    else { return; }
    ////}

    //public SceneConstructSet SCS;



    //public void Starttttt()
    //{

    //}

    ////public Action<ButtonEventData> buttonClicked;

    //void Start()
    //{
    //    ButtonInfo[] buttonInfos = gameObject.GetComponentsInChildren<ButtonInfo>();

    //    for (int i = 0; i < buttonInfos.Length; i++)
    //    {
    //        ButtonInfo buttonInfo = buttonInfos[i];

    //        if (buttonInfo != null)
    //        {
    //            buttonInfo.SetListener(buttonClicked);
    //        }
    //    }
    //}

    //void buttonClicked(ButtonEventData eventData)
    //{
    //    switch (eventData.ButtonType)
    //    {
    //        case ButtonInfo.ButtonType.ShowPopUP:
    //            Debug.Log("ShowPopUP 버튼 클릭됨");
    //            break;
    //        case ButtonInfo.ButtonType.CheckAndShowPopUP:
    //            Debug.Log("CheckAndShowPopUP 버튼 클릭됨");
    //            break;
    //        case ButtonInfo.ButtonType.Confirm:
    //            Debug.Log("Confirm 버튼 클릭됨");
    //            break;
    //        case ButtonInfo.ButtonType.Close:
    //            Debug.Log("Close 버튼 클릭됨");
    //            break;
    //        default:
    //            Debug.LogWarning("알 수 없는 버튼 타입");
    //            break;
    //    }

    //    // PopupTypes 리스트 접근
    //    foreach (var popupType in eventData.PopupTypes)
    //    {
    //        Debug.Log($"PopupType: {popupType}");
    //    }
    //}


    ////    GameObject[] list = gameObject.GetComponentsInChildren<GameObject>();


    ////    for(int i = 0; i < list.Length; i++)
    ////    {
    ////        ButtonInfo buttonInfo = list[i].GetComponent<ButtonInfo>();

    ////        if (buttonInfo != null) 
    ////        {
    ////            buttonInfo.SetListner(buttonClicked);


    ////            //buttonInfo.abd += Starttttt;


    ////        }
    ////    }

    ////}

    ////// Update is called once per frame
    ////void Update()
    ////{

    ////}
}


