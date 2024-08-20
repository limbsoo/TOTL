using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockSelect : MonoBehaviour
{
    public Button[] buttons;
    public Color normalColor = Color.white; // 기본 버튼 색상
    public Color disabledColor = Color.gray; // 비활성화 상태에서 사용할 색상
    public Button specialButton;


    public Sprite[] newSprites;

    void Start()
    {
        specialButton.interactable = false;
        SetButtonColor(specialButton, disabledColor);


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

        // specialButton을 활성화합니다.
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
    //    // 모든 버튼을 비활성화합니다.
    //    foreach (Button btn in buttons)
    //    {
    //        btn.interactable = false;
    //    }

    //    // 클릭된 버튼만 다시 활성화합니다.
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
