using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonStateController : MonoBehaviour
{
    //public enum ButtonState {None, DeActivate, Activate}

    //public ButtonState m_state;


    private Button btn;

    private int m_cost;

    //[SerializeField]
    CanvasGroup canvasGroup;

    ButtonEvents buttonEvents;


    private void Start()
    {
        //btn = GetComponent<Button>();
        ////btn.onClick.AddListener(ActivateButtonAction);
        //m_cost = StageManager.instance.LCS.InitrerollCost;
        //canvasGroup = GetComponent<CanvasGroup>();
    }


    private void ChangeState(int gold)
    {
        btn = GetComponent<Button>();
        //btn.onClick.AddListener(ActivateButtonAction);
        m_cost = StageManager.instance.LCS.InitrerollCost;
        canvasGroup = GetComponent<CanvasGroup>();
        



        if (m_cost <= gold)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            btn.interactable = true;
            //m_state = ButtonState.DeActivate;
        }

        else
        {
            canvasGroup.alpha = .6f;
            canvasGroup.blocksRaycasts = false;
            btn.interactable = false;
            //m_state = ButtonState.Activate;
        }

    }


    private void OnEnable()
    {
        buttonEvents = GetComponentInParent<ButtonEvents>();
        buttonEvents.OnCheckMoney += ChangeState;
        //EventManager.OnCheckMoney += ChangeState;
    }

    private void OnDisable()
    {
        buttonEvents.OnCheckMoney -= ChangeState;

        // EventManager.OnCheckMoney -= ChangeState;
    }


}
