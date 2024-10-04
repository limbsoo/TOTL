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

    public int EndPhase = 9;

    public bool isSelected;


    public TimeLine[] TL;


    //public UnityEvent selectComplete;

    //지정된 위치 세이브 불러오기

    RectTransform rectTransform;

    private void Start()
    {
        UIManager.instance.OnInitializeUI += CreateBlock;

        rectTransform = GetComponent<RectTransform>();

        blocks = new List<GameObject>();

        TL = slots.GetComponentsInChildren<TimeLine>();


        if (DataManager.Instance.data.curWave >=1 ) SetBlocks(DataManager.Instance.data);
        
        
        if (DataManager.Instance.data.curWave <= 7) CreateBlock();

        //StageManager.instance.OnContinueWave += SetBlocks;

    }

    private void Update()
    {
        //if(blocks.Count > 0) 
        //{
        //    if (blocks[blocks.Count-1].) 
        //    {

        //    }

        //}
    }


    public void SetBlocks(GameData PlayData)
    {
        //RectTransform rect = GetComponent<RectTransform>();

        for (int i = 0; i < PlayData.setBlocks.Count; i++)
        {
            GameObject go = null;
            go = Instantiate(block);
            go.transform.SetParent(rectTransform, false);
            blocks.Add(go);
            FieldEffectBlock feb = go.GetComponent<FieldEffectBlock>();

            feb.Init(PlayData, blocks.Count - 1);

            //image.sprite = idle[Function.instance.checkIdx(DataManager.Instance.data.setBlocks[i].blockName)];
            //TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
            //ttt[blocks[i].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;




        }



    }


    public void CreateBlock()
    {
        //RectTransform rect = GetComponent<RectTransform>();

        if (DataManager.Instance.data.curWave < EndPhase)
        {
            GameObject go = null;
            go = Instantiate(block);
            go.transform.SetParent(rectTransform, false);
            blocks.Add(go);
            FieldEffectBlock feb = go.GetComponent<FieldEffectBlock>();
            feb.Init(null, blocks.Count - 1);
        }

        else
        {
            //강화버전
        }


    }

    //리롤 횟수 제한 추가
    public void RerollBlock()
    {
        FieldEffectBlock feb = blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>();

        feb.m_upperIdx = UnityEngine.Random.Range(0, 3);
        feb.m_downerIdx = UnityEngine.Random.Range(0, 3);

        feb.UpperImage.sprite = feb.UpperSprites[feb.m_upperIdx];
        feb.DownerImage.sprite = feb.DownerSprites[feb.m_downerIdx];
    }



}



//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;

//public class FieldEffectPopUpManager : MonoBehaviour
//{
//    public static FieldEffectPopUpManager instance { get; private set; }

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else Destroy(gameObject);
//    }

//    public List<GameObject> blocks { get; private set; }



//    public Canvas parentCanvas;


//    //private int idx;

//    public Button[] buttons;
//    public Color normalColor;
//    public Color disabledColor;

//    public GameObject block;
//    public Sprite[] idle;


//    public GridLayoutGroup slots;

//    public int EndPhase = 9;


//    //public UnityEvent selectComplete;

//    //지정된 위치 세이브 불러오기

//    void Start()
//    {
//        UIManager.instance.OnInitializeUI += CreateBlock;

//        //나중에 스테이지매니저랑 똑같이할까
//        blocks = new List<GameObject>();

//        //foreach (Button btn in buttons)
//        //{
//        //    btn.onClick.AddListener(() => OnButtonClick(btn));
//        //}





//        if(DataManager.Instance.data.curWave >=1)
//        {
//            SetBlocks();
//        }

//        CreateBlock();

//        //StageManager.instance.OnContinueWave += SetBlocks;

//    }

//    public void SetBlocks()
//    {
//        //GameObject newBlock = blocks[0];
//        //blocks = new List<GameObject>();

//        for(int i = 0; i < DataManager.Instance.data.setBlocks.Count; i++)
//        {
//            CreateBlock();


//            Image image = blocks[i].GetComponent<Image>();

//            image.sprite = idle[Function.instance.checkIdx(DataManager.Instance.data.setBlocks[i].blockName)];
//            TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
//            ttt[blocks[i].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;

//            blocks[i].name = image.sprite.name;

//            FieldEffectBlock feb = blocks[i].GetComponent<FieldEffectBlock>();
//            //feb.rectTransform.position = DataManager.Instance.data.setBlocks[i].position;


//            feb.movePos(DataManager.Instance.data.setBlocks[i].position);

//        }


//    }


//    public void CreateBlock()
//    {
//        RectTransform rect = GetComponent<RectTransform>();
//        GameObject newObject = null;
//        newObject = Instantiate(block);
//        newObject.transform.SetParent(rect.transform, false);
//        blocks.Add(newObject);

//        newObject.GetComponent<FieldEffectBlock>().idx = blocks.Count - 1;

//        newObject.GetComponent<FieldEffectBlock>().UpperIdx = 0;
//        newObject.GetComponent<FieldEffectBlock>().DownerIdx = 0;



//        foreach (Button btn in buttons)
//        {
//            SetButtonColor(btn, normalColor);
//        }
//    }



//    void OnButtonClick(Button clickedButton)
//    {
//        ChangeButtonColor(clickedButton);
//        ChangeBlockColor(clickedButton.name);
//    }

//    void ChangeButtonColor(Button clickedButton)
//    {
//        // 모든 버튼의 색상을 비활성화 색상으로 변경합니다.
//        foreach (Button btn in buttons)
//        {
//            SetButtonColor(btn, disabledColor);
//        }

//        // 클릭된 버튼의 색상을 원래 색상으로 돌립니다.
//        SetButtonColor(clickedButton, normalColor);
//    }

//    void SetButtonColor(Button button, Color color)
//    {
//        ColorBlock cb = button.colors;
//        cb.normalColor = color;
//        cb.highlightedColor = color;
//        cb.pressedColor = color;
//        cb.selectedColor = color;
//        button.colors = cb;
//    }

//    void ChangeBlockColor(string s)
//    {
//        if(blocks.Count != 0)
//        {
//            Image image = blocks[blocks.Count - 1].GetComponent<Image>();

//            image.sprite = idle[Function.instance.checkIdx(s)];


//            //switch (s)
//            //{
//            //    case "Blink":
//            //        image.sprite = idle[0];
//            //        break;
//            //    case "Shadow":
//            //        image.sprite = idle[1];
//            //        break;
//            //    case "Disable":
//            //        image.sprite = idle[2];
//            //        break;
//            //}

//            TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
//            ttt[blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;

//            blocks[blocks.Count - 1].name = image.sprite.name;
//        }

//    }





//}