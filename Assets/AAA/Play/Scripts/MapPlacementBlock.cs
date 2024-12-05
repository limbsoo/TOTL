using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;


[SerializeField]
public class MapPlacementBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;

    //Vector3 originPos;
    int originIdx;

    bool isBeingHeld = false;
    public bool isInLine;
    Vector3 LoadedPos = Vector3.zero;

    Vector3 rectPosition;
    //private RectTransform targetRectTransform;

    [SerializeField]
    TimeLine nearSlot;

    bool isSet;

    public int lineNum;



    public int m_blockIdx;

    //public int name;

    //TimeLine savedSlot;


    //public int slotIdx;
    //public int savedSlotIdx;



    bool isFirst;


    //Sprite[] upperSprites;
    //Sprite[] downerSprites;


    //public int m_upperIdx;
    //public int m_downerIdx;

    public Image UpperImage;
    public Image DownerImage;

    public TMP_Text StartText;
    public TMP_Text EndText;


    [SerializeField] BlockData blockdata;

    //public WeightKinds weightKinds;


    //[SerializeField] FieldKinds fieldKinds;
    //[SerializeField] EffectKinds effectKinds;
    //[SerializeField] WeightKinds weightKinds;

    //int start;
    //int end;
    //int weight;
    //[SerializeField] int value;
    //[SerializeField] int duration;

    //private Rigidbody2D rb2;

    private void Start()
    {
        isSet = false;
        canvasGroup = GetComponent<CanvasGroup>();
        //originIdx = nearSlot.idx;
    }



    public int GetFieldKinds()
    {
        return (int)blockdata.fieldKinds;
    }


    public BlockData GetBlockData()
    {
        return blockdata;
    }


    public void Reroll()
    {
        RandomLoadFieldEffectBlock();
        SetBlockImage();
    }




    public void RandomLoadFieldEffectBlock()
    {
        int randNum = UnityEngine.Random.Range(1, StageManager.instance.ReturnLevelSet().FieldBlock.Count + 1);
        blockdata.fieldKinds = (FieldKinds)Enum.GetValues(typeof(FieldKinds)).GetValue(randNum);

        randNum = UnityEngine.Random.Range(1, StageManager.instance.ReturnLevelSet().EffectBlock.Count + 1);
        blockdata.effectKinds = (EffectKinds)Enum.GetValues(typeof(EffectKinds)).GetValue(randNum);

        //blockdata.weightKinds = WeightKinds.None;

        //BlockInfo blockinfo;

        FieldEffectBlock fieldBlock = StageManager.instance.ReturnLevelSet().FieldBlock[(int)blockdata.fieldKinds - 1].GetComponent<FieldEffectBlock>();
        FieldEffectBlock effectBlock = StageManager.instance.ReturnLevelSet().EffectBlock[(int)blockdata.effectKinds - 1].GetComponent<FieldEffectBlock>();

        blockdata.fieldValue = fieldBlock.blockInfo.value;
        blockdata.fieldDuration = fieldBlock.blockInfo.duration;
        blockdata.effectValue = effectBlock.blockInfo.value;
        blockdata.effectDuration = fieldBlock.blockInfo.duration;

        blockdata.start = UnityEngine.Random.Range(0, 10);
        blockdata.end = (blockdata.start + blockdata.fieldDuration) % 10;
    }

    public void SetBlockImage()
    {
        UpperImage.sprite = StageManager.instance.ReturnLevelSet().FieldBlock[(int)blockdata.fieldKinds - 1].GetComponent<SpriteRenderer>().sprite;
        DownerImage.sprite = StageManager.instance.ReturnLevelSet().EffectBlock[(int)blockdata.effectKinds - 1].GetComponent<SpriteRenderer>().sprite;

        gameObject.name = "";
        gameObject.name += UpperImage.sprite.name;
        gameObject.name += DownerImage.sprite.name;

        StartText.text = blockdata.start.ToString();
        EndText.text = blockdata.end.ToString();
    }


    public void Init(GameData PlayData, int idx, int lineNum, Vector3 pos)
    {
        m_blockIdx = idx;
        blockdata = new BlockData();

        if (PlayData == null) { RandomLoadFieldEffectBlock(); }

        else
        {

            blockdata = PlayData.blockdata[idx];
            //LoadedPos = PlayData.bd[idx].position;

        }


        SetBlockImage();

        //rectPosition = blockdata.position;

        rectTransform = GetComponent<RectTransform>();
        rectTransform.position = pos;

        blockdata.lineNum = lineNum;
        blockdata.position = rectTransform.position;

        isSet = false;

        isFirst = false;

    }

    public void setWeight()
    {
        switch (blockdata.weightKinds)
        {
            case WeightKinds.ActiveTime:
                blockdata.fieldDuration += blockdata.weight;

                blockdata.end += 1;
                if (blockdata.end == 10) { blockdata.end = 0; }

                break;
            case WeightKinds.Value:
                blockdata.effectValue += blockdata.weight;


                break;
        }
    }




    public Image selected;



    public void changeSelectedAndWeight()
    {
        MapPlacementBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<MapPlacementBlock>();
        feb.ChangeColor(false);
        ChangeColor(true);
        FieldEffectPopUpManager.instance.curBlockIdx = m_blockIdx;

        //int randNum = UnityEngine.Random.Range(1, 2);
        //blockdata.weightKinds = (WeightKinds)randNum;

        blockdata.weightKinds = WeightKinds.Value;
        blockdata.weight += 1;
    }













    public void changeSelected()
    {
        MapPlacementBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<MapPlacementBlock>();
        feb.ChangeColor(false);
        ChangeColor(true);
        FieldEffectPopUpManager.instance.curBlockIdx = m_blockIdx;
    }

    public void ChangeColor(bool isTrue)
    {
        selected.enabled = isTrue;
    }


    public void SimulateDrag(GameObject target, Vector2 start, Vector2 end)
    {
        // PointerEventData 객체 생성
        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            position = start, // 드래그 시작 위치
            delta = end - start // 드래그 이동량
        };

        // 드래그 시작 (OnBeginDrag)
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.beginDragHandler);

        // 드래그 중 (OnDrag)
        pointer.position = end; // 드래그 중 위치 업데이트
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.dragHandler);

        // 드래그 끝 (OnEndDrag)
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.endDragHandler);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        isBeingHeld = true;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        originIdx = nearSlot.idx;

        //nearSlot.feb = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isBeingHeld = true;
        rectTransform.anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isBeingHeld = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        if (isInLine)
        {
            if(nearSlot.feb != null)
            {
                nearSlot.feb.rectTransform.position = FieldEffectPopUpManager.instance.m_timelines[originIdx].rectTransform.position;
                nearSlot.feb.isSet = false;
            }

            rectTransform.position = nearSlot.rectTransform.position;
            isSet = false;
        }

        else
        {
            rectTransform.position = FieldEffectPopUpManager.instance.m_timelines[originIdx].rectTransform.position;
        }

    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = true;
            nearSlot = other.GetComponent<TimeLine>();

            if (!isSet)
            {
                isSet = true;
                nearSlot.feb = this;

                blockdata.lineNum = nearSlot.idx;
                blockdata.position = nearSlot.rectTransform.position;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine")) 
        {
            other.GetComponent<TimeLine>().feb = null;
            isInLine = false; 
            
        }
    }

















    public void SimulateClick(GameObject target)
    {
        // PointerEventData 객체 생성 (마우스 클릭 이벤트 데이터)
        PointerEventData pointer = new PointerEventData(EventSystem.current)
        {
            // 포인터의 위치나 다른 정보를 설정할 수 있음 (필요 시)
            position = new Vector2(target.transform.position.x, target.transform.position.y)
        };

        // 클릭 이벤트 전달
        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerClickHandler);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
    }



}





































