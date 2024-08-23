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


    //    // ��� ��ư�� ������ ��Ȱ��ȭ �������� �����մϴ�.
    //    foreach (Button btn in buttons)
    //    {
    //        SetButtonColor(btn, disabledColor);
    //    }

    //    // Ŭ���� ��ư�� ������ ���� �������� �����ϴ�.
    //    SetButtonColor(clickedButton, normalColor);

    //    OnSelectedBlock.Invoke();
    //    //OnSelectedBlock1.Invoke();
    //    clikkkkkk.Invoke();
    //    // specialButton�� Ȱ��ȭ�մϴ�.
    //    specialButton.interactable = true;
    //    SetButtonColor(specialButton, normalColor);
    //}

    public Button[] buttons;
    public Color normalColor = Color.white; // �⺻ ��ư ����
    public Color disabledColor = Color.gray; // ��Ȱ��ȭ ���¿��� ����� ����
    public Button specialButton;

    public UnityEvent OnSelectedBlock;

    //public Sprite[] newSprites;

    public Action clikkkkkk;


    //public event EventHandler




    void Start()
    {



        //specialButton.interactable = false;
        //SetButtonColor(specialButton, disabledColor);


        // �� ��ư�� Ŭ�� �̺�Ʈ�� ����մϴ�.
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        // ��� ��ư�� ������ ��Ȱ��ȭ �������� �����մϴ�.
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, disabledColor);
        }

        // Ŭ���� ��ư�� ������ ���� �������� �����ϴ�.
        SetButtonColor(clickedButton, normalColor);

        OnSelectedBlock.Invoke();
        //OnSelectedBlock1.Invoke();

        //// specialButton�� Ȱ��ȭ�մϴ�.
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
        Debug.Log("����");
    }
}
