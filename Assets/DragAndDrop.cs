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
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        //targetRectTransform = rectTransform;
        //originPos = rectTransform.anchoredPosition;
        originPos = rectTransform.position;
    }



    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }


    public void OnDrag(PointerEventData eventData)
    {
        isBeingHeld = true;

        //rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        rectTransform.anchoredPosition += eventData.delta;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isBeingHeld = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (isInLine)
        //{
        //    this.gameObject.transform.position = LoadedPos;
        //}
    }

    public void OnDrop(PointerEventData eventData)
    {

    }

    Vector3 originPos;

    private void Start()
    {
        //originPos = rectTransform.anchoredPosition;
    }


    private void Update()
    {
        if (!isBeingHeld)
        {
            if (isInLine)
            {

                float rectangleWidth = 256f;
                float cellWidth = rectangleWidth / 10f;

                //float startX = LoadedPos.x - rectangleWidth / 2f;
                float startX = targetRectTransform.position.x - rectangleWidth / 2f;

                float diffff = 999999f;
                float savedX = 1f;

                for (int i = 0; i < 10; i++)
                {
                    float centerX = startX + (i * cellWidth) + (cellWidth / 2f);

                    if (Math.Abs(centerX - rectTransform.position.x) < diffff)
                    //if(Math.Abs(centerX - this.gameObject.transform.position.x) < diffff)
                    {
                        savedX = centerX;
                        //diffff = Math.Abs(centerX - this.gameObject.transform.position.x);
                        diffff = Math.Abs(centerX - rectTransform.position.x);
                        
                    }

                    else
                    {
                        if (savedX + 64f > targetRectTransform.position.x + rectangleWidth / 2f)
                        //if(savedX + 64f > LoadedPos.x + rectangleWidth / 2f)
                        {
                            rectTransform.position = new Vector3(startX + (8 * cellWidth) + (cellWidth / 2f), targetRectTransform.position.y, targetRectTransform.position.z);
                            //this.gameObject.transform.position = new Vector3(startX + (7 * cellWidth) + (cellWidth / 2f), LoadedPos.y, LoadedPos.z);
                            break;
                        }

                        else if (savedX - 64f < targetRectTransform.position.x - rectangleWidth / 2f)
                        //else if (savedX - 64f < LoadedPos.x - rectangleWidth / 2f)
                        {
                            rectTransform.position = new Vector3(startX + (2 * cellWidth) + (cellWidth / 2f), targetRectTransform.position.y, targetRectTransform.position.z);
                            //this.gameObject.transform.position = new Vector3(startX + (2 * cellWidth) + (cellWidth / 2f), LoadedPos.y, LoadedPos.z);
                            break;
                        }

                        else
                        {
                            rectTransform.position = new Vector3(savedX, targetRectTransform.position.y, targetRectTransform.position.z);
                            //this.gameObject.transform.position = new Vector3(savedX, LoadedPos.y, LoadedPos.z);
                            break;
                        }


                    }


                }


                //this.gameObject.transform.position = LoadedPos;
            }

            else rectTransform.position = originPos;

            //else this.gameObject.transform.position = originPos;

        }
    }



    bool isBeingHeld = false;
    public bool isInLine;
    Vector3 LoadedPos;

    Vector3 rectPosition;
    private RectTransform targetRectTransform;

    //Vector3 rectangleWidth;
    //Vector3 rectangleHeight;

    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.CompareTag("TimeLine"))
    //    {
    //        isInLine = true;
    //        LoadedPos = other.transform.position;
    //    }
    //}

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = true;
            //LoadedPos = other.transform.position;

            targetRectTransform = other.GetComponent<RectTransform>();


            //rectWidth = other.transform.rect.width;


            //targetRectTransform = other.GetComponent<RectTransform>();

            //// 사각형의 시작 위치와 너비를 가져옵니다.
            //rectPosition = targetRectTransform.position;
            //float rectWidth = targetRectTransform.rect.width;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("TimeLine"))
        {
            isInLine = false;
        }
    }

}