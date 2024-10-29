using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System.Drawing;

public class FieldEffectPopUpManager : MonoBehaviour
{
    public static FieldEffectPopUpManager instance { get; private set; }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    TimeLine[] m_timelines;
    ButtonEvents m_buttonEvents;
    public int EndPhase;


    public List<GameObject> blocks { get; private set; }



    public Canvas parentCanvas;


    //private int idx;

    //public Button[] buttons;
    //public Color normalColor;
    //public Color disabledColor;

    public GameObject block;
    public Sprite[] idle;


    public GridLayoutGroup slots;



    public bool isSelected;




    public List<TMP_Text> slotEffectExplans = new List<TMP_Text>();


    public UnityEngine.UI.Button Reroll;


    //public UnityEvent selectComplete;

    //지정된 위치 세이브 불러오기

    RectTransform rectTransform;



    private void Start()
    {
        m_timelines = slots.GetComponentsInChildren<TimeLine>();

        UIManager.instance.OnInitializeUI += CreateBlock;

        rectTransform = GetComponent<RectTransform>();

        blocks = new List<GameObject>();




        if (DataManager.Instance.data.curWave >=1 ) SetBlocks(DataManager.Instance.data);
        CreateBlock();

        //if (DataManager.Instance.data.curWave <= 7) CreateBlock();

        //StageManager.instance.OnContinueWave += SetBlocks;

        m_buttonEvents = GetComponent<ButtonEvents>();
        m_buttonEvents.OnCheckMoney?.Invoke(DataManager.Instance.data.gold);


        //EventManager.OnCheckMoney?.Invoke(DataManager.Instance.data.gold);
    }


    public TMP_Text slot1;
    public TMP_Text selectBlockName;
    public TMP_Text selectBlockInformation;


    private void Update()
    {
        //if(Player.instance.gold < 50)
        //{
        //    Reroll.interactable = false;
        //}

        //else
        //{
        //    Reroll.interactable = true;
        //}


        for(int i = 0; i < slotEffectExplans.Count;i++)
        {
            slotEffectExplans[i].text = "";
        }



        if (blocks.Count > 0)
        {
            UpdateTexts();

            for (int i = 0; i < blocks.Count; i++)
            {
                FieldEffectBlock feeeeb = blocks[i].GetComponent<FieldEffectBlock>();

                if(feeeeb.lineNum != -1)
                {
                    string s = "";
                    s += feeeeb.UpperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.DownerSprites[feeeeb.m_downerIdx].name + " field effect occurs from ";




                    s += feeeeb.start.ToString();
                    s += " to ";
                    s += feeeeb.end.ToString();


                    //s += " ";
                    //s += feeeeb.UpperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.DownerSprites[feeeeb.m_downerIdx].name + " Field Effect";

                    slotEffectExplans[feeeeb.lineNum].text = s;











                    //string s = "";
                    //s += feeeeb.start.ToString();
                    //s += " to ";
                    //s += feeeeb.end.ToString();
                    //s += " ";
                    //s += feeeeb.UpperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.DownerSprites[feeeeb.m_downerIdx].name + " Field Effect";

                    //slotEffectExplans[feeeeb.lineNum].text = s;
                }




            }


        }

    }

    public void UpdateTexts()
    {
        FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();
        selectBlockName.text = feb.UpperSprites[feb.m_upperIdx].name + " & " + feb.DownerSprites[feb.m_downerIdx].name;



        string s = "";

        s += "<color=red>";

        s += feb.start.ToString();

        s += "</color>";

        s += " 부터 ";

        s += "<color=red>";


        s += feb.end.ToString();

        s += "</color>";

        s += " 까지 활성화되며 ";

        switch (feb.m_upperIdx)
        {
            case (0): //0 Blink
                s += " 활성화 된 동안 필드 효과가 발생한다. 해당 필드 효과 범위 안에 플레이어 존재 시";
                break;

            case (1): //0 Delay
                s += " 활성화 된 동안 필드 효과가 발생한다. 해당 필드 효과 범위 안에 플레이어 존재 시 3초 뒤에 필드 효과가 적용되며 효과 범위를 벗어나도 발생";
                break;

            case (2): //0 Moving
                s += " 활성화 된 동안 필드 효과가 발생한다. 필드 효과 범위가 움직이며, 해당 필드 효과 범위 안에 플레이어 존재 시";
                break;
        }


        switch (feb.m_downerIdx)
        {
            case (0): //0 Damage
                s += " 일정 데미지를 준다.";
                break;

            case (1): //0 Seal
                s += " 5초간 스킬을 봉인한다.";
                break;

            case (2): //0 Slow
                s += " 3초간 이동속도를 낮춘다.";
                break;
        }

        //s += " Block.a부터 b 기간 동안 활성화되는";

        selectBlockInformation.text = s;














        //string s = "";

        //s += feb.start.ToString();
        //s += " To ";
        //s += feb.end.ToString();


        //switch (feb.m_upperIdx)
        //{
        //    case (0): //0 Blink
        //        s += " Blink & ";
        //        break;

        //    case (1): //0 Delay
        //        s += " Delay & ";
        //        break;

        //    case (2): //0 Moving
        //        s += " Moving & ";
        //        break;
        //}


        //switch (feb.m_downerIdx)
        //{
        //    case (0): //0 Damage
        //        s += "Damage";
        //        break;

        //    case (1): //0 Seal
        //        s += "Seal";
        //        break;

        //    case (2): //0 Slow
        //        s += "Slow";
        //        break;
        //}

        //s += " Block.a부터 b 기간 동안 활성화되는";


        ////if (blocks[blocks.Count - 1])
        ////{

        ////}

        //selectBlockInformation.text = s;

        ////slotEffectExplans[feb.lineNum].text = feb.UpperSprites[feb.m_upperIdx].name + " + " + feb.DownerSprites[feb.m_downerIdx].name + " block";




















        //// 만들때랑 갱신할때만 불러오게
        ////FieldEffectBlock feb = blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>();
        //FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();

        //selectBlockName.text = feb.UpperSprites[feb.m_upperIdx].name + " + " + feb.DownerSprites[feb.m_downerIdx].name + " block";

        //string s = "";

        //switch (feb.m_upperIdx)
        //{
        //    case (0): //0 Blink
        //        s += "a부터 b 기간 동안 활성화되는 ";
        //        break;

        //    case (1): //0 Delay
        //        s += "a부터 b 기간 동안 활성화되며 해당 공간 방문 시 n 초 후에 효과가 발동되는 ";
        //        break;

        //    case (2): //0 Moving
        //        s += "a부터 b 기간 동안 이동하는 ";
        //        break;
        //}


        //switch (feb.m_downerIdx)
        //{
        //    case (0): //0 Damage
        //        s += "m의 데미지를 주는";
        //        break;

        //    case (1): //0 Seal
        //        s += "스킬을 n초간 봉인하는";
        //        break;

        //    case (2): //0 Slow
        //        s += "k만큼 속도를 낮추는";
        //        break;
        //}

        //s += " 블록이다.";


        ////if (blocks[blocks.Count - 1])
        ////{

        ////}

        //selectBlockInformation.text = s;
    }