//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
//using Unity.VisualScripting;
//using Unity.VisualScripting.Antlr3.Runtime.Misc;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;
//using static UnityEngine.GraphicsBuffer;
//using static UnityEngine.Rendering.DebugUI;


//[SerializeField]
//public class MapPlacementBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
//{
//    [SerializeField]
//    RectTransform rectTransform;

//    [SerializeField]
//    CanvasGroup canvasGroup;
//    [SerializeField] Canvas canvas;

//    //Vector3 originPos;
//    int originIdx;

//    bool isBeingHeld = false;
//    public bool isInLine;
//    Vector3 LoadedPos = Vector3.zero;

//    Vector3 rectPosition;
//    //private RectTransform targetRectTransform;

//    [SerializeField]
//    TimeLine nearSlot;

//    bool isSet;

//    public int lineNum;



//    public int m_blockIdx;

//    //public int name;

//    //TimeLine savedSlot;


//    //public int slotIdx;
//    //public int savedSlotIdx;



//    bool isFirst;


//    //Sprite[] upperSprites;
//    //Sprite[] downerSprites;


//    //public int m_upperIdx;
//    //public int m_downerIdx;

//    public Image UpperImage;
//    public Image DownerImage;

//    public TMP_Text StartText;
//    public TMP_Text EndText;


