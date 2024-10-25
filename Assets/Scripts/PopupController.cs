using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static ButtonController;
using static UnityEngine.EventSystems.EventTrigger;

//public enum ConditionOperator
//{
//    HaveSaveData, abcd
//}

//[SerializeField]
public class PopupController : MonoBehaviour
{
    public enum PopUpStyle { Maintain, Disappear}
    public enum PopUpEffect { None, Stop }

    public PopUpStyle popUpStyle;
    public PopUpEffect popUpEffect;

    void OnEnable()
    {
        switch (popUpStyle)
        {
            case PopUpStyle.Maintain:
                break;
            case PopUpStyle.Disappear:

                StartCoroutine(Function.instance.CountDown(2f, () =>
                {
                    gameObject.SetActive(false);
                }));

                break;
        }

        if(popUpEffect == PopUpEffect.Stop)
        {
            Time.timeScale = 0f;
        }
    }

    private void OnDisable()
    {
        if (popUpEffect == PopUpEffect.Stop)
        {
            Time.timeScale = 1f;
        }
    }






    ////public ConditionOperator co;

    ////[Serializable]



    //public MainSceneUIManager mainSceneUIManager;


    ////[System.Serializable]
    ////public class HesitateOpenPopupEvent : UnityEvent<string[], ConditionOperator> { }

    ////public HesitateOpenPopupEvent onHesitateOpenPopup; // 인스펙터에서 할당할 수 있는 UnityEvent


    ////[SerializeField]
    //public void HesitateOpenPopup(string[] popupIDs)
    //{
    //    ////ConditionOperator conditionOperator

    //    //switch (0) 
    //    //{
    //    //    case 0:
    //    //        if(HaveSaveData()) mainSceneUIManager.ShowPopup(popupIDs[0]);
    //    //        else mainSceneUIManager.ShowPopup(popupIDs[1]);
    //    //        break;

    //    //}

    //    ////private Dictionary<string, GameObject> popups
    //}

    //public void OpenPopup(string popupID)
    //{
    //    mainSceneUIManager.ShowPopup(popupID);
    //}

    //public void ClosePopup(string popupID)
    //{
    //    mainSceneUIManager.HidePopup(popupID);
    //}

    //public bool HaveSaveData()
    //{
    //    DataManager.Instance.HaveSaveData();
    //    return true;
    //}
}
