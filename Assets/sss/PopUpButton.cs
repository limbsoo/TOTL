using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpButton : MonoBehaviour
{
    public PopupButtonType popupButtonType;
    public PopupButtonConditionType popupButtonConditionType;

    Button btn;

    List<PopUp> popUps;


    public void ClickButton()
    {
        if(popupButtonConditionType != PopupButtonConditionType.None)
        {
            switch (popupButtonConditionType)
            {
                //캐릭터 있는지
                case PopupButtonConditionType.HaveSaveData:

                    if (DataManager.Instance.HaveSaveData()) { ActivePopUp(PopupType.DataIsExist); }
                    else { ActivePopUp(PopupType.SelectCharacter); }
                    break;


            }
        }

        else
        {
            switch (popupButtonType)
            {
                case PopupButtonType.ShowPopUP:
                    popUps[0].gameObject.SetActive(true);
                    break;

                case PopupButtonType.Close:
                    gameObject.transform.parent.gameObject.SetActive(false);
                    break;

            }

        }




    }

    public void ActivePopUp(PopupType popupType)
    {

    }



    private void OnEnable()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ClickButton);






        //PopUpButton[] popUpButtons = gameObject.GetComponentsInChildren<PopUpButton>();

        //for (int i = 0; i < popUpButtons.Length; i++)
        //{
        //    if (popUpButtons[i] != null)
        //    {
        //        buttonInfo.SetListener(buttonClicked);
        //    }
        //}



    }

    private void OnDisable()
    {
    }



    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