    public void WriteBlockPeriod(int start, int end)
    {

    }


    public void WriteBlockExplain()
    {

    }




    public void SetBlocks(GameData PlayData)
    {
        //RectTransform rect = GetComponent<RectTransform>();

        for (int i = 0; i < PlayData.febs.Count; i++)
        {
            //GameObject go = null;

            GameObject go = Instantiate(block);


            //go = Instantiate(block);
            go.transform.SetParent(rectTransform, false);
            blocks.Add(go);
            FieldEffectBlock feb = go.GetComponent<FieldEffectBlock>();

            //feb.setRect();

            //feb.rectTransform = new Vector3(-1000, -1000, -1110);

            feb.Init(PlayData, blocks.Count - 1);




            //image.sprite = idle[Function.instance.checkIdx(DataManager.Instance.data.setBlocks[i].blockName)];
            //TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
            //ttt[blocks[i].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;




        }



    }


    public int curBlockIdx;


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
            StartCoroutine(CoWaitForPosition(feb));
            feb.changeSelected();
        }

        else
        {

            FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();
            feb.ChangeColor(false);

            curBlockIdx = UnityEngine.Random.Range(0, 9);
            feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();
            feb.ChangeColor(true);
            feb.weight += 1;

            //강화버전
        }


    }

    public void RandomSelect()
    {

    }

    IEnumerator CoWaitForPosition(FieldEffectBlock feb)
    {
        yield return new WaitForEndOfFrame();

        //feb.SetPos(new Vector3(-1000, -1000, -1110));

        //Vector3 newPos = new Vector3();

        FieldEffectBlock feeb;

        if (DataManager.Instance.data.febs.Count == 0)
        {
            feb.SetPos(m_timelines[0].rectTransform.position);
        }

        else
        {
            List<int> occupied = new List<int>();
            for (int i = 0; i < DataManager.Instance.data.febs.Count; i++)
            {
                //feeb = DataManager.Instance.data.febs[i].GetComponent<FieldEffectBlock>();
                occupied.Add(DataManager.Instance.data.febs[i].lineNum);
            }
            occupied.Sort();
            int idx = 0;

            for(int i = 0; i < occupied.Count; i++)
            {
                if(idx == occupied[i]) idx++;
                else break;
            }

            feb.SetPos(m_timelines[idx].rectTransform.position);
        }


        //if (Player.instance.gold < 50)
        //{
        //    Reroll.interactable = false;
        //}

        //else
        //{
        //    Reroll.interactable = true;
        //}

    }



    //리롤 횟수 제한 추가
    public void RerollBlock()
    {
        FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();

        feb.m_upperIdx = UnityEngine.Random.Range(0, 3);
        feb.m_downerIdx = UnityEngine.Random.Range(0, 3);

        feb.UpperImage.sprite = feb.UpperSprites[feb.m_upperIdx];
        feb.DownerImage.sprite = feb.DownerSprites[feb.m_downerIdx];

        feb.start = UnityEngine.Random.Range(0, 10);
        feb.end = (feb.start + 5) % 10;
        feb.StartText.text = feb.start.ToString();
        feb.EndText.text = feb.end.ToString();

        //if (Player.instance.gold < 50)
        //{
        //    Reroll.interactable = false;
        //}


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