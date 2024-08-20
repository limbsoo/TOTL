using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSelect : MonoBehaviour
{
    public Button[] buttons;
    public Color normalColor = Color.white; // �⺻ ��ư ����
    public Color disabledColor = Color.gray; // ��Ȱ��ȭ ���¿��� ����� ����
    public Button specialButton;


    public Sprite[] newSprites;

    void Start()
    {
        specialButton.interactable = false;
        SetButtonColor(specialButton, disabledColor);


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

        // specialButton�� Ȱ��ȭ�մϴ�.
        specialButton.interactable = true;
        SetButtonColor(specialButton, normalColor);
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

    //void OnButtonClick(Button clickedButton)
    //{
    //    // ��� ��ư�� ��Ȱ��ȭ�մϴ�.
    //    foreach (Button btn in buttons)
    //    {
    //        btn.interactable = false;
    //    }

    //    // Ŭ���� ��ư�� �ٽ� Ȱ��ȭ�մϴ�.
    //    clickedButton.interactable = true;
    //}









    //public int selectIdx;

    //public GameObject[] gs;

    //public void selectedOne(GameObject g)
    //{
    //    for(int i = 0; i < gs.Length; i++) 
    //    {
    //        if(g != gs[i])
    //        {
    //            SelectOption so = gs[i].GetComponent<SelectOption>();
    //            so.selecting();
    //        }

    //    }
    //}
}
