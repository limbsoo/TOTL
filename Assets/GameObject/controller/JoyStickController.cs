using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    public Vector2 inputVector;    // 추가
    private bool isInput;    // 추가


    public static JoyStickController instance { get; private set; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);

        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // var inputDir = eventData.position - rectTransform.anchoredPosition;
        // var clampedDir = inputDir.magnitude < leverRange ? inputDir 
        //     : inputDir.normalized * leverRange;
        // lever.anchoredPosition = clampedDir;

        //ControlJoystickLever(eventData);  // 추가
        isInput = true;    // 추가
    }

    // 오브젝트를 클릭해서 드래그 하는 도중에 들어오는 이벤트
    // 하지만 클릭을 유지한 상태로 마우스를 멈추면 이벤트가 들어오지 않음    
    public void OnDrag(PointerEventData eventData)
    {
        // var inputDir = eventData.position - rectTransform.anchoredPosition;
        // var clampedDir = inputDir.magnitude < leverRange ? inputDir 
        //     : inputDir.normalized * leverRange;
        // lever.anchoredPosition = clampedDir;

        

        ControlJoystickLever(eventData);    // 추가
        isInput = false;    // 추가
    }

    public Action JoyStickMove;

    // 추가
    public void ControlJoystickLever(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;

        Vector2 newPos = rectTransform.anchoredPosition;


        // 조이스틱의 x축 범위 제한
        if (Mathf.Abs(newPos.x) > leverRange)
        {
            newPos.x = Mathf.Sign(newPos.x) * leverRange;
        }

        // 조이스틱의 y축 범위 제한
        if (Mathf.Abs(newPos.y) > leverRange)
        {
            newPos.y = Mathf.Sign(newPos.y) * leverRange;
        }

        // 조이스틱 위치 업데이트
        rectTransform.anchoredPosition = newPos;

        // 조이스틱의 입력 값을 정규화 (leverRange 기준으로 -1 ~ 1 사이 값)
        inputVector = new Vector2(newPos.x / leverRange, newPos.y / leverRange);

        // 조이스틱 입력을 사용하여 캐릭터 이동 방향을 설정
        //MoveCharacter(inputVector);

        //if (Math.Abs(rectTransform.anchoredPosition.x) > leverRange)
        //{


        //    if(rectTransform.anchoredPosition.x > 0) newPos.x = leverRange;
        //    else newPos.x = - leverRange;

        //}

        //if (Math.Abs(rectTransform.anchoredPosition.y) > leverRange)
        //{
        //    if (rectTransform.anchoredPosition.y > 0) newPos.y = leverRange;
        //    else newPos.y = -leverRange;
        //}



        //rectTransform.anchoredPosition = newPos;

        //inputVector = eventData.delta;
        //inputVector = inputVector.normalized;


        ////var inputDir = eventData.position - rectTransform.anchoredPosition;

        ////rectTransform.anchoredPosition += eventData.delta;

        ////var clampedDir = inputDir.magnitude < leverRange ? inputDir
        ////    : inputDir.normalized * leverRange;



        ////lever.anchoredPosition = clampedDir;
        ////inputVector = clampedDir / leverRange;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }
}
