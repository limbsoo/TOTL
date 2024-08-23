using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
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

    TimeLine tBlock;

    bool isSet = false;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        originPos = rectTransform.position;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        if(tBlock != null)
        {
            if(tBlock.haveBlock) tBlock.haveBlock = false; 
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
        if (isInLine && !tBlock.haveBlock)
        {
            // tBlock에 오브젝트가 없으면 위치를 정렬
            AlignObjectWithTimeLine();
            tBlock.haveBlock = true;
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

        for (int i = 0; i < tBlock.length; i++)
        {
            if (Math.Abs(tBlock.gridCenters[i].x - rectTransform.position.x) < difference)
            {
                savedX = tBlock.gridCenters[i].x;
                difference = Math.Abs(tBlock.gridCenters[i].x - rectTransform.position.x);
            }
            else break;
        }

        if (savedX == 0) return;

        float startPosX = tBlock.rectTransform.position.x - tBlock.rectTransform.sizeDelta.x / 2;
        float endPosX = tBlock.rectTransform.position.x + tBlock.rectTransform.sizeDelta.x / 2;

        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
        {
            for (int i = 0; i < tBlock.length; i++)
            {
                if (tBlock.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = tBlock.gridCenters[i].x;
                else break;
            }
        }
        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
        {
            for (int i = tBlock.length - 1; i >= 0; i--)
            {
                if (tBlock.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = tBlock.gridCenters[i].x;
                else break;
            }
        }

        rectTransform.position = new Vector3(savedX, tBlock.gridCenters[0].y, tBlock.gridCenters[0].z);

        for (int i = 0; i < tBlock.length; i++)
        {
            if (tBlock.gridCenters[i].x >= savedX - rectTransform.sizeDelta.x / 2)
            {
                tBlock.startCell = i;
                break;
            }
        }

        for (int i = tBlock.startCell; i < tBlock.length; i++)
        {
            if (tBlock.gridCenters[i].x <= savedX + rectTransform.sizeDelta.x / 2)
            {
                tBlock.endCell = i;
            }
            else break;
        }

        tBlock.endCell += 1;
    }



    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            tBlock = other.GetComponent<TimeLine>();

            if (tBlock.haveBlock) isInLine = false;
            else isInLine = true;

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = false;
            tBlock = null;
        }
    }

}

























//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;

//public class DragAndDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
//{

//    RectTransform rectTransform;
//    CanvasGroup canvasGroup;
//    [SerializeField] Canvas canvas;
//    private void Awake()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();

//        //targetRectTransform = rectTransform;
//        //originPos = rectTransform.anchoredPosition;
//        originPos = rectTransform.position;
//    }



//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        canvasGroup.alpha = .6f;
//        canvasGroup.blocksRaycasts = false;
//    }


//    public void OnDrag(PointerEventData eventData)
//    {
//        isBeingHeld = true;

//        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
//        rectTransform.anchoredPosition += eventData.delta;

//    }

//    public void OnEndDrag(PointerEventData eventData)
//    {
//        isBeingHeld = false;
//        canvasGroup.alpha = 1f;
//        canvasGroup.blocksRaycasts = true;
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//    }

//    Vector3 originPos;

//    private void Start()
//    {
//        //originPos = rectTransform.anchoredPosition;
//    }


//    private void Update()
//    {
//        if (!isBeingHeld)
//        {
//            if (isInLine)
//            {

//                float rectangleWidth = 256f;
//                float cellWidth = rectangleWidth / 10f;

//                //float startX = LoadedPos.x - rectangleWidth / 2f;
//                float startX = targetRectTransform.position.x - rectangleWidth / 2f;

//                float diffff = 999999f;
//                float savedX = 1f;

//                for (int i = 0; i < 10; i++)
//                {
//                    float centerX = startX + (i * cellWidth) + (cellWidth / 2f);

//                    if (Math.Abs(centerX - rectTransform.position.x) < diffff)
//                    //if(Math.Abs(centerX - this.gameObject.transform.position.x) < diffff)
//                    {
//                        savedX = centerX;
//                        //diffff = Math.Abs(centerX - this.gameObject.transform.position.x);
//                        diffff = Math.Abs(centerX - rectTransform.position.x);

//                    }

//                    else
//                    {
//                        if (savedX + 64f > targetRectTransform.position.x + rectangleWidth / 2f)
//                        //if(savedX + 64f > LoadedPos.x + rectangleWidth / 2f)
//                        {
//                            rectTransform.position = new Vector3(startX + (8 * cellWidth) + (cellWidth / 2f), targetRectTransform.position.y, targetRectTransform.position.z);
//                            //this.gameObject.transform.position = new Vector3(startX + (7 * cellWidth) + (cellWidth / 2f), LoadedPos.y, LoadedPos.z);
//                            break;
//                        }

//                        else if (savedX - 64f < targetRectTransform.position.x - rectangleWidth / 2f)
//                        //else if (savedX - 64f < LoadedPos.x - rectangleWidth / 2f)
//                        {
//                            rectTransform.position = new Vector3(startX + (2 * cellWidth) + (cellWidth / 2f), targetRectTransform.position.y, targetRectTransform.position.z);
//                            //this.gameObject.transform.position = new Vector3(startX + (2 * cellWidth) + (cellWidth / 2f), LoadedPos.y, LoadedPos.z);
//                            break;
//                        }

//                        else
//                        {
//                            rectTransform.position = new Vector3(savedX, targetRectTransform.position.y, targetRectTransform.position.z);
//                            //this.gameObject.transform.position = new Vector3(savedX, LoadedPos.y, LoadedPos.z);
//                            break;
//                        }


//                    }


//                }


//                //this.gameObject.transform.position = LoadedPos;
//            }

//            else rectTransform.position = originPos;

//            //else this.gameObject.transform.position = originPos;

//        }
//    }



//    bool isBeingHeld = false;
//    public bool isInLine;
//    Vector3 LoadedPos;

//    Vector3 rectPosition;
//    private RectTransform targetRectTransform;

//    //Vector3 rectangleWidth;
//    //Vector3 rectangleHeight;

//    //private void OnTriggerEnter2D(Collider2D other)
//    //{
//    //    if (other.CompareTag("TimeLine"))
//    //    {
//    //        isInLine = true;
//    //        LoadedPos = other.transform.position;
//    //    }
//    //}

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = true;
//            //LoadedPos = other.transform.position;

//            targetRectTransform = other.GetComponent<RectTransform>();


//            //rectWidth = other.transform.rect.width;


//            //targetRectTransform = other.GetComponent<RectTransform>();

//            //// 사각형의 시작 위치와 너비를 가져옵니다.
//            //rectPosition = targetRectTransform.position;
//            //float rectWidth = targetRectTransform.rect.width;
//        }
//    }


//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = false;
//        }
//    }

//}