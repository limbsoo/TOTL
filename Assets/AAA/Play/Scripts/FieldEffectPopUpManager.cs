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
    public int EndPhase;

    public GameObject slots;






    public List<GameObject> blocks { get; private set; }



    public Canvas parentCanvas;

    public GameObject block;
    public Sprite[] idle;




    public bool isSelected;




    public List<TMP_Text> slotEffectExplans = new List<TMP_Text>();


    public UnityEngine.UI.Button Reroll;





    RectTransform rectTransform;

    public TMP_Text Cost;

    public TMP_Text GoldCnt;

    public TMP_Text SelectedBlockNaming;

    ButtonClickEvent[] _buttons;



    private void Start()
    {
        StageUI.instance.OnInitializeUI += CreateBlock;

        m_timelines = slots.GetComponentsInChildren<TimeLine>();
        rectTransform = GetComponent<RectTransform>();
        blocks = new List<GameObject>();

        SetBlocks(DataManager.Instance.saveData);

        MapPlacementBlock mpb = blocks[blocks.Count - 1].GetComponent<MapPlacementBlock>();
        mpb.changeSelected();

        RegisterButton();

        //m_buttonEvents = GetComponent<ButtonEvents>();
        //m_buttonEvents.OnCheckMoney?.Invoke(DataManager.Instance.saveData.gold);

    }

    void RegisterButton()
    {
        _buttons = GetComponentsInChildren<ButtonClickEvent>();

        if (_buttons.Length != 0)
        {
            foreach (var button in _buttons)
            {
                button.OnButtonClicked += (buttonType, name) => HandleButtonEvent(buttonType, name);
            }
        }
    }

    protected virtual void HandleButtonEvent(ButtonType buttonType, string name)
    {
        SoundManager.instance.Play("Click", SoundCatecory.Effect, false);

        switch (buttonType)
        {
            case ButtonType.Reroll:

                MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
                feb.Reroll();
                StageManager.instance.UseGold(StageManager.instance.ReturnLevelSet().InitrerollCost);

                if (StageManager.instance.IsUnderCost(StageManager.instance.ReturnLevelSet().InitrerollCost))
                {
                    Reroll.interactable = false;
                }
                break;

        }

    }


    public TMP_Text slot1;
    public TMP_Text selectBlockName;
    public TMP_Text selectBlockInformation;
    public TMP_Text selectBlockWeight;

    //public UnityEngine.UI.Image SelectBlockImage;

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

        LevelConstructSet levelConstructSet = StageManager.instance.ReturnLevelSet();

        Cost.text = levelConstructSet.InitrerollCost.ToString();
        GoldCnt.text = StageManager.instance.RetunrGold().ToString();

        for (int i = 0; i < slotEffectExplans.Count; i++)
        {
            slotEffectExplans[i].text = "";
        }


        for (int i = 0; i < blocks.Count; i++)
        {
            bd = blocks[i].GetComponent<MapPlacementBlock>().GetBlockData();

            slotEffectExplans[bd.lineNum].text =

                string.Format("{2} & {3}   ({0} ~ {1})",
                //string.Format("{0} ~ {1}�� �� {2} & {3} ȿ�� �߻� ����",
                bd.start.ToString(), 
                bd.end.ToString(), 
                bd.effectKinds.ToString(), 
                bd.fieldKinds.ToString());
        }

        bd = blocks[curBlockIdx].GetComponent<MapPlacementBlock>().GetBlockData();

        //selectBlockInformation.text = string.Format("{0} ~ {1}�� �� {2} ȿ���� ���� {3} �ʵ� ȿ�� �߻�.",
        //        bd.start.ToString(),
        //        bd.end.ToString(),
        //        bd.effectKinds.ToString(),
        //        bd.fieldKinds.ToString()); ;


        selectBlockInformation.text = string.Format("�� ����� {0}�ʺ��� {1}�ʱ��� Ȱ��ȭ�˴ϴ�.\n", bd.start.ToString(), bd.end.ToString());
        selectBlockInformation.text += "\n";

        string effectInfo = "";

        switch (bd.fieldKinds)
        {
            case FieldKinds.Long:
                selectBlockInformation.text += "�� ������ �÷��̾ �����ϸ� ��� ȿ���� �޽��ϴ�.\n";

                break;
            case FieldKinds.Stack:
                selectBlockInformation.text += 
                    string.Format("�÷��̾ �� ������ {0}�� �̻� �ӹ��� ��� ȿ���� �߻��մϴ�.\n",
                    bd.fieldValue.ToString());

                break;
            case FieldKinds.Move:
                selectBlockInformation.text += "�� ������ ���� �ӵ��� �̵��ϸ�, �÷��̾ ���� �� ȿ���� �޽��ϴ�.\n";
                break;
        }


        selectBlockInformation.text += "\n";

        string effectValue = "";

        float effectValuue = bd.effectValue;

        if (bd.fieldKinds == FieldKinds.Stack) { effectValuue *= bd.fieldValue; }


        if (bd.weight > 0) { effectValue += SetTextColor("red", effectValuue + bd.weight); }
        else { effectValue += (effectValuue + bd.weight).ToString(); }


        

        switch (bd.effectKinds)
        {
            case EffectKinds.Damage:
                selectBlockInformation.text +=
                    string.Format("ȿ�� �߻� �� {0} �������� �޽��ϴ�.\n",
                    effectValue);
                break;
            case EffectKinds.Slow:
                selectBlockInformation.text +=
                    string.Format("ȿ�� �߻� �� {0}�� ���� �̵� �ӵ��� �������� �����մϴ�.\n",
                    effectValue);
                break;
            case EffectKinds.Seal:
                selectBlockInformation.text +=
                    string.Format("ȿ�� �߻� �� {0}�� ���� ��ų�� ���ε˴ϴ�.\n",
                    effectValue);

                break;
        }

        //selectBlockInformation.text = s;

        selectBlockWeight.text = bd.weight.ToString();


        //s = "";

        //s += string.Format("{0} + {1} Block", bd.fieldKinds.ToString(), bd.effectKinds.ToString());

        SelectedBlockNaming.text = string.Format("{0} + {1}", bd.fieldKinds.ToString(), bd.effectKinds.ToString());

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

    //    LevelConstructSet levelConstructSet = StageManager.instance.ReturnLevelSet();

    //    Cost.text = levelConstructSet.InitrerollCost.ToString();
    //    GoldCnt.text = StageManager.instance.RetunrGold().ToString();

    //    for (int i = 0; i < slotEffectExplans.Count; i++)
    //    {
    //        slotEffectExplans[i].text = "";
    //    }


    //    for (int i = 0; i < blocks.Count; i++)
    //    {
    //        s = "";

    //        bd = blocks[i].GetComponent<MapPlacementBlock>().GetBlockData();

    //        s += bd.start.ToString() + " - ";
    //        s += bd.end.ToString();

    //        //if (bd.weightKinds == WeightKinds.ActiveTime)
    //        //{
    //        //    s += SetTextColor("red", bd.end + bd.weight);
    //        //}

    //        //else { s += bd.end.ToString(); }

    //        s += " �� ����  ";
    //        s += bd.fieldKinds.ToString() + " Field ";
    //        s += bd.effectKinds.ToString() + " Effect ";

    //        slotEffectExplans[bd.lineNum].text = s;
    //    }

    //    //���� 



    //    bd = blocks[curBlockIdx].GetComponent<MapPlacementBlock>().GetBlockData();
    //    s = "";

    //    //s += "�ʵ� �ð� " + bd.start.ToString() + "�� ���� ";
    //    //s += bd.end.ToString() + "�� ���� ";



    //    s += string.Format("�ʵ� �ð� {0}�� ���� {1}�� ����", bd.start.ToString(), bd.end.ToString());



    //    //s +=  bd.fieldDuration.ToString() /*+ " +(" + bd.weight.ToString() + ")"*/ + " �� ����  ";

    //    //if (bd.weightKinds == WeightKinds.ActiveTime)
    //    //{
    //    //    s += SetTextColor("red", bd.end + bd.weight);
    //    //}

    //    //else { s += bd.end.ToString(); }


    //    switch (bd.fieldKinds)
    //    {
    //        case FieldKinds.Long:
    //            break;
    //        case FieldKinds.Stack:
    //            s += "�÷��̾ �ش籸���� �����Ͽ� ";
    //            s += bd.fieldValue.ToString() + " �� �̻� �ӹ��� ��� " /*+ bd.fieldValue.ToString() + " * "*/;

    //            break;
    //        case FieldKinds.Move:
    //            s += "������ �ӵ��� �����̸� " + "�÷��̾ �ش籸�� ���� �� ";
    //            break;
    //    }


    //    s += (bd.effectValue + bd.weight).ToString();

    //    switch (bd.effectKinds)
    //    {
    //        case EffectKinds.Damage:
    //            //s += bd.effectValue.ToString();
    //            //s += " +(" + bd.weight.ToString() + ")";
    //            s += " �� �������� �޴´�.";
    //            break;
    //        case EffectKinds.Slow:
    //            //s += bd.effectValue.ToString();
    //            //s += " +(" + bd.weight.ToString() + ")";
    //            s += " �� ���� �̵��ӵ��� �����Ѵ�.";
    //            break;
    //        case EffectKinds.Seal:
    //            //s += bd.effectValue.ToString();
    //            //s += " +(" + bd.weight.ToString() + ")";
    //            s += " �� ���� ��ų�� ���εȴ�.";

    //            break;
    //    }

    //    selectBlockInformation.text = s;

    //    selectBlockWeight.text = bd.weight.ToString();


    //    //s = "";

    //    //s += string.Format("{0} + {1} Block", bd.fieldKinds.ToString(), bd.effectKinds.ToString());

    //    SelectedBlockNaming.text = string.Format("{0} + {1}", bd.fieldKinds.ToString(), bd.effectKinds.ToString());

    //}


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

    //        s += "<color=red>" + bd.start.ToString() + " - " + bd.end.ToString() + "</color>" + " �� ����  ";
    //        s += "<color=yellow>" + bd.fieldKinds.ToString() + "</color>" + " Field " ;
    //        s += "<color=green>" + bd.effectKinds.ToString()  + "</color>" + " Effect ";

    //        slotEffectExplans[bd.lineNum].text = s;
    //    }

    //    bd = blocks[curBlockIdx].GetComponent<MapPlacementBlock>().GetBlockData();
    //    s = "";

    //    s += "�ʵ� �ð� " +  "<color=red>" + bd.start.ToString() + "</color>" + " �ʿ� Ȱ��ȭ�Ǿ� ";
    //    s += "<color=red>" +  bd.fieldDuration.ToString() + " +(" + bd.weight.ToString() + ")" + "</color>" + " �� ����  "; //"�� �÷��̾ �ش籸�� ���� �� ";

    //    switch (bd.fieldKinds)
    //    {
    //        case FieldKinds.Long:


    //            break;
    //        case FieldKinds.Stack:
    //            s += "�÷��̾ �ش籸���� �����Ͽ� ";
    //            s += bd.fieldDuration.ToString() + " �� �̻� �ӹ��� ��� " + bd.fieldValue.ToString() + " * ";

    //            break;
    //        case FieldKinds.Move:
    //            s += "������ �ӵ��� �����̸� " + "�÷��̾ �ش籸�� ���� �� ";
    //            break;
    //    }

    //    s += "<color=green>";


    //    switch (bd.effectKinds)
    //    {
    //        case EffectKinds.Damage:
    //            s += bd.effectValue.ToString() + "</color>" + " �� �������� �޴´�.";

    //            break;
    //        case EffectKinds.Slow:
    //            s += bd.effectValue.ToString() + "</color>" + " �� ���� �̵��ӵ��� �����Ѵ�.";

    //            break;
    //        case EffectKinds.Seal:
    //            s += bd.effectValue.ToString() + "</color>" + " �� ���� ��ų�� ���εȴ�.";
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
        if (!StageManager.instance.IsUnderCost(StageManager.instance.ReturnLevelSet().InitrerollCost))
        {
            Reroll.interactable = true;
        }



        if (PlayData.blockdata.Count >= 1)
        {
            for (int i = 0; i < PlayData.blockdata.Count; i++)
            {
                GameObject go = Instantiate(block);
                go.transform.SetParent(rectTransform, false);
                blocks.Add(go);
                MapPlacementBlock feb = go.GetComponent<MapPlacementBlock>();
                //feb.Init(PlayData, blocks.Count - 1);

                feb.Init(PlayData, blocks.Count - 1, PlayData.blockdata[i].lineNum, m_timelines[PlayData.blockdata[i].lineNum].rectTransform.position);

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

        if(StageManager.instance.ReturnLevelSet().endArrage >= StageManager.instance.GetCurWave())
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



    ////���� Ƚ�� ���� �߰�
    //public void RerollBlock()
    //{
    //    MapPlacementBlock feb = blocks[curBlockIdx].GetComponent<MapPlacementBlock>();
    //    feb.Reroll();
    //    StageManager.instance.UseGold(StageManager.instance.ReturnLevelSet().InitrerollCost);

    //    if(StageManager.instance.IsUnderCost(StageManager.instance.ReturnLevelSet().InitrerollCost))
    //    {
    //        Reroll.interactable = false;
    //    }

    //    //if (Player.instance.gold < 50)
    //    //{
    //    //    Reroll.interactable = false;
    //    //}

    //}
}



