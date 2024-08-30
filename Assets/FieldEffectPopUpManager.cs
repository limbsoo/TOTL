using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class FieldEffectPopUpManager : MonoBehaviour
{
    public static FieldEffectPopUpManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public List<GameObject> blocks { get; private set; }



    public Canvas parentCanvas;


    //private int idx;

    public Button[] buttons;
    public Color normalColor;
    public Color disabledColor;

    public GameObject block;
    public Sprite[] idle;


    public GridLayoutGroup slots;

    //public UnityEvent selectComplete;

    //������ ��ġ ���̺� �ҷ�����

    void Start()
    {
        UIManager.instance.OnInitializeUI += CreateBlock;

        blocks = new List<GameObject>();

        foreach (Button btn in buttons)
        {
            btn.onClick.AddListener(() => OnButtonClick(btn));
        }

        CreateBlock();
    }

    public void CreateBlock()
    {
        RectTransform rect = GetComponent<RectTransform>();
        GameObject newObject = null;
        newObject = Instantiate(block);
        newObject.transform.SetParent(rect.transform, false);
        blocks.Add(newObject);

        foreach (Button btn in buttons)
        {
            SetButtonColor(btn, normalColor);
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
        if(blocks.Count != 0)
        {
            Image image = blocks[blocks.Count - 1].GetComponent<Image>();

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

            TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
            ttt[blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;
        }

    }





}
