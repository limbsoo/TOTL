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

    public Vector2 inputVector;    // �߰�
    private bool isInput;    // �߰�


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

        //ControlJoystickLever(eventData);  // �߰�
        isInput = true;    // �߰�
    }

    // ������Ʈ�� Ŭ���ؼ� �巡�� �ϴ� ���߿� ������ �̺�Ʈ
    // ������ Ŭ���� ������ ���·� ���콺�� ���߸� �̺�Ʈ�� ������ ����    
    public void OnDrag(PointerEventData eventData)
    {
        // var inputDir = eventData.position - rectTransform.anchoredPosition;
        // var clampedDir = inputDir.magnitude < leverRange ? inputDir 
        //     : inputDir.normalized * leverRange;
        // lever.anchoredPosition = clampedDir;

        

        ControlJoystickLever(eventData);    // �߰�
        isInput = false;    // �߰�
    }

    public Action JoyStickMove;

    // �߰�
    public void ControlJoystickLever(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;

        Vector2 newPos = rectTransform.anchoredPosition;


        // ���̽�ƽ�� x�� ���� ����
        if (Mathf.Abs(newPos.x) > leverRange)
        {
            newPos.x = Mathf.Sign(newPos.x) * leverRange;
        }

        // ���̽�ƽ�� y�� ���� ����
        if (Mathf.Abs(newPos.y) > leverRange)
        {
            newPos.y = Mathf.Sign(newPos.y) * leverRange;
        }

        // ���̽�ƽ ��ġ ������Ʈ
        rectTransform.anchoredPosition = newPos;

        // ���̽�ƽ�� �Է� ���� ����ȭ (leverRange �������� -1 ~ 1 ���� ��)
        inputVector = new Vector2(newPos.x / leverRange, newPos.y / leverRange);

        // ���̽�ƽ �Է��� ����Ͽ� ĳ���� �̵� ������ ����
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