//    [SerializeField] BlockData blockdata;




//    //[SerializeField] FieldKinds fieldKinds;
//    //[SerializeField] EffectKinds effectKinds;
//    //[SerializeField] WeightKinds weightKinds;

//    //int start;
//    //int end;
//    //int weight;
//    //[SerializeField] int value;
//    //[SerializeField] int duration;

//    //private Rigidbody2D rb2;

//    private void Start()
//    {
//        isSet = false;
//        //rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();




//        //rb2 = GetComponent<Rigidbody2D>();

//        //if(LoadedPos != Vector3.zero) rectTransform.position = LoadedPos;

//        //else
//        //{
//        //    rectTransform.position = new Vector3(-1000, -1000, -1110);
//        //}
//    }

//    //public void PlusWeight()
//    //{
//    //    weight += 1;
//    //}

//    public int GetFieldKinds()
//    {
//        return (int) blockdata.fieldKinds;
//    }


//    //public BlockData GetBlockData()
//    //{
//    //    BlockData blockData = new BlockData();

//    //    blockData.fieldKinds = fieldKinds;
//    //    blockData.effectKinds = effectKinds;
//    //    blockData.weightKinds = weightKinds;
//    //    blockData.start = start;
//    //    blockData.end = end;
//    //    blockData.weight = weight;
//    //    blockData.value = value;
//    //    blockData.duration = duration;
//    //    blockData.lineNum = lineNum;
//    //    blockData.position = rectTransform.position;

//    //    return blockData;
//    //}

//    public BlockData GetBlockData()
//    {
//        return blockdata;
//    }


//    public void Reroll()
//    {
//        RandomLoadFieldEffectBlock();
//        SetBlockImage();

//        ////weigth도 넣어야하는디

//        //fieldKinds = (FieldKinds)Enum.GetValues(typeof(FieldKinds)).GetValue(UnityEngine.Random.Range(0, StageManager.instance.LCS.UpperSprites.Length));
//        //effectKinds = (EffectKinds)Enum.GetValues(typeof(EffectKinds)).GetValue(UnityEngine.Random.Range(0, StageManager.instance.LCS.DownerSprites.Length));
//        //weightKinds = WeightKinds.None;

//        //FieldEffect fe = StageManager.instance.LCS.FieldEffect[(int)fieldKinds].GetComponent<FieldEffect>();


//        //start = UnityEngine.Random.Range(0, 10);
//        //end = (start + duration) % 10;
//        ////weight = 0;

//        //value = fe.GetValue();
//        //duration = fe.GetDuration();

//        //StartText.text = start.ToString();
//        //EndText.text = end.ToString();
//    }




//    public void RandomLoadFieldEffectBlock()
//    {
//        int randNum = UnityEngine.Random.Range(1, StageManager.instance.LCS.FieldBlock.Count + 1);
//        blockdata.fieldKinds = (FieldKinds)Enum.GetValues(typeof(FieldKinds)).GetValue(randNum);

//        randNum = UnityEngine.Random.Range(1, StageManager.instance.LCS.EffectBlock.Count + 1);
//        blockdata.effectKinds = (EffectKinds)Enum.GetValues(typeof(EffectKinds)).GetValue(randNum);

//        blockdata.weightKinds = WeightKinds.None;

//        //BlockInfo blockinfo;

