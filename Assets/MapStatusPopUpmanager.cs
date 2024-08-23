using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class MapStatusPopUpmanager : MonoBehaviour
{
    public Button[] buttons;
    public Color normalColor;
    public Color disabledColor;

    public GameObject block;
    public Sprite[] idle;

    //������ ��ġ ���̺� �ҷ�����

    void Start()
    {
        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }
    }

    void OnButtonClick(Button clickedButton)
    {
        ChangeButtonColor(clickedButton);
        ChangeBlockColor(clickedButton.name);
    }

    void ChangeButtonColor(Button clickedButton)
    {
        // ��� ��ư�� ������ ��Ȱ��ȭ �������� �����մϴ�.
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, disabledColor);
        }

        // Ŭ���� ��ư�� ������ ���� �������� �����ϴ�.
        SetButtonColor(clickedButton, normalColor);
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

    void ChangeBlockColor(string s)
    {
        Image image = block.GetComponent<Image>();

        switch (s)
        {
            case "Blink":
                image.sprite = idle[0];
                break;
            case "Shadow":
                image.sprite = idle[1];
                break;
            case "Disable":
                image.sprite = idle[2];
                break;
        }
    }





}
