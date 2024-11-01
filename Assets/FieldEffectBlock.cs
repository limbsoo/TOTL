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


[SerializeField]
public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    [SerializeField]
    RectTransform rectTransform;

    [SerializeField]
    CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;

    Vector3 originPos;
    bool isBeingHeld = false;
    public bool isInLine;
    Vector3 LoadedPos = Vector3.zero;

    Vector3 rectPosition;
    //private RectTransform targetRectTransform;

    [SerializeField]
    TimeLine nearSlot;

    bool isSet;

    public int lineNum;

    public int start;
    public int end;

    public int _idx;

    //public int name;

    //TimeLine savedSlot;


    //public int slotIdx;
    //public int savedSlotIdx;


    public Sprite[] UpperSprites;
    public Sprite[] DownerSprites;


    public int m_upperIdx;
    public int m_downerIdx;

    public Image UpperImage;
    public Image DownerImage;

    public TMP_Text StartText;
    public TMP_Text EndText;



    public int weight;



    private Rigidbody2D rb2;

    private void Start()
    {
        isSet = false;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        rb2 = GetComponent<Rigidbody2D>();

        if(LoadedPos != Vector3.zero) rectTransform.position = LoadedPos;

        else
        {
            rectTransform.position = new Vector3(-1000, -1000, -1110);
        }
    }


    public void Init(GameData PlayData, int idx)
    {
        if(PlayData == null)
        {
            _idx = idx;
            m_upperIdx = UnityEngine.Random.Range(0, 3);
            m_downerIdx = UnityEngine.Random.Range(0, 3);

            start = UnityEngine.Random.Range(0, 10);
            end = (start + 5) % 10;

            //LoadedPos = new Vector3(-1000, -1000, -1110);
        }

        else
        {

            _idx = idx;
            m_upperIdx = PlayData.febs[idx].m_upperIdx;
            m_downerIdx = PlayData.febs[idx].m_downerIdx;

            start = PlayData.febs[idx].start;
            end = PlayData.febs[idx].end;

            //LoadedPos = PlayData.febs[idx].nearSlot.rectTransform.position;

            //rectTransform.position = PlayData.setBlocks[idx].position;
            LoadedPos = PlayData.febs[idx].position;
        }

        UpperImage.sprite = UpperSprites[m_upperIdx];
        DownerImage.sprite = DownerSprites[m_downerIdx];

        gameObject.name = "";
        gameObject.name += UpperImage.sprite.name;
        gameObject.name += DownerImage.sprite.name;


        StartText.text = start.ToString();
        EndText.text = end.ToString();
    }


    public Vector3 GetPos()
    {
        return rectTransform.position;
    }


    public void setRect()
    {
        LoadedPos = new Vector3(-1000, -1000, -1110);
    }



    public Image selected;

    public void changeSelected()
    {
        if (DataManager.Instance.data.curWave < FieldEffectPopUpManager.instance.EndPhase)
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<FieldEffectBlock>();
            feb.ChangeColor(false);
            feb.weight -= 1;

            ChangeColor(true);
            FieldEffectPopUpManager.instance.curBlockIdx = _idx;
            weight += 1;
        }

        else
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[FieldEffectPopUpManager.instance.curBlockIdx].GetComponent<FieldEffectBlock>();
            feb.ChangeColor(false);


            ChangeColor(true);
            FieldEffectPopUpManager.instance.curBlockIdx = _idx;
        }


    }

    public void ChangeColor(bool isTrue)
    {
        if(isTrue) 
        {
            selected.enabled = isTrue;
            //canvasGroup.blocksRaycasts = false;
        }

        else
        {
            selected.enabled = isTrue;
            //canvasGroup.blocksRaycasts = true;
        }
    }


    public void SetPos(Vector3 pos)
    {
        rectTransform.position = pos;

        //LoadedPos = pos;
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
        originPos = rectTransform.position;
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        isSet = false;
        nearSlot.haveBlock = false;
        lineNum = -1;

        if (nearSlot != null)
        {
            if (nearSlot.feb != null)
            {
                nearSlot.feb = null;
            }
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
            if (nearSlot.feb != null)
            {
                FieldEffectBlock otherBlock = nearSlot.feb;
                Vector3 tempPos = otherBlock.rectTransform.position;
                otherBlock.rectTransform.position = originPos;
                nearSlot.feb = null;

            }

            isSet = true;
            nearSlot.haveBlock = true;
            //lineNum = nearSlot.idx;
        }

        else rectTransform.position = originPos;

    }



    private void FindStartEndIdx()
    {
        for (int i = 0; i < nearSlot.length; i++)
        {
            if (nearSlot.gridCenters[i].x >= rectTransform.position.x - rectTransform.sizeDelta.x / 2)
            {
                nearSlot.startCell = i;
                break;
            }
        }

        for (int i = nearSlot.startCell; i < nearSlot.length; i++)
        {
            if (nearSlot.gridCenters[i].x <= rectTransform.position.x + rectTransform.sizeDelta.x / 2)
            {
                nearSlot.endCell = i;
            }
            else break;
        }

        lineNum = nearSlot.idx;
        nearSlot.blockName = GetComponent<Image>().sprite.name;
        nearSlot.endCell += 1;

        nearSlot.blockIdx = _idx;


        //start = nearSlot.startCell;
        //end = nearSlot.endCell;

    }



    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
    private void AlignObjectWithTimeLine()
    {
        rectTransform.position = nearSlot.rectTransform.position;


        //float difference = 999999f;
        //float savedX = 0f;

        //for (int i = 0; i < nearSlot.length; i++)
        //{
        //    if (Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x) < difference)
        //    {
        //        savedX = nearSlot.gridCenters[i].x;
        //        difference = Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x);
        //    }
        //    else break;
        //}

        //if (savedX == 0) return;

        //float startPosX = nearSlot.rectTransform.position.x - nearSlot.rectTransform.sizeDelta.x / 2;
        //float endPosX = nearSlot.rectTransform.position.x + nearSlot.rectTransform.sizeDelta.x / 2;

        //if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
        //{
        //    for (int i = 0; i < nearSlot.length; i++)
        //    {
        //        if (nearSlot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
        //        else break;
        //    }
        //}
        //else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
        //{
        //    for (int i = nearSlot.length - 1; i >= 0; i--)
        //    {
        //        if (nearSlot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
        //        else break;
        //    }
        //}

        //rectTransform.position = new Vector3(savedX, nearSlot.gridCenters[0].y, nearSlot.gridCenters[0].z);

    }

    public void SetBlockPos()
    {

    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            nearSlot = other.GetComponent<TimeLine>();

            lineNum = nearSlot.idx;
            isInLine = true;

            if(LoadedPos != Vector3.zero)
            {
                SimulateDrag(gameObject, new Vector2(LoadedPos.x, LoadedPos.y), new Vector2(LoadedPos.x, LoadedPos.y));
                LoadedPos = Vector3.zero;
            }


            if(isSet)
            {
                if(nearSlot.feb == null)
                {
                    nearSlot.feb = this;
                    AlignObjectWithTimeLine();
                    FindStartEndIdx();
                    originPos = rectTransform.position;
                }
            }



        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = false;
            //nearSlot = null;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
    }


    public void movePos(Vector3 pos)
    {
        LoadedPos = pos;
    }


}

