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

    //지정된 위치 세이브 불러오기

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
        // 모든 버튼의 색상을 비활성화 색상으로 변경합니다.
        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, disabledColor);
        }

        // 클릭된 버튼의 색상을 원래 색상으로 돌립니다.
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
