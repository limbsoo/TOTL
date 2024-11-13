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

    public TimeLine[] m_timelines;
    ButtonEvents m_buttonEvents;
    public int EndPhase;

    public GameObject slots;



    public List<GameObject> blocks { get; private set; }



    public Canvas parentCanvas;


    //private int idx;

    //public Button[] buttons;
    //public Color normalColor;
    //public Color disabledColor;

    public GameObject block;
    public Sprite[] idle;


    //public GridLayoutGroup slots;



    public bool isSelected;




    public List<TMP_Text> slotEffectExplans = new List<TMP_Text>();


    public UnityEngine.UI.Button Reroll;





    RectTransform rectTransform;

    public TMP_Text Cost;

    private void Start()
    {
        UIManager.instance.OnInitializeUI += CreateBlock;

        m_timelines = slots.GetComponentsInChildren<TimeLine>();
        rectTransform = GetComponent<RectTransform>();
        blocks = new List<GameObject>();

        SetBlocks(DataManager.Instance.saveData);

        MapPlacementBlock mpb = blocks[blocks.Count - 1].GetComponent<MapPlacementBlock>();
        mpb.changeSelected();


        //if (DataManager.Instance.data.curWave > 1) 
        //{ 
        //    SetBlocks(DataManager.Instance.data);
        //}



        //CreateBlock();




        //if (DataManager.Instance.data.curWave > StageManager.instance.LCS.endArrage) { EnchanceBlock(); }
        //else { CreateBlock(); }


        m_buttonEvents = GetComponent<ButtonEvents>();
        m_buttonEvents.OnCheckMoney?.Invoke(DataManager.Instance.saveData.gold);

    }


    public TMP_Text slot1;
    public TMP_Text selectBlockName;
    public TMP_Text selectBlockInformation;
    public TMP_Text selectBlockWeight;

    //private void Update()
    //{

    //    for (int i = 0; i < slotEffectExplans.Count; i++)
    //    {
    //        slotEffectExplans[i].text = "";
    //    }

    //    if (blocks.Count > 0) { }
    //}

    private void Update()
    {
        if (blocks.Count > 0)
        {
            UpdateTexts();
        }
    }

    public void UpdateTexts()
    {
        string s;
        BlockData bd;

        Cost.text = StageManager.instance.LCS.InitrerollCost.ToString();


        for (int i = 0; i < slotEffectExplans.Count; i++)
        {
            slotEffectExplans[i].text = "";
        }


        for (int i = 0; i < blocks.Count; i++)
        {
            s = "";

            bd = blocks[i].GetComponent<MapPlacementBlock>().GetBlockData();

            s += bd.start.ToString() + " - ";
            s += bd.end.ToString();

            //if (bd.weightKinds == WeightKinds.ActiveTime)
            //{
            //    s += SetTextColor("red", bd.end + bd.weight);
            //}

            //else { s += bd.end.ToString(); }

            s += " 초 동안  ";
            s += bd.fieldKinds.ToString() + " Field ";
            s += bd.effectKinds.ToString() + " Effect ";

            slotEffectExplans[bd.lineNum].text = s;
        }





        bd = blocks[curBlockIdx].GetComponent<MapPlacementBlock>().GetBlockData();
        s = "";

        s += "필드 시간 " + bd.start.ToString() + " 초에 활성화되어 ";


        s +=  bd.fieldDuration.ToString() /*+ " +(" + bd.weight.ToString() + ")"*/ + " 초 동안  ";

        //if (bd.weightKinds == WeightKinds.ActiveTime)
        //{
        //    s += SetTextColor("red", bd.end + bd.weight);
        //}

        //else { s += bd.end.ToString(); }


        switch (bd.fieldKinds)
        {
            case FieldKinds.Long:
                break;
            case FieldKinds.Stack:
                s += "플레이어가 해당구역을 진입하여 ";
                s += bd.fieldDuration.ToString() + " 초 이상 머무를 경우 " /*+ bd.fieldValue.ToString() + " * "*/;

                break;
            case FieldKinds.Move:
                s += "일정한 속도로 움직이며 " + "플레이어가 해당구역 진입 시 ";
                break;
        }


        s += (bd.effectValue + bd.weight).ToString();

        switch (bd.effectKinds)
        {
            case EffectKinds.Damage:
                //s += bd.effectValue.ToString();
                //s += " +(" + bd.weight.ToString() + ")";
                s += " 의 데미지를 받는다.";
                break;
            case EffectKinds.Slow:
                //s += bd.effectValue.ToString();
                //s += " +(" + bd.weight.ToString() + ")";
                s += " 초 동안 이동속도가 감소한다."; 
                break;
            case EffectKinds.Seal:
                //s += bd.effectValue.ToString();
                //s += " +(" + bd.weight.ToString() + ")";
                s += " 초 동안 스킬이 봉인된다.";

                break;
        }

        selectBlockInformation.text = s;

        selectBlockWeight.text = bd.weight.ToString();
    }


    public string SetTextColor(string color, float value)
    {
        string s = "";
        s += "<color=";
        s += color;
        s += ">";
        s += value;
        s += "</color>";
        return s;
    }


    //public void UpdateTexts()
    //{
    //    string s;
    //    BlockData bd;

    //    Cost.text = StageManager.instance.LCS.InitrerollCost.ToString();


    //    for (int i = 0; i < slotEffectExplans.Count;i++)
    //    {
    //        slotEffectExplans[i].text = "";
    //    }


    //    for (int i = 0; i < blocks.Count; i++)
    //    {
    //        s = "";

    //        bd = blocks[i].GetComponent<MapPlacementBlock>().GetBlockData();

    //        s += "<color=red>" + bd.start.ToString() + " - " + bd.end.ToString() + "</color>" + " 초 동안  ";
    //        s += "<color=yellow>" + bd.fieldKinds.ToString() + "</color>" + " Field " ;
    //        s += "<color=green>" + bd.effectKinds.ToString()  + "</color>" + " Effect ";

    //        slotEffectExplans[bd.lineNum].text = s;
    //    }

    //    bd = blocks[curBlockIdx].GetComponent<MapPlacementBlock>().GetBlockData();
    //    s = "";

    //    s += "필드 시간 " +  "<color=red>" + bd.start.ToString() + "</color>" + " 초에 활성화되어 ";
    //    s += "<color=red>" +  bd.fieldDuration.ToString() + " +(" + bd.weight.ToString() + ")" + "</color>" + " 초 동안  "; //"간 플레이어가 해당구역 진입 시 ";

    //    switch (bd.fieldKinds)
    //    {
    //        case FieldKinds.Long:


    //            break;
    //        case FieldKinds.Stack:
    //            s += "플레이어가 해당구역을 진입하여 ";
    //            s += bd.fieldDuration.ToString() + " 초 이상 머무를 경우 " + bd.fieldValue.ToString() + " * ";

    //            break;
    //        case FieldKinds.Move:
    //            s += "일정한 속도로 움직이며 " + "플레이어가 해당구역 진입 시 ";
    //            break;
    //    }

    //    s += "<color=green>";


    //    switch (bd.effectKinds)
    //    {
    //        case EffectKinds.Damage:
    //            s += bd.effectValue.ToString() + "</color>" + " 의 데미지를 받는다.";

    //            break;
    //        case EffectKinds.Slow:
    //            s += bd.effectValue.ToString() + "</color>" + " 초 동안 이동속도가 감소한다.";

    //            break;
    //        case EffectKinds.Seal:
    //            s += bd.effectValue.ToString() + "</color>" + " 초 동안 스킬이 봉인된다.";
    //            break;
    //    }

    //    selectBlockInformation.text = s;
    //}


    public void EnchanceBlock()
    {
        MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
        feb.ChangeColor(false);

        curBlockIdx = UnityEngine.Random.Range(0, 9);
        feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
        feb.ChangeColor(true);
        //feb.PlusWeight();
    }


    public void SetBlocks(GameData PlayData)
    {
        if (PlayData.blockdata.Count >= 1)
        {
            for (int i = 0; i < PlayData.blockdata.Count; i++)
            {
                GameObject go = Instantiate(block);
                go.transform.SetParent(rectTransform, false);
                blocks.Add(go);
                MapPlacementBlock feb = go.GetComponent<MapPlacementBlock>();
                //feb.Init(PlayData, blocks.Count - 1);

                feb.Init(PlayData, blocks.Count - 1, PlayData.blockdata[i].lineNum, m_timelines[i].rectTransform.position);

                //feb.movePos(new Vector3(StageManager.instance.gridCenters[PlayData.bd[i].lineNum].x, StageManager.instance.gridCenters[PlayData.bd[i].lineNum].y, StageManager.instance.gridCenters[PlayData.bd[i].lineNum].z));

            }
        }


        else
        {
            GameObject go = Instantiate(block);
            //GameObject go = null;
            //go = Instantiate(block);
            go.transform.SetParent(rectTransform, false);
            blocks.Add(go);
            MapPlacementBlock mpb = go.GetComponent<MapPlacementBlock>();

            List<int> list = new List<int>();

            for (int i = 0; i < DataManager.Instance.saveData.blockdata.Count; i++)
            {
                list.Add(DataManager.Instance.saveData.blockdata[i].lineNum);
            }

            if (list.Count > 0)
            {
                list.Sort();


                int i = 0;
                for (; i < list.Count; i++)
                {
                    if (list[i] != i)
                    {
                        break;
                    }
                }

                mpb.Init(null, blocks.Count - 1, i, m_timelines[i].rectTransform.position);
            }


            else
            {
                mpb.Init(null, blocks.Count - 1, 0, m_timelines[0].rectTransform.position);
            }
            //mpb.changeSelected();
        }

        

        DataManager.Instance.UpdateBlockData();
    }


    public int curBlockIdx;


    public void CreateBlock()
    {
        //wavetime == 10

        if(StageManager.instance.LCS.endArrage >= StageManager.instance.GetCurWave())
        {
            GameObject go = Instantiate(block);
            go.transform.SetParent(rectTransform, false);
            blocks.Add(go);
            MapPlacementBlock mpb = go.GetComponent<MapPlacementBlock>();

            List<int> list = new List<int>();

            for (int i = 0; i < DataManager.Instance.saveData.blockdata.Count; i++)
            {
                list.Add(DataManager.Instance.saveData.blockdata[i].lineNum);
            }

            if (list.Count > 0)
            {
                list.Sort();
                int i = 0;

                for (; i < list.Count; i++)
                {
                    if (list[i] != i) { break; }
                }

                mpb.Init(null, blocks.Count - 1, i, m_timelines[i].rectTransform.position);
            }


            else
            {
                mpb.Init(null, blocks.Count - 1, 0, m_timelines[0].rectTransform.position);
            }


            mpb.changeSelected();
        }

        else
        {
            int randNum = UnityEngine.Random.Range(0, blocks.Count);
            MapPlacementBlock mpb = blocks[randNum].GetComponent<MapPlacementBlock>();

            mpb.changeSelectedAndWeight();



        }









        //mpb.Init(null, blocks.Count - 1);
        //StartCoroutine(CoWaitForPosition(mpb));

        //StartCoroutine(Function.instance.CountDown(1f, () =>
        //{
        //    onceDamaged = false;
        //    gameObject.GetComponent<MeshRenderer>().material = mat[0];
        //}));






        DataManager.Instance.UpdateBlockData();

    }


    //IEnumerator CoWaitForPosition(MapPlacementBlock feb)
    //{
    //    yield return new WaitForEndOfFrame();

    //    MapPlacementBlock feeb;

    //    if (DataManager.Instance.data.bd.Count == 0)
    //    {
    //        feb.SetPos(m_timelines[0].rectTransform.position);
    //    }

    //    else
    //    {
    //        List<int> occupied = new List<int>();
    //        for (int i = 0; i < DataManager.Instance.data.bd.Count; i++)
    //        {
    //            //feeb = DataManager.Instance.data.febs[i].GetComponent<FieldEffectBlock>();
    //            occupied.Add((int)DataManager.Instance.data.bd[i].lineNum);
    //        }
    //        occupied.Sort();
    //        int idx = 0;

    //        for (int i = 0; i < occupied.Count; i++)
    //        {
    //            if (idx == occupied[i]) idx++;
    //            else break;
    //        }

    //        feb.SetPos(m_timelines[idx].rectTransform.position);
    //    }

    //    DataManager.Instance.SaveGameData();
    //}



    //리롤 횟수 제한 추가
    public void RerollBlock()
    {
        MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
        feb.Reroll();
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







































//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using TMPro;
//using UnityEngine.Rendering.Universal;
//using UnityEngine.UIElements;
//using Unity.VisualScripting;
//using System.Drawing;

//public class FieldEffectPopUpManager : MonoBehaviour
//{
//    public static FieldEffectPopUpManager instance { get; private set; }

//    void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else Destroy(gameObject);
//    }

//    TimeLine[] m_timelines;
//    ButtonEvents m_buttonEvents;
//    public int EndPhase;


//    public List<GameObject> blocks { get; private set; }



//    public Canvas parentCanvas;


//    //private int idx;

//    //public Button[] buttons;
//    //public Color normalColor;
//    //public Color disabledColor;

//    public GameObject block;
//    public Sprite[] idle;


//    public GridLayoutGroup slots;



//    public bool isSelected;




//    public List<TMP_Text> slotEffectExplans = new List<TMP_Text>();


//    public UnityEngine.UI.Button Reroll;


//    RectTransform rectTransform;



//    private void Start()
//    {
//        UIManager.instance.OnInitializeUI += CreateBlock;

//        m_timelines = slots.GetComponentsInChildren<TimeLine>();
//        rectTransform = GetComponent<RectTransform>();
//        blocks = new List<GameObject>();

//        if (DataManager.Instance.data.curWave >= 1) { SetBlocks(DataManager.Instance.data); }

//        if (DataManager.Instance.data.curWave > StageManager.instance.LCS.endArrage) { CreateBlock(); }
//        else { CreateBlock(); }


//        m_buttonEvents = GetComponent<ButtonEvents>();
//        m_buttonEvents.OnCheckMoney?.Invoke(DataManager.Instance.data.gold);

//    }


//    public TMP_Text slot1;
//    public TMP_Text selectBlockName;
//    public TMP_Text selectBlockInformation;


//    private void Update()
//    {
//        //if(Player.instance.gold < 50)
//        //{
//        //    Reroll.interactable = false;
//        //}

//        //else
//        //{
//        //    Reroll.interactable = true;
//        //}


//        for(int i = 0; i < slotEffectExplans.Count;i++)
//        {
//            slotEffectExplans[i].text = "";
//        }



//        if (blocks.Count > 0)
//        {
//            //UpdateTexts();

//            //for (int i = 0; i < blocks.Count; i++)
//            //{
//            //    FieldEffectBlock feeeeb = blocks[i].GetComponent<FieldEffectBlock>();

//            //    if(feeeeb.lineNum != -1)
//            //    {
//            //        string s = "";
//            //        s += feeeeb.upperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.downerSprites[feeeeb.m_downerIdx].name + " field effect occurs from ";




//            //        s += feeeeb.start.ToString();
//            //        s += " to ";
//            //        s += feeeeb.end.ToString();


//            //        //s += " ";
//            //        //s += feeeeb.UpperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.DownerSprites[feeeeb.m_downerIdx].name + " Field Effect";

//            //        slotEffectExplans[feeeeb.lineNum].text = s;











//            //        //string s = "";
//            //        //s += feeeeb.start.ToString();
//            //        //s += " to ";
//            //        //s += feeeeb.end.ToString();
//            //        //s += " ";
//            //        //s += feeeeb.UpperSprites[feeeeb.m_upperIdx].name + " & " + feeeeb.DownerSprites[feeeeb.m_downerIdx].name + " Field Effect";

//            //        //slotEffectExplans[feeeeb.lineNum].text = s;
//            //    }




//            //}


//        }

//    }

//    //public void UpdateTexts()
//    //{
//    //    FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();
//    //    selectBlockName.text = feb.upperSprites[feb.m_upperIdx].name + " & " + feb.downerSprites[feb.m_downerIdx].name;



//    //    string s = "";

//    //    s += "<color=red>";

//    //    s += feb.start.ToString();

//    //    s += "</color>";

//    //    s += " 부터 ";

//    //    s += "<color=red>";


//    //    s += feb.end.ToString();

//    //    s += "</color>";

//    //    s += " 까지 활성화되며 ";

//    //    switch (feb.m_upperIdx)
//    //    {
//    //        case (0): //0 Blink
//    //            s += " 활성화 된 동안 필드 효과가 발생한다. 해당 필드 효과 범위 안에 플레이어 존재 시";
//    //            break;

//    //        case (1): //0 Delay
//    //            s += " 활성화 된 동안 필드 효과가 발생한다. 해당 필드 효과 범위 안에 플레이어 존재 시 3초 뒤에 필드 효과가 적용되며 효과 범위를 벗어나도 발생";
//    //            break;

//    //        case (2): //0 Moving
//    //            s += " 활성화 된 동안 필드 효과가 발생한다. 필드 효과 범위가 움직이며, 해당 필드 효과 범위 안에 플레이어 존재 시";
//    //            break;
//    //    }


//    //    switch (feb.m_downerIdx)
//    //    {
//    //        case (0): //0 Damage
//    //            s += " 일정 데미지를 준다.";
//    //            break;

//    //        case (1): //0 Seal
//    //            s += " 5초간 스킬을 봉인한다.";
//    //            break;

//    //        case (2): //0 Slow
//    //            s += " 3초간 이동속도를 낮춘다.";
//    //            break;
//    //    }

//    //    //s += " Block.a부터 b 기간 동안 활성화되는";

//    //    selectBlockInformation.text = s;














//    //    //string s = "";

//    //    //s += feb.start.ToString();
//    //    //s += " To ";
//    //    //s += feb.end.ToString();


//    //    //switch (feb.m_upperIdx)
//    //    //{
//    //    //    case (0): //0 Blink
//    //    //        s += " Blink & ";
//    //    //        break;

//    //    //    case (1): //0 Delay
//    //    //        s += " Delay & ";
//    //    //        break;

//    //    //    case (2): //0 Moving
//    //    //        s += " Moving & ";
//    //    //        break;
//    //    //}


//    //    //switch (feb.m_downerIdx)
//    //    //{
//    //    //    case (0): //0 Damage
//    //    //        s += "Damage";
//    //    //        break;

//    //    //    case (1): //0 Seal
//    //    //        s += "Seal";
//    //    //        break;

//    //    //    case (2): //0 Slow
//    //    //        s += "Slow";
//    //    //        break;
//    //    //}

//    //    //s += " Block.a부터 b 기간 동안 활성화되는";


//    //    ////if (blocks[blocks.Count - 1])
//    //    ////{

//    //    ////}

//    //    //selectBlockInformation.text = s;

//    //    ////slotEffectExplans[feb.lineNum].text = feb.UpperSprites[feb.m_upperIdx].name + " + " + feb.DownerSprites[feb.m_downerIdx].name + " block";




















//    //    //// 만들때랑 갱신할때만 불러오게
//    //    ////FieldEffectBlock feb = blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>();
//    //    //FieldEffectBlock feb = blocks[curBlockIdx].GetComponent<FieldEffectBlock>();

//    //    //selectBlockName.text = feb.UpperSprites[feb.m_upperIdx].name + " + " + feb.DownerSprites[feb.m_downerIdx].name + " block";

//    //    //string s = "";

//    //    //switch (feb.m_upperIdx)
//    //    //{
//    //    //    case (0): //0 Blink
//    //    //        s += "a부터 b 기간 동안 활성화되는 ";
//    //    //        break;

//    //    //    case (1): //0 Delay
//    //    //        s += "a부터 b 기간 동안 활성화되며 해당 공간 방문 시 n 초 후에 효과가 발동되는 ";
//    //    //        break;

//    //    //    case (2): //0 Moving
//    //    //        s += "a부터 b 기간 동안 이동하는 ";
//    //    //        break;
//    //    //}


//    //    //switch (feb.m_downerIdx)
//    //    //{
//    //    //    case (0): //0 Damage
//    //    //        s += "m의 데미지를 주는";
//    //    //        break;

//    //    //    case (1): //0 Seal
//    //    //        s += "스킬을 n초간 봉인하는";
//    //    //        break;

//    //    //    case (2): //0 Slow
//    //    //        s += "k만큼 속도를 낮추는";
//    //    //        break;
//    //    //}

//    //    //s += " 블록이다.";


//    //    ////if (blocks[blocks.Count - 1])
//    //    ////{

//    //    ////}

//    //    //selectBlockInformation.text = s;
//    //}




//    public void SetBlocks(GameData PlayData)
//    {
//        for (int i = 0; i < PlayData.bd.Count; i++)
//        {
//            GameObject go = Instantiate(block);
//            go.transform.SetParent(rectTransform, false);
//            blocks.Add(go);
//            MapPlacementBlock feb = go.GetComponent<MapPlacementBlock>();
//            feb.Init(PlayData, blocks.Count - 1);
//        }

//        DataManager.Instance.SaveGameData();
//    }


//    public int curBlockIdx;


//    public void CreateBlock()
//    {
//        GameObject go = null;
//        go = Instantiate(block);
//        go.transform.SetParent(rectTransform, false);
//        blocks.Add(go);
//        MapPlacementBlock mpb = go.GetComponent<MapPlacementBlock>();
//        mpb.Init(null, blocks.Count - 1);
//        StartCoroutine(CoWaitForPosition(mpb));
//        mpb.changeSelected();




//        ////RectTransform rect = GetComponent<RectTransform>();

//        //if (DataManager.Instance.data.curWave < EndPhase)
//        //{
//        //    GameObject go = null;
//        //    go = Instantiate(block);
//        //    go.transform.SetParent(rectTransform, false);
//        //    blocks.Add(go);
//        //    MapPlacementBlock feb = go.GetComponent<MapPlacementBlock>();
//        //    feb.Init(null, blocks.Count - 1);
//        //    StartCoroutine(CoWaitForPosition(feb));
//        //    feb.changeSelected();
//        //}

//        //else
//        //{

//        //    MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
//        //    feb.ChangeColor(false);

//        //    curBlockIdx = UnityEngine.Random.Range(0, 9);
//        //    feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
//        //    feb.ChangeColor(true);
//        //    feb.PlusWeight();


//        //    //feb.weight += 1;

//        //    //강화버전
//        //}


//    }

//    public void RandomSelect()
//    {

//    }

//    IEnumerator CoWaitForPosition(MapPlacementBlock feb)
//    {
//        yield return new WaitForEndOfFrame();

//        //feb.SetPos(new Vector3(-1000, -1000, -1110));

//        //Vector3 newPos = new Vector3();

//        MapPlacementBlock feeb;

//        if (DataManager.Instance.data.bd.Count == 0)
//        {
//            feb.SetPos(m_timelines[0].rectTransform.position);
//        }

//        else
//        {
//            List<int> occupied = new List<int>();
//            for (int i = 0; i < DataManager.Instance.data.bd.Count; i++)
//            {
//                //feeb = DataManager.Instance.data.febs[i].GetComponent<FieldEffectBlock>();
//                occupied.Add((int)DataManager.Instance.data.bd[i].lineNum);
//            }
//            occupied.Sort();
//            int idx = 0;

//            for(int i = 0; i < occupied.Count; i++)
//            {
//                if(idx == occupied[i]) idx++;
//                else break;
//            }

//            feb.SetPos(m_timelines[idx].rectTransform.position);
//        }


//        //if (Player.instance.gold < 50)
//        //{
//        //    Reroll.interactable = false;
//        //}

//        //else
//        //{
//        //    Reroll.interactable = true;
//        //}

//    }



//    //리롤 횟수 제한 추가
//    public void RerollBlock()
//    {
//        MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
//        feb.Reroll();


//        //feb.m_upperIdx = UnityEngine.Random.Range(0, 3);
//        //feb.m_downerIdx = UnityEngine.Random.Range(0, 3);

//        //feb.UpperImage.sprite = feb.upperSprites[feb.m_upperIdx];
//        //feb.DownerImage.sprite = feb.downerSprites[feb.m_downerIdx];

//        //feb.start = UnityEngine.Random.Range(0, 10);
//        //feb.end = (feb.start + 5) % 10;
//        //feb.StartText.text = feb.start.ToString();
//        //feb.EndText.text = feb.end.ToString();





//        //if (Player.instance.gold < 50)
//        //{
//        //    Reroll.interactable = false;
//        //}


//    }



//}



////using System.Collections;
////using System.Collections.Generic;
////using UnityEngine;
////using UnityEngine.UI;
////using UnityEngine.Events;

////public class FieldEffectPopUpManager : MonoBehaviour
////{
////    public static FieldEffectPopUpManager instance { get; private set; }

////    private void Awake()
////    {
////        if (instance == null)
////        {
////            instance = this;
////            DontDestroyOnLoad(gameObject);
////        }
////        else Destroy(gameObject);
////    }

////    public List<GameObject> blocks { get; private set; }



////    public Canvas parentCanvas;


////    //private int idx;

////    public Button[] buttons;
////    public Color normalColor;
////    public Color disabledColor;

////    public GameObject block;
////    public Sprite[] idle;


////    public GridLayoutGroup slots;

////    public int EndPhase = 9;


////    //public UnityEvent selectComplete;

////    //지정된 위치 세이브 불러오기

////    void Start()
////    {
////        UIManager.instance.OnInitializeUI += CreateBlock;

////        //나중에 스테이지매니저랑 똑같이할까
////        blocks = new List<GameObject>();

////        //foreach (Button btn in buttons)
////        //{
////        //    btn.onClick.AddListener(() => OnButtonClick(btn));
////        //}





////        if(DataManager.Instance.data.curWave >=1)
////        {
////            SetBlocks();
////        }

////        CreateBlock();

////        //StageManager.instance.OnContinueWave += SetBlocks;

////    }

////    public void SetBlocks()
////    {
////        //GameObject newBlock = blocks[0];
////        //blocks = new List<GameObject>();

////        for(int i = 0; i < DataManager.Instance.data.setBlocks.Count; i++)
////        {
////            CreateBlock();


////            Image image = blocks[i].GetComponent<Image>();

////            image.sprite = idle[Function.instance.checkIdx(DataManager.Instance.data.setBlocks[i].blockName)];
////            TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
////            ttt[blocks[i].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;

////            blocks[i].name = image.sprite.name;

////            FieldEffectBlock feb = blocks[i].GetComponent<FieldEffectBlock>();
////            //feb.rectTransform.position = DataManager.Instance.data.setBlocks[i].position;


////            feb.movePos(DataManager.Instance.data.setBlocks[i].position);

////        }


////    }


////    public void CreateBlock()
////    {
////        RectTransform rect = GetComponent<RectTransform>();
////        GameObject newObject = null;
////        newObject = Instantiate(block);
////        newObject.transform.SetParent(rect.transform, false);
////        blocks.Add(newObject);

////        newObject.GetComponent<FieldEffectBlock>().idx = blocks.Count - 1;

////        newObject.GetComponent<FieldEffectBlock>().UpperIdx = 0;
////        newObject.GetComponent<FieldEffectBlock>().DownerIdx = 0;



////        foreach (Button btn in buttons)
////        {
////            SetButtonColor(btn, normalColor);
////        }
////    }



////    void OnButtonClick(Button clickedButton)
////    {
////        ChangeButtonColor(clickedButton);
////        ChangeBlockColor(clickedButton.name);
////    }

////    void ChangeButtonColor(Button clickedButton)
////    {
////        // 모든 버튼의 색상을 비활성화 색상으로 변경합니다.
////        foreach (Button btn in buttons)
////        {
////            SetButtonColor(btn, disabledColor);
////        }

////        // 클릭된 버튼의 색상을 원래 색상으로 돌립니다.
////        SetButtonColor(clickedButton, normalColor);
////    }

////    void SetButtonColor(Button button, Color color)
////    {
////        ColorBlock cb = button.colors;
////        cb.normalColor = color;
////        cb.highlightedColor = color;
////        cb.pressedColor = color;
////        cb.selectedColor = color;
////        button.colors = cb;
////    }

////    void ChangeBlockColor(string s)
////    {
////        if(blocks.Count != 0)
////        {
////            Image image = blocks[blocks.Count - 1].GetComponent<Image>();

////            image.sprite = idle[Function.instance.checkIdx(s)];


////            //switch (s)
////            //{
////            //    case "Blink":
////            //        image.sprite = idle[0];
////            //        break;
////            //    case "Shadow":
////            //        image.sprite = idle[1];
////            //        break;
////            //    case "Disable":
////            //        image.sprite = idle[2];
////            //        break;
////            //}

////            TimeLine[] ttt = slots.GetComponentsInChildren<TimeLine>();
////            ttt[blocks[blocks.Count - 1].GetComponent<FieldEffectBlock>().lineNum].blockName = image.sprite.name;

////            blocks[blocks.Count - 1].name = image.sprite.name;
////        }

////    }





////}