using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ButtonClickEvent : MonoBehaviour
{
    //void Start()
    //{
    //    Button btn = GetComponent<Button>();
    //    //btn.onClick.AddListener(() => OnButtonClick(btn));

    //    btn.onClick.AddListener(() => EventManager.instance.clickEvent(btn));
    //}

    //public Action clikkkkkk;

    //void OnButtonClick(Button clickedButton)
    //{
    //    //clikkkkkk.Invoke();


    //    // 모든 버튼의 색상을 비활성화 색상으로 변경합니다.
    //    foreach (Button btn in buttons)
    //    {
    //        SetButtonColor(btn, disabledColor);
    //    }

    //    // 클릭된 버튼의 색상을 원래 색상으로 돌립니다.
    //    SetButtonColor(clickedButton, normalColor);

    //    OnSelectedBlock.Invoke();
    //    //OnSelectedBlock1.Invoke();
    //    clikkkkkk.Invoke();
    //    // specialButton을 활성화합니다.
    //    specialButton.interactable = true;
    //    SetButtonColor(specialButton, normalColor);
    //}

    public Button[] buttons;
    public Color normalColor = Color.white; // 기본 버튼 색상
    public Color disabledColor = Color.gray; // 비활성화 상태에서 사용할 색상
    public Button specialButton;

    public UnityEvent OnSelectedBlock;

    //public Sprite[] newSprites;

    public Action clikkkkkk;


    //public event EventHandler




    void Start()
    {



        //specialButton.interactable = false;
        //SetButtonColor(specialButton, disabledColor);


        // 각 버튼에 클릭 이벤트를 등록합니다.
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        // 모든 버튼의 색상을 비활성화 색상으로 변경합니다.
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, disabledColor);
        }

        // 클릭된 버튼의 색상을 원래 색상으로 돌립니다.
        SetButtonColor(clickedButton, normalColor);

        OnSelectedBlock.Invoke();
        //OnSelectedBlock1.Invoke();

        //// specialButton을 활성화합니다.
        //specialButton.interactable = true;
        //SetButtonColor(specialButton, normalColor);
    }

    void SetButtonColor(Button button, Color color)
    {
        ColorBlock cb = button.colors;
        cb.normalColor = color;
        cb.highlightedColor = color;
        cb.pressedColor = color;
        cb.selectedColor = color;
        button.colors = cb;
    }

    void selectedBlock()
    {
        Debug.Log("구독");
    }
}