//        FieldEffectBlock fieldBlock = StageManager.instance.LCS.FieldBlock[(int)blockdata.fieldKinds - 1].GetComponent<FieldEffectBlock>();
//        FieldEffectBlock effectBlock = StageManager.instance.LCS.EffectBlock[(int)blockdata.effectKinds - 1].GetComponent<FieldEffectBlock>();

//        blockdata.fieldValue = fieldBlock.blockInfo.value;
//        blockdata.fieldDuration = fieldBlock.blockInfo.duration;
//        blockdata.effectValue = effectBlock.blockInfo.value;
//        blockdata.effectDuration = fieldBlock.blockInfo.duration;

//        blockdata.start = UnityEngine.Random.Range(0, 10);
//        blockdata.end = (blockdata.start + blockdata.fieldDuration) % 10;
//    }

//    public void SetBlockImage()
//    {
//        UpperImage.sprite = StageManager.instance.LCS.FieldBlock[(int)blockdata.fieldKinds - 1].GetComponent<SpriteRenderer>().sprite;
//        DownerImage.sprite = StageManager.instance.LCS.EffectBlock[(int)blockdata.effectKinds - 1].GetComponent<SpriteRenderer>().sprite;

//        gameObject.name = "";
//        gameObject.name += UpperImage.sprite.name;
//        gameObject.name += DownerImage.sprite.name;

//        StartText.text = blockdata.start.ToString();
//        EndText.text = blockdata.end.ToString();
//    }


//    public void Init(GameData PlayData, int idx, int lineNum, Vector3 pos)
//    {
//        m_blockIdx = idx;
//        blockdata = new BlockData();

//        if (PlayData == null) { RandomLoadFieldEffectBlock(); }

//        else
//        {

//            blockdata = PlayData.bd[idx];
//            //LoadedPos = PlayData.bd[idx].position;

//        }


//        SetBlockImage();

//        //rectPosition = blockdata.position;

//        rectTransform = GetComponent<RectTransform>();
//        rectTransform.position = pos;

//        blockdata.lineNum = lineNum;
//        blockdata.position = rectTransform.position;

//        isFirst = false;

//        //rectPosition = pos;
//    }


//    public void Init(GameData PlayData, int idx)
//    {
//        m_blockIdx = idx;
//        blockdata = new BlockData();

//        if (PlayData == null) { RandomLoadFieldEffectBlock(); }

//        else 
//        {

//            blockdata = PlayData.bd[idx];
//            //LoadedPos = PlayData.bd[idx].position;

//        }


//        SetBlockImage();

//        rectPosition = blockdata.position;



//        //if(PlayData == null)
//        //{
//        //    m_idx = idx;

//        //    fieldKinds = (FieldKinds)Enum.GetValues(typeof(FieldKinds)).GetValue(UnityEngine.Random.Range(0, StageManager.instance.LCS.UpperSprites.Length));
//        //    effectKinds = (EffectKinds)Enum.GetValues(typeof(EffectKinds)).GetValue(UnityEngine.Random.Range(0, StageManager.instance.LCS.DownerSprites.Length));
//        //    weightKinds = WeightKinds.None;

//        //    FieldEffect fe = StageManager.instance.LCS.FieldEffect[(int)fieldKinds].GetComponent<FieldEffect>();


//        //    start = UnityEngine.Random.Range(0, 10);
//        //    end = (start + duration) % 10;
//        //    weight = 0;

//        //    value = fe.GetValue();
//        //    duration = fe.GetDuration();
//        //}

//        //else
//        //{
//        //    m_idx = idx;

//        //    fieldKinds = PlayData.febs[idx].fieldKinds;
//        //    effectKinds = PlayData.febs[idx].effectKinds;


//        //    value = PlayData.febs[idx].value;
//        //    duration = PlayData.febs[idx].duration;
//        //    start = PlayData.febs[idx].start;
//        //    end = PlayData.febs[idx].end;
//        //    weightKinds = PlayData.febs[idx].weightKinds;

//        //    LoadedPos = PlayData.febs[idx].position;

//        //    //m_upperIdx = PlayData.febs[idx].m_upperIdx;
//        //    //m_downerIdx = PlayData.febs[idx].m_downerIdx;
//        //    //LoadedPos = PlayData.febs[idx].nearSlot.rectTransform.position;
//        //    //rectTransform.position = PlayData.setBlocks[idx].position;

