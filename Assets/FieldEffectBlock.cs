using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    [SerializeField] Canvas canvas;

    Vector3 originPos;
    bool isBeingHeld = false;
    public bool isInLine;
    Vector3 LoadedPos;

    Vector3 rectPosition;
    private RectTransform targetRectTransform;

    TimeLine slot;
    int slotIdx = -1;

    bool isSet = false;

    public int lineNum;

    public int start;
    public int end;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originPos = rectTransform.position;

        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        if (slot != null)
        {
            if (slot.haveBlock) slot.haveBlock = false;
        }
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

        // 드래그 종료 시 조건 검사
        if (isInLine && !slot.haveBlock)
        {
            // tBlock에 오브젝트가 없으면 위치를 정렬
            AlignObjectWithTimeLine();
            slot.haveBlock = true;
            isSet = true;

            originPos = rectTransform.position;
        }

        else rectTransform.position = originPos;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
    }


    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
    private void AlignObjectWithTimeLine()
    {
        float difference = 999999f;
        float savedX = 0f;

        for (int i = 0; i < slot.length; i++)
        {
            if (Math.Abs(slot.gridCenters[i].x - rectTransform.position.x) < difference)
            {
                savedX = slot.gridCenters[i].x;
                difference = Math.Abs(slot.gridCenters[i].x - rectTransform.position.x);
            }
            else break;
        }

        if (savedX == 0) return;

        float startPosX = slot.rectTransform.position.x - slot.rectTransform.sizeDelta.x / 2;
        float endPosX = slot.rectTransform.position.x + slot.rectTransform.sizeDelta.x / 2;

        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
        {
            for (int i = 0; i < slot.length; i++)
            {
                if (slot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = slot.gridCenters[i].x;
                else break;
            }
        }
        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
        {
            for (int i = slot.length - 1; i >= 0; i--)
            {
                if (slot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = slot.gridCenters[i].x;
                else break;
            }
        }

        rectTransform.position = new Vector3(savedX, slot.gridCenters[0].y, slot.gridCenters[0].z);

        for (int i = 0; i < slot.length; i++)
        {
            if (slot.gridCenters[i].x >= savedX - rectTransform.sizeDelta.x / 2)
            {
                slot.startCell = i;
                break;
            }
        }

        for (int i = slot.startCell; i < slot.length; i++)
        {
            if (slot.gridCenters[i].x <= savedX + rectTransform.sizeDelta.x / 2)
            {
                slot.endCell = i;
            }
            else break;
        }


        lineNum = slot.idx;
        slot.blockName = GetComponent<Image>().sprite.name;
        slot.endCell += 1;



        start = slot.startCell;
        end  = slot.endCell;
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            slot = other.GetComponent<TimeLine>();

            if (slot.haveBlock) isInLine = false;
            else isInLine = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = false;
            slot = null;
        }
    }

}



//public class FieldEffectBlock : MonoBehaviour
//{
//    //SpriteRenderer spriteRenderer;

//    //Image image;

//    //public Sprite[] idle;
//    //int state;
//    //int spriteIdx;


//    //// Start is called before the first frame update
//    //void Start()
//    //{
//    //    image = GetComponent<Image>();
//    //    state = 0;
//    //    spriteIdx = 0;

//    //}

//    //// Update is called once per frame
//    //void Update()
//    //{

//    //}


//    //public void TriggerChangeBlockState()
//    //{
//    //    if(state == 0)
//    //    {
//    //        image.sprite = idle[1];
//    //        //spriteRenderer.sprite = idle[1];
//    //        state = 1;
//    //    }

//    //    else
//    //    {
//    //        image.sprite = idle[0];
//    //        //spriteRenderer.sprite = idle[0];
//    //        state = 0;
//    //    }



//    //    //switch (state)
//    //    //{
//    //    //    case 0:
//    //    //}
//    //}

//}
