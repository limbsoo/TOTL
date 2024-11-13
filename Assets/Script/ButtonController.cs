using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private Button btn;




    public enum ButtonAction { Run, CheckNRun, Next, DeActivate }

    public enum ButtonCondition { None, HaveSaveData }
    public enum ButtonDirection { None, Left, Right }

    public List<GameObject> TargetObjects;

    public ButtonAction buttonAction;
    public ButtonCondition buttonCondition;
    public ButtonDirection buttonDirection;

    public static Action OnLoadScene;

    private int curIdx = 0;

    [HideInInspector]
    public List<string> optionList = new List<string>();


    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(ActivateButtonAction);
    }




    public void ActivateButtonAction()
    {
        switch (buttonAction)
        {
            case ButtonAction.Run:
                TargetObjects[0].SetActive(true);
                break;

            case ButtonAction.CheckNRun:
                switch (buttonCondition)
                {
                    case ButtonCondition.None:
                        break;

                    case ButtonCondition.HaveSaveData:

                        if (DataManager.Instance.HaveSaveData())
                        {
                            if(TargetObjects[0] == null) OnLoadScene.Invoke();

                            else TargetObjects[0].SetActive(true);
                        } 

                        else TargetObjects[1].SetActive(true);

                        break;

                }
                break;

            case ButtonAction.Next:

                TargetObjects[curIdx].SetActive(false);

                switch (buttonDirection)
                {
                    case ButtonDirection.Left:
                        curIdx--;
                        if (curIdx < 0) curIdx = TargetObjects.Count - 1;
                        break;

                    case ButtonDirection.Right:
                        curIdx++;
                        if (curIdx > TargetObjects.Count - 1) curIdx = 0;
                        break;
                }

                TargetObjects[curIdx].SetActive(true);

                MainScene.instance.playerIdx = curIdx;

                break;

            case ButtonAction.DeActivate:
                curIdx = 0;

                gameObject.transform.parent.gameObject.SetActive(false);

                //TargetObjects[0].SetActive(false);
                break;


        }
    }


    //// Start is called before the first frame update


    //// Update is called once per frame
    //void Update()
    //{

    //}
}