//        //}



//        //UpperImage.sprite = StageManager.instance.LCS.UpperSprites[(int) fieldKinds];
//        //DownerImage.sprite = StageManager.instance.LCS.DownerSprites[(int)effectKinds];


//        ////UpperImage.sprite = upperSprites[m_upperIdx];
//        ////DownerImage.sprite = downerSprites[m_downerIdx];

//        //gameObject.name = "";
//        //gameObject.name += UpperImage.sprite.name;
//        //gameObject.name += DownerImage.sprite.name;


//        //StartText.text = start.ToString();
//        //EndText.text = end.ToString();
//    }


//    public Vector3 GetPos()
//    {
//        return rectTransform.position;
//    }


//    public void setRect()
//    {
//        LoadedPos = new Vector3(-1000, -1000, -1110);
//    }



//    public Image selected;

//    public void changeSelected()
//    {
//        if (DataManager.Instance.data.curWave < FieldEffectPopUpManager.instance.EndPhase)
//        {
//            MapPlacementBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<MapPlacementBlock>();
//            feb.ChangeColor(false);
//            //feb.weight -= 1;

//            ChangeColor(true);
//            FieldEffectPopUpManager.instance.curBlockIdx = m_blockIdx;
//            //weight += 1;
//        }

//        else
//        {
//            MapPlacementBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<MapPlacementBlock>();
//            feb.ChangeColor(false);


//            ChangeColor(true);
//            FieldEffectPopUpManager.instance.curBlockIdx = m_blockIdx;
//        }


//    }

//    public void ChangeColor(bool isTrue)
//    {
//        selected.enabled = isTrue;
//    }


//    public void SetPos(Vector3 pos)
//    {
//        rectTransform.position = pos;

//        //LoadedPos = pos;
//    }


//    public void SimulateDrag(GameObject target, Vector2 start, Vector2 end)
//    {
//        // PointerEventData 객체 생성
//        PointerEventData pointer = new PointerEventData(EventSystem.current)
//        {
//            position = start, // 드래그 시작 위치
//            delta = end - start // 드래그 이동량
//        };

//        // 드래그 시작 (OnBeginDrag)
//        ExecuteEvents.Execute(target, pointer, ExecuteEvents.beginDragHandler);

//        // 드래그 중 (OnDrag)
//        pointer.position = end; // 드래그 중 위치 업데이트
//        ExecuteEvents.Execute(target, pointer, ExecuteEvents.dragHandler);

//        // 드래그 끝 (OnEndDrag)
//        ExecuteEvents.Execute(target, pointer, ExecuteEvents.endDragHandler);
//    }


//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        //originPos = rectTransform.position;
//        originIdx = lineNum;


//        canvasGroup.alpha = .6f;
//        canvasGroup.blocksRaycasts = false;

//        isSet = false;
//        nearSlot.haveBlock = false;
//        lineNum = -1;

//        if (nearSlot != null)
//        {
//            if (nearSlot.feb != null)
//            {
//                nearSlot.feb = null;
//            }
//        }

//    }

//    public void SimulateClick(GameObject target)
//    {
//        // PointerEventData 객체 생성 (마우스 클릭 이벤트 데이터)
//        PointerEventData pointer = new PointerEventData(EventSystem.current)
//        {
//            // 포인터의 위치나 다른 정보를 설정할 수 있음 (필요 시)
//            position = new Vector2(target.transform.position.x, target.transform.position.y)
//        };

//        // 클릭 이벤트 전달
//        ExecuteEvents.Execute(target, pointer, ExecuteEvents.pointerClickHandler);
//    }



//    public void OnDrag(PointerEventData eventData)
//    {
//        isBeingHeld = true;
//        rectTransform.anchoredPosition += eventData.delta;

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        isBeingHeld = false;
//        canvasGroup.alpha = 1f;
//        canvasGroup.blocksRaycasts = true;

//        if (isInLine)
//        {
//            if (nearSlot.feb != null)
//            {
//                MapPlacementBlock otherBlock = nearSlot.feb;
//                otherBlock.rectTransform.position = FieldEffectPopUpManager.instance.m_timelines[originIdx].rectTransform.position;
//                nearSlot.feb = null;




//                //MapPlacementBlock otherBlock = nearSlot.feb;
//                //Vector3 tempPos = otherBlock.rectTransform.position;
//                //otherBlock.rectTransform.position = originPos;
//                //nearSlot.feb = null;

//            }

//            isSet = true;
//            nearSlot.haveBlock = true;
//            //lineNum = nearSlot.idx;
//        }

//        else rectTransform.position = FieldEffectPopUpManager.instance.m_timelines[originIdx].rectTransform.position;

//    }



//    private void FindStartEndIdx()
//    {
//        for (int i = 0; i < nearSlot.length; i++)
//        {
//            if (nearSlot.gridCenters[i].x >= rectTransform.position.x - rectTransform.sizeDelta.x / 2)
//            {
//                nearSlot.startCell = i;
//                break;
//            }
//        }

//        for (int i = nearSlot.startCell; i < nearSlot.length; i++)
//        {
//            if (nearSlot.gridCenters[i].x <= rectTransform.position.x + rectTransform.sizeDelta.x / 2)
//            {
//                nearSlot.endCell = i;
//            }
//            else break;
//        }

//        lineNum = nearSlot.idx;
//        nearSlot.blockName = GetComponent<Image>().sprite.name;
//        nearSlot.endCell += 1;

//        nearSlot.blockIdx = m_blockIdx;


//        //start = nearSlot.startCell;
//        //end = nearSlot.endCell;

//    }



//    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
//    private void AlignObjectWithTimeLine()
//    {
//        rectTransform.position = nearSlot.rectTransform.position;


//        //float difference = 999999f;
//        //float savedX = 0f;

//        //for (int i = 0; i < nearSlot.length; i++)
//        //{
//        //    if (Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x) < difference)
//        //    {
//        //        savedX = nearSlot.gridCenters[i].x;
//        //        difference = Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x);
//        //    }
//        //    else break;
//        //}

//        //if (savedX == 0) return;

//        //float startPosX = nearSlot.rectTransform.position.x - nearSlot.rectTransform.sizeDelta.x / 2;
//        //float endPosX = nearSlot.rectTransform.position.x + nearSlot.rectTransform.sizeDelta.x / 2;

//        //if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
//        //{
//        //    for (int i = 0; i < nearSlot.length; i++)
//        //    {
//        //        if (nearSlot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//        //        else break;
//        //    }
//        //}
//        //else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
//        //{
//        //    for (int i = nearSlot.length - 1; i >= 0; i--)
//        //    {
//        //        if (nearSlot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//        //        else break;
//        //    }
//        //}

//        //rectTransform.position = new Vector3(savedX, nearSlot.gridCenters[0].y, nearSlot.gridCenters[0].z);

//    }

//    public void SetBlockPos()
//    {

//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            nearSlot = other.GetComponent<TimeLine>();

//            lineNum = nearSlot.idx;
//            isInLine = true;

//            if(!isFirst)
//            {

//                SimulateDrag(gameObject, new Vector2(rectTransform.position.x, rectTransform.position.y), new Vector2(rectTransform.position.x, rectTransform.position.y));
//                isFirst = true;
//            }

//            //if(LoadedPos != Vector3.zero)
//            //{
//            //    SimulateDrag(gameObject, new Vector2(LoadedPos.x, LoadedPos.y), new Vector2(LoadedPos.x, LoadedPos.y));
//            //    LoadedPos = Vector3.zero;
//            //}


//            if (isSet)
//            {
//                if(nearSlot.feb == null)
//                {
//                    nearSlot.feb = this;
//                    AlignObjectWithTimeLine();
//                    FindStartEndIdx();

//                    originIdx = nearSlot.idx;

//                    //FieldEffectPopUpManager.instance.m_timelines[originIdx].rectTransform.position

//                    //originPos = rectTransform.position;
//                }
//            }



//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = false;
//            //nearSlot = null;
//        }
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//    }


//    public void movePos(Vector3 pos)
//    {
//        LoadedPos = pos;
//    }


//}

