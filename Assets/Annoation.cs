
//StageManager.cs

//public static StageManager instance;
//public static StageManager Instance
//{
//    get
//    {
//        if (instance == null)
//        {
//            instance = FindObjectOfType<StageManager>();
//            if (instance == null)
//            {
//                GameObject singletonObject = new GameObject(typeof(StageManager).ToString());
//                instance = singletonObject.AddComponent<StageManager>();
//                DontDestroyOnLoad(singletonObject);
//            }
//        }
//        return instance;
//    }
//}

//private void Awake()
//{
//    if (instance == null)
//    {
//        instance = this;
//        DontDestroyOnLoad(gameObject);
//    }
//    else if (instance != this)
//    {
//        Destroy(gameObject);
//    }
//}

// 메인화면으로 돌아왔을 때 인스턴스를 제거하는 메서드
//public void ResetInstance()
//{
//    instance = null;
//    Destroy(gameObject);
//}

//Player.cs

//public void Move()
//{
//    if (movement != Vector3.zero)
//    {
//        Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
//        float width = StageManager.mapTransform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
//        float height = StageManager.mapTransform.localScale.z * 10f;
//        newPos.x = Mathf.Clamp(newPos.x, StageManager.mapTransform.position.x - width / 2, StageManager.mapTransform.position.x + width / 2);
//        newPos.z = Mathf.Clamp(newPos.z, StageManager.mapTransform.position.z - height / 2, StageManager.mapTransform.position.z + height / 2);
//        rb.MovePosition(newPos); // 물리적 이동 처리
//        //rb.MoveRotation(lastRotation);

//        // 움직이는 방향으로 회전
//        //if (movement != Vector3.zero)
//        {
//            Quaternion newRotation = Quaternion.LookRotation(movement);
//            rb.MoveRotation(newRotation);

//        }

//        // 움직이는 방향으로 회전
//        //lastRotation = Quaternion.LookRotation(movement);


//    }

//    else
//    {
//        rb.velocity = Vector3.zero; // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
//        //rb.MoveRotation(lastRotation); // 마지막 회전 값 유지
//    }

//    //rb.MoveRotation(lastRotation);

//    //else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
//}





//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
//{
//    RectTransform rectTransform;
//    CanvasGroup canvasGroup;
//    [SerializeField] Canvas canvas;

//    Vector3 originPos;
//    bool isBeingHeld = false;
//    public bool isInLine;
//    Vector3 LoadedPos;

//    Vector3 rectPosition;
//    private RectTransform targetRectTransform;

//    TimeLine nearSlot;

//    bool isSet = false;

//    public int lineNum;

//    public int start;
//    public int end;

//    public int idx;

//    TimeLine savedSlot;


//    private void Start()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        originPos = rectTransform.position;
//        canvasGroup.alpha = .6f;
//        canvasGroup.blocksRaycasts = false;

//        //if (slot != null)
//        //{
//        //    if (slot.haveBlock) slot.haveBlock = false;
//        //}
//    }

//    public void Init()
//    {
//        start = -1;
//        end = -1;
//        savedSlot = null;
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
//            if (nearSlot.feb == null)
//            {
//                nearSlot.feb = this;
//                AlignObjectWithTimeLine();
//                FindStartEndIdx();
//                originPos = rectTransform.position;
//                savedSlot = nearSlot;
//            }

//            else
//            {
//                if (nearSlot.feb == this)
//                {
//                    AlignObjectWithTimeLine();
//                    FindStartEndIdx();
//                    originPos = rectTransform.position;
//                    savedSlot = nearSlot;
//                }

//                else
//                {
//                    FieldEffectBlock otherBlock = nearSlot.feb;

//                    Vector3 tempPos = otherBlock.rectTransform.position;
//                    otherBlock.rectTransform.position = originPos;
//                    rectTransform.position = tempPos;

//                    //// 2. 슬롯 정보 교환 (savedSlot, nearSlot)
//                    TimeLine tempSlot = otherBlock.savedSlot;
//                    otherBlock.savedSlot = savedSlot;
//                    savedSlot = tempSlot;

//                    // 2. 슬롯 정보 교환 (savedSlot, nearSlot)
//                    TimeLine tempSlot2 = otherBlock.nearSlot;
//                    otherBlock.nearSlot = nearSlot;
//                    nearSlot = tempSlot2;

//                    int tmpStat = otherBlock.start;
//                    otherBlock.start = start;
//                    start = tmpStat;

//                    int tmpEnd = otherBlock.end;
//                    otherBlock.end = end;
//                    end = tmpEnd;

//                    nearSlot.feb = this;
//                    originPos = rectTransform.position;
//                    otherBlock.nearSlot.feb = otherBlock;
//                }
//            }

//        }



//        else rectTransform.position = originPos;

//    }

//    //public void OnEndDrag(PointerEventData eventData)
//    //{
//    //    isBeingHeld = false;
//    //    canvasGroup.alpha = 1f;
//    //    canvasGroup.blocksRaycasts = true;

//    //    if (isInLine)
//    //    {
//    //        if(nearSlot.feb == null)
//    //        {
//    //            nearSlot.feb = this;
//    //            AlignObjectWithTimeLine();
//    //            FindStartEndIdx();
//    //            savedSlot = nearSlot;
//    //            originPos = rectTransform.position;
//    //        }

//    //        else
//    //        {
//    //            if(nearSlot.feb == this)
//    //            {
//    //                AlignObjectWithTimeLine();
//    //                FindStartEndIdx();
//    //                originPos = rectTransform.position;
//    //            }

//    //            else
//    //            {
//    //                TimeLine slot = nearSlot.feb.nearSlot;
//    //                nearSlot.feb.nearSlot = savedSlot;
//    //                savedSlot = slot;

//    //                //FieldEffectBlock febbbb = new FieldEffectBlock();
//    //                //febbbb = nearSlot.feb;
//    //                nearSlot.feb.rectTransform.position = originPos;
//    //                nearSlot.feb = this;
//    //                originPos = rectTransform.position;
//    //            }

//    //            //FieldEffectBlock febbbb = new FieldEffectBlock();

//    //            //febbbb = nearSlot.feb;

//    //            //febbbb.rectTransform.position = originPos;


//    //            //nearSlot.feb = this;
//    //            //AlignObjectWithTimeLine();
//    //            //FindStartEndIdx();
//    //            //originPos = rectTransform.position;

//    //            //if (savedSlot == null)
//    //            //{
//    //            //    FieldEffectBlock febbbb = nearSlot.feb;


//    //            //    nearSlot.feb.Init();
//    //            //    nearSlot.feb.rectPosition = originPos;

//    //            //    nearSlot.feb = this;
//    //            //    AlignObjectWithTimeLine();
//    //            //    FindStartEndIdx();
//    //            //    savedSlot = nearSlot;
//    //            //    originPos = rectTransform.position;



//    //            //    //int tempStart = nearSlot.feb.start;
//    //            //    //int tempEnd = nearSlot.feb.end;

//    //            //    //nearSlot.feb.Init();
//    //            //    //nearSlot.feb.rectPosition = originPos;


//    //            //}

//    //            //else
//    //            //{
//    //            //    FieldEffectBlock febb = nearSlot.feb;
//    //            //    febb.savedSlot = savedSlot;
//    //            //    febb.rectPosition = originPos;

//    //            //    nearSlot.feb = this;
//    //            //    AlignObjectWithTimeLine();
//    //            //    FindStartEndIdx();
//    //            //    savedSlot = nearSlot;
//    //            //    originPos = rectTransform.position;



//    //            //    //nearSlot.feb
//    //            //    //this



//    //            //    //nearSlot.feb.savedSlot = 

//    //            //    //FieldEffectBlock febb = nearSlot.feb;
//    //            //    //nearSlot.feb = this;


//    //            //}

//    //            //if(savedSlot == null)
//    //            //{
//    //            //    nearSlot.feb.savedSlot = null;
//    //            //    nearSlot.feb.rectPosition = originPos;

//    //            //    nearSlot.feb = this;
//    //            //    AlignObjectWithTimeLine();
//    //            //    FindStartEndIdx();
//    //            //    savedSlot = nearSlot;
//    //            //}

//    //            //else
//    //            //{
//    //            //    nearSlot.feb.savedSlot = savedSlot;
//    //            //    nearSlot.feb.rectPosition = originPos;


//    //            //    nearSlot.feb = this;
//    //            //    AlignObjectWithTimeLine();
//    //            //    FindStartEndIdx();
//    //            //    savedSlot = nearSlot;
//    //            //}
//    //        }

//    //        //if (!slot.haveBlock)
//    //        //{
//    //        //    // tBlock에 오브젝트가 없으면 위치를 정렬
//    //        //    AlignObjectWithTimeLine();
//    //        //    slot.haveBlock = true;
//    //        //    isSet = true;

//    //        //    originPos = rectTransform.position;
//    //        //}

//    //        //else
//    //        //{
//    //        //    FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[slot.blockIdx].GetComponent<FieldEffectBlock>();
//    //        //    feb.rectTransform.position = originPos;
//    //        //    feb.start = start;
//    //        //    feb.end = end;
//    //        //    feb.lineNum = lineNum;
//    //        //    feb.isSet = isSet;
//    //        //    feb.slot.haveBlock = slot.haveBlock;
//    //        //    feb.slot.blockIdx = feb.idx;

//    //        //    AlignObjectWithTimeLine();
//    //        //    slot.haveBlock = true;
//    //        //    isSet = true;

//    //        //    originPos = rectTransform.position;
//    //        //}
//    //    }

//    //    else rectTransform.position = originPos;

//    //}

//    private void FindStartEndIdx()
//    {
//        //for (int i = 0; i < nearSlot.length; i++)
//        //{
//        //    if (nearSlot.gridCenters[i].x >= rectTransform.position.x - rectTransform.sizeDelta.x / 2)
//        //    {
//        //        start = i;
//        //        break;
//        //    }
//        //}

//        //for (int i = start; i < nearSlot.length; i++)
//        //{
//        //    if (nearSlot.gridCenters[i].x <= rectTransform.position.x + rectTransform.sizeDelta.x / 2)
//        //    {
//        //        end = i;
//        //    }
//        //    else break;
//        //}


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


//    }



//    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
//    private void AlignObjectWithTimeLine()
//    {
//        float difference = 999999f;
//        float savedX = 0f;

//        for (int i = 0; i < nearSlot.length; i++)
//        {
//            if (Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x) < difference)
//            {
//                savedX = nearSlot.gridCenters[i].x;
//                difference = Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x);
//            }
//            else break;
//        }

//        if (savedX == 0) return;

//        float startPosX = nearSlot.rectTransform.position.x - nearSlot.rectTransform.sizeDelta.x / 2;
//        float endPosX = nearSlot.rectTransform.position.x + nearSlot.rectTransform.sizeDelta.x / 2;

//        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
//        {
//            for (int i = 0; i < nearSlot.length; i++)
//            {
//                if (nearSlot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }
//        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
//        {
//            for (int i = nearSlot.length - 1; i >= 0; i--)
//            {
//                if (nearSlot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }

//        rectTransform.position = new Vector3(savedX, nearSlot.gridCenters[0].y, nearSlot.gridCenters[0].z);

//        //for (int i = 0; i < slot.length; i++)
//        //{
//        //    if (slot.gridCenters[i].x >= savedX - rectTransform.sizeDelta.x / 2)
//        //    {
//        //        slot.startCell = i;
//        //        break;
//        //    }
//        //}

//        //for (int i = slot.startCell; i < slot.length; i++)
//        //{
//        //    if (slot.gridCenters[i].x <= savedX + rectTransform.sizeDelta.x / 2)
//        //    {
//        //        slot.endCell = i;
//        //    }
//        //    else break;
//        //}


//        //lineNum = slot.idx;
//        //slot.blockName = GetComponent<Image>().sprite.name;
//        //slot.endCell += 1;

//        //slot.blockIdx = idx;

//        //start = slot.startCell;
//        //end = slot.endCell;
//    }



//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            nearSlot = other.GetComponent<TimeLine>();
//            isInLine = true;
//            //if (slot.haveBlock) isInLine = false;
//            //else isInLine = true;

//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = false;
//            nearSlot = null;
//        }
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//    }


//}







////public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
////{
////    RectTransform rectTransform;
////    CanvasGroup canvasGroup;
////    [SerializeField] Canvas canvas;

////    Vector3 originPos;
////    bool isBeingHeld = false;
////    public bool isInLine;
////    Vector3 LoadedPos;

////    Vector3 rectPosition;
////    private RectTransform targetRectTransform;

////    TimeLine slot;

////    bool isSet = false;

////    public int lineNum;

////    public int start;
////    public int end;

////    public int idx;


////    private void Start()
////    {
////        rectTransform = GetComponent<RectTransform>();
////        canvasGroup = GetComponent<CanvasGroup>();
////    }

////    public void OnBeginDrag(PointerEventData eventData)
////    {
////        originPos = rectTransform.position;

////        canvasGroup.alpha = .6f;
////        canvasGroup.blocksRaycasts = false;

////        if (slot != null)
////        {
////            if (slot.haveBlock) slot.haveBlock = false;
////        }
////    }

////    public void OnDrag(PointerEventData eventData)
////    {
////        isBeingHeld = true;
////        rectTransform.anchoredPosition += eventData.delta;
////    }

////    public void OnEndDrag(PointerEventData eventData)
////    {
////        isBeingHeld = false;
////        canvasGroup.alpha = 1f;
////        canvasGroup.blocksRaycasts = true;


////        if (isInLine)
////        {
////            if (!slot.haveBlock)
////            {
////                // tBlock에 오브젝트가 없으면 위치를 정렬
////                AlignObjectWithTimeLine();
////                slot.haveBlock = true;
////                isSet = true;

////                originPos = rectTransform.position;
////            }

////            else
////            {
////                FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[slot.blockIdx].GetComponent<FieldEffectBlock>();
////                feb.rectTransform.position = originPos;
////                feb.start = start;
////                feb.end = end;
////                feb.lineNum = lineNum;
////                feb.isSet = isSet;
////                feb.slot.haveBlock = slot.haveBlock;
////                feb.slot.blockIdx = feb.idx;

////                AlignObjectWithTimeLine();
////                slot.haveBlock = true;
////                isSet = true;

////                originPos = rectTransform.position;



////            }
////        }

////        else rectTransform.position = originPos;



////        //// 드래그 종료 시 조건 검사
////        //if (isInLine)
////        //{
////        //    if(!slot.haveBlock)
////        //    {
////        //        // tBlock에 오브젝트가 없으면 위치를 정렬
////        //        AlignObjectWithTimeLine();
////        //        slot.haveBlock = true;
////        //        isSet = true;

////        //        originPos = rectTransform.position;
////        //    }

////        //    else
////        //    { 
////        //        FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[slot.blockIdx].GetComponent<FieldEffectBlock>();
////        //        feb.rectTransform.position = originPos;
////        //        feb.start = start;
////        //        feb.end = end;
////        //        feb.lineNum = lineNum;
////        //        feb.isSet = isSet;
////        //        feb.slot.haveBlock = false;
////        //        feb.slot.blockIdx = feb.idx;

////        //        //AlignObjectWithTimeLine();
////        //        //slot.haveBlock = true;
////        //        //isSet = true;

////        //        originPos = rectTransform.position;



////        //    }
////        //}

////        //else rectTransform.position = originPos;




////        //// 드래그 종료 시 조건 검사
////        //if (isInLine && !slot.haveBlock)
////        //{
////        //    // tBlock에 오브젝트가 없으면 위치를 정렬
////        //    AlignObjectWithTimeLine();
////        //    slot.haveBlock = true;
////        //    isSet = true;

////        //    originPos = rectTransform.position;
////        //}

////        //else rectTransform.position = originPos;
////    }

////    public void OnPointerDown(PointerEventData eventData)
////    {
////    }

////    public void OnDrop(PointerEventData eventData)
////    {
////    }


////    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
////    private void AlignObjectWithTimeLine()
////    {
////        float difference = 999999f;
////        float savedX = 0f;

////        for (int i = 0; i < slot.length; i++)
////        {
////            if (Math.Abs(slot.gridCenters[i].x - rectTransform.position.x) < difference)
////            {
////                savedX = slot.gridCenters[i].x;
////                difference = Math.Abs(slot.gridCenters[i].x - rectTransform.position.x);
////            }
////            else break;
////        }

////        if (savedX == 0) return;

////        float startPosX = slot.rectTransform.position.x - slot.rectTransform.sizeDelta.x / 2;
////        float endPosX = slot.rectTransform.position.x + slot.rectTransform.sizeDelta.x / 2;

////        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
////        {
////            for (int i = 0; i < slot.length; i++)
////            {
////                if (slot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = slot.gridCenters[i].x;
////                else break;
////            }
////        }
////        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
////        {
////            for (int i = slot.length - 1; i >= 0; i--)
////            {
////                if (slot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = slot.gridCenters[i].x;
////                else break;
////            }
////        }

////        rectTransform.position = new Vector3(savedX, slot.gridCenters[0].y, slot.gridCenters[0].z);

////        for (int i = 0; i < slot.length; i++)
////        {
////            if (slot.gridCenters[i].x >= savedX - rectTransform.sizeDelta.x / 2)
////            {
////                slot.startCell = i;
////                break;
////            }
////        }

////        for (int i = slot.startCell; i < slot.length; i++)
////        {
////            if (slot.gridCenters[i].x <= savedX + rectTransform.sizeDelta.x / 2)
////            {
////                slot.endCell = i;
////            }
////            else break;
////        }


////        lineNum = slot.idx;
////        slot.blockName = GetComponent<Image>().sprite.name;
////        slot.endCell += 1;

////        slot.blockIdx = idx;

////        start = slot.startCell;
////        end  = slot.endCell;
////    }



////    private void OnTriggerStay2D(Collider2D other)
////    {
////        if (other.CompareTag("TimeLine"))
////        {
////            slot = other.GetComponent<TimeLine>();
////            isInLine = true;
////            //if (slot.haveBlock) isInLine = false;
////            //else isInLine = true;

////        }
////    }

////    private void OnTriggerExit2D(Collider2D other)
////    {
////        if (other.CompareTag("TimeLine"))
////        {
////            isInLine = false;
////            slot = null;
////        }
////    }

////}












////public class FieldEffectBlock : MonoBehaviour
////{
////    //SpriteRenderer spriteRenderer;

////    //Image image;

////    //public Sprite[] idle;
////    //int state;
////    //int spriteIdx;


////    //// Start is called before the first frame update
////    //void Start()
////    //{
////    //    image = GetComponent<Image>();
////    //    state = 0;
////    //    spriteIdx = 0;

////    //}

////    //// Update is called once per frame
////    //void Update()
////    //{

////    //}


////    //public void TriggerChangeBlockState()
////    //{
////    //    if(state == 0)
////    //    {
////    //        image.sprite = idle[1];
////    //        //spriteRenderer.sprite = idle[1];
////    //        state = 1;
////    //    }

////    //    else
////    //    {
////    //        image.sprite = idle[0];
////    //        //spriteRenderer.sprite = idle[0];
////    //        state = 0;
////    //    }



////    //    //switch (state)
////    //    //{
////    //    //    case 0:
////    //    //}
////    //}

////}


//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
//{
//    RectTransform rectTransform;
//    CanvasGroup canvasGroup;
//    [SerializeField] Canvas canvas;

//    Vector3 originPos;
//    bool isBeingHeld = false;
//    public bool isInLine;
//    Vector3 LoadedPos;

//    Vector3 rectPosition;
//    private RectTransform targetRectTransform;

//    TimeLine nearSlot;

//    bool isSet = false;

//    public int lineNum;

//    public int start;
//    public int end;

//    public int idx;

//    TimeLine savedSlot;


//    private void Start()
//    {
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        originPos = rectTransform.position;
//        canvasGroup.alpha = .6f;
//        canvasGroup.blocksRaycasts = false;
//    }

//    public void Init()
//    {
//        start = -1;
//        end = -1;
//        savedSlot = null;
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
//            if (nearSlot.feb == null)
//            {
//                nearSlot.feb = this;
//                AlignObjectWithTimeLine();
//                FindStartEndIdx();
//                originPos = rectTransform.position;
//                savedSlot = nearSlot;
//            }

//            else
//            {
//                if (nearSlot.feb == this)
//                {
//                    savedSlot.feb = null;
//                    AlignObjectWithTimeLine();
//                    FindStartEndIdx();
//                    originPos = rectTransform.position;
//                    savedSlot = nearSlot;
//                }

//                else
//                {
//                    if(savedSlot == null) rectTransform.position = originPos;

//                    else
//                    {
//                        FieldEffectBlock otherBlock = nearSlot.feb;
//                        Vector3 tempPos = otherBlock.rectTransform.position;
//                        otherBlock.rectTransform.position = originPos;
//                        rectTransform.position = tempPos;

//                        TimeLine tempSlot = otherBlock.savedSlot;
//                        otherBlock.savedSlot = savedSlot;
//                        //savedSlot = tempSlot;

//                        TimeLine tempSlot2 = otherBlock.nearSlot;
//                        otherBlock.nearSlot = savedSlot;
//                        //nearSlot = tempSlot2;


//                        AlignObjectWithTimeLine();
//                        FindStartEndIdx();
//                        originPos = rectTransform.position;
//                        savedSlot = nearSlot;
//                    }



//                    //FieldEffectBlock otherBlock = nearSlot.feb;

//                    //Vector3 tempPos = otherBlock.rectTransform.position;
//                    //otherBlock.rectTransform.position = originPos;
//                    //rectTransform.position = tempPos;

//                    //TimeLine tempSlot = otherBlock.nearSlot;
//                    //otherBlock.nearSlot = savedSlot;
//                    //nearSlot = tempSlot;

//                    //otherBlock.nearSlot.feb = otherBlock;
//                    //nearSlot.feb = this;
//                    //originPos = rectTransform.position;
//                }
//            }

//        }



//        else rectTransform.position = originPos;

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

//        nearSlot.blockIdx = idx;
//    }



//    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
//    private void AlignObjectWithTimeLine()
//    {
//        float difference = 999999f;
//        float savedX = 0f;

//        for (int i = 0; i < nearSlot.length; i++)
//        {
//            if (Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x) < difference)
//            {
//                savedX = nearSlot.gridCenters[i].x;
//                difference = Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x);
//            }
//            else break;
//        }

//        if (savedX == 0) return;

//        float startPosX = nearSlot.rectTransform.position.x - nearSlot.rectTransform.sizeDelta.x / 2;
//        float endPosX = nearSlot.rectTransform.position.x + nearSlot.rectTransform.sizeDelta.x / 2;

//        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
//        {
//            for (int i = 0; i < nearSlot.length; i++)
//            {
//                if (nearSlot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }
//        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
//        {
//            for (int i = nearSlot.length - 1; i >= 0; i--)
//            {
//                if (nearSlot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }

//        rectTransform.position = new Vector3(savedX, nearSlot.gridCenters[0].y, nearSlot.gridCenters[0].z);

//    }



//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            nearSlot = other.GetComponent<TimeLine>();
//            isInLine = true;
//            //if (slot.haveBlock) isInLine = false;
//            //else isInLine = true;

//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = false;
//            nearSlot = null;
//        }
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//    }


//}



//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.Events;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//public class FieldEffectBlock : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
//{
//    RectTransform rectTransform;
//    CanvasGroup canvasGroup;
//    [SerializeField] Canvas canvas;

//    Vector3 originPos;
//    bool isBeingHeld = false;
//    public bool isInLine;
//    Vector3 LoadedPos;

//    Vector3 rectPosition;
//    private RectTransform targetRectTransform;

//    TimeLine nearSlot;

//    bool isSet;

//    public int lineNum;

//    public int start;
//    public int end;

//    public int idx;

//    TimeLine savedSlot;


//    public int slotIdx;
//    public int savedSlotIdx;

//    private Rigidbody2D rb2;

//    private void Start()
//    {
//        isSet = false;
//        rectTransform = GetComponent<RectTransform>();
//        canvasGroup = GetComponent<CanvasGroup>();
//        rb2 = GetComponent<Rigidbody2D>();
//    }

//    public void OnBeginDrag(PointerEventData eventData)
//    {
//        originPos = rectTransform.position;
//        canvasGroup.alpha = .6f;
//        canvasGroup.blocksRaycasts = false;

//        isSet = false;



//        if (nearSlot != null)
//        {
//            if (nearSlot.feb != null)
//            {
//                nearSlot.feb = null;
//            }



//            //nearSlot.blockIdx = -1;


//        }

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
//                FieldEffectBlock otherBlock = nearSlot.feb;
//                Vector3 tempPos = otherBlock.rectTransform.position;
//                otherBlock.rb2.MovePosition(originPos);
//                rb2.MovePosition(tempPos);
//            }

//            //isSet = true;
//        }

//        else rb2.MovePosition(originPos);



//        //if (isInLine)
//        //{
//        //    if(nearSlot.feb != null)
//        //    {
//        //        FieldEffectBlock otherBlock = nearSlot.feb;
//        //        Vector3 tempPos = otherBlock.rectTransform.position;
//        //        otherBlock.rb2.MovePosition(tempPos);
//        //        rb2.MovePosition(tempPos);
//        //        otherBlock.isSet = true;

//        //        //nearSlot.feb = this;
//        //        //AlignObjectWithTimeLine();
//        //        //FindStartEndIdx();
//        //        //originPos = rectTransform.position;
//        //    }


//        //    originPos = rectTransform.position;
//        //    isSet = true;


//        //    //if(savedSlot == null)
//        //    //{
//        //    //    if (nearSlot.feb == null)
//        //    //    {
//        //    //        nearSlot.feb = this;
//        //    //        AlignObjectWithTimeLine();
//        //    //        FindStartEndIdx();
//        //    //        originPos = rectTransform.position;
//        //    //        savedSlot = nearSlot;
//        //    //    }

//        //    //    else
//        //    //    {
//        //    //        rectTransform.position = originPos;
//        //    //    }




//        //    //}

//        //    //else
//        //    //{
//        //    //    if (nearSlot.feb == null)
//        //    //    {
//        //    //        AlignObjectWithTimeLine();
//        //    //        FindStartEndIdx();
//        //    //        originPos = rectTransform.position;
//        //    //        savedSlot = nearSlot;
//        //    //    }

//        //    //    else
//        //    //    {
//        //    //        if (nearSlot.feb == this)
//        //    //        {
//        //    //            savedSlot.feb = null;
//        //    //            AlignObjectWithTimeLine();
//        //    //            FindStartEndIdx();
//        //    //            originPos = rectTransform.position;
//        //    //            savedSlot = nearSlot;
//        //    //        }

//        //    //        else
//        //    //        {
//        //    //            FieldEffectBlock otherBlock = nearSlot.feb;
//        //    //            Vector3 tempPos = otherBlock.rectTransform.position;
//        //    //            otherBlock.rb2.MovePosition(tempPos);
//        //    //            rb2.MovePosition(tempPos);
//        //    //            originPos = rectTransform.position;



//        //    //            //if (nearSlot == null) rectTransform.position = originPos;

//        //    //            //else
//        //    //            //{
//        //    //            //    FieldEffectBlock otherBlock = nearSlot.feb;
//        //    //            //    Vector3 tempPos = otherBlock.rectTransform.position;
//        //    //            //    otherBlock.rb2.MovePosition(tempPos);
//        //    //            //    rb2.MovePosition(tempPos);
//        //    //            //    originPos = rectTransform.position;
//        //    //            //}

//        //    //        }
//        //    //    }
//        //    //}

//        //}



//        //else rectTransform.position = originPos;

//    }

//    //public void OnEndDrag(PointerEventData eventData)
//    //{
//    //    isBeingHeld = false;
//    //    canvasGroup.alpha = 1f;
//    //    canvasGroup.blocksRaycasts = true;

//    //    if (isInLine)
//    //    {
//    //        if(nearSlot.blockIdx == -1)
//    //        {
//    //            AlignObjectWithTimeLine();
//    //            FindStartEndIdx();
//    //            originPos = rectTransform.position;
//    //            //savedSlot = nearSlot;

//    //            nearSlot.savedIdx = nearSlot.blockIdx;
//    //        }

//    //        else
//    //        {
//    //            FieldEffectBlock otherBlock = FieldEffectPopUpManager.instance.blocks[nearSlot.blockIdx].GetComponent<FieldEffectBlock>();


//    //            //nearSlot.blockIdx = 



//    //            //FieldEffectBlock otherBlock = FieldEffectPopUpManager.instance.blocks[nearSlot.blockIdx].GetComponent<FieldEffectBlock>();
//    //            //Vector3 tempPos = otherBlock.rectTransform.position;
//    //            //otherBlock.rectTransform.position = originPos;
//    //            //rectTransform.position = tempPos;






//    //            //int blockIdxsss = nearSlot.savedIdx;
//    //            //savedSlot.blockIdx = savedSlot.idx;
//    //            //savedSlot.idx = blockIdxsss;

//    //            //nearSlot.blockIdx





//    //            //TimeLine tempSlot = otherBlock.savedSlot;
//    //            //otherBlock.savedSlot = savedSlot;
//    //            ////savedSlot = tempSlot;

//    //            //TimeLine tempSlot2 = otherBlock.nearSlot;
//    //            //otherBlock.nearSlot = savedSlot;
//    //            ////nearSlot = tempSlot2;


//    //            //AlignObjectWithTimeLine();
//    //            //FindStartEndIdx();
//    //            originPos = rectTransform.position;
//    //            //savedSlot = nearSlot;
//    //        }


//    //        //if (nearSlot.feb == null)
//    //        //{
//    //        //    nearSlot.feb = this;
//    //        //    AlignObjectWithTimeLine();
//    //        //    FindStartEndIdx();
//    //        //    originPos = rectTransform.position;
//    //        //    savedSlot = nearSlot;
//    //        //}

//    //        //else
//    //        //{
//    //        //    if (nearSlot.feb == this)
//    //        //    {
//    //        //        savedSlot.feb = null;
//    //        //        AlignObjectWithTimeLine();
//    //        //        FindStartEndIdx();
//    //        //        originPos = rectTransform.position;
//    //        //        savedSlot = nearSlot;
//    //        //    }

//    //        //    else
//    //        //    {
//    //        //        if (savedSlot == null) rectTransform.position = originPos;

//    //        //        else
//    //        //        {
//    //        //            FieldEffectBlock otherBlock = nearSlot.feb;
//    //        //            Vector3 tempPos = otherBlock.rectTransform.position;
//    //        //            otherBlock.rectTransform.position = originPos;
//    //        //            rectTransform.position = tempPos;

//    //        //            TimeLine tempSlot = otherBlock.savedSlot;
//    //        //            otherBlock.savedSlot = savedSlot;
//    //        //            //savedSlot = tempSlot;

//    //        //            TimeLine tempSlot2 = otherBlock.nearSlot;
//    //        //            otherBlock.nearSlot = savedSlot;
//    //        //            //nearSlot = tempSlot2;


//    //        //            AlignObjectWithTimeLine();
//    //        //            FindStartEndIdx();
//    //        //            originPos = rectTransform.position;
//    //        //            savedSlot = nearSlot;
//    //        //        }



//    //        //        //FieldEffectBlock otherBlock = nearSlot.feb;

//    //        //        //Vector3 tempPos = otherBlock.rectTransform.position;
//    //        //        //otherBlock.rectTransform.position = originPos;
//    //        //        //rectTransform.position = tempPos;

//    //        //        //TimeLine tempSlot = otherBlock.nearSlot;
//    //        //        //otherBlock.nearSlot = savedSlot;
//    //        //        //nearSlot = tempSlot;

//    //        //        //otherBlock.nearSlot.feb = otherBlock;
//    //        //        //nearSlot.feb = this;
//    //        //        //originPos = rectTransform.position;
//    //        //    }
//    //        //}

//    //    }



//    //    else rectTransform.position = originPos;

//    //}

//    //public void OnEndDrag(PointerEventData eventData)
//    //{
//    //    isBeingHeld = false;
//    //    canvasGroup.alpha = 1f;
//    //    canvasGroup.blocksRaycasts = true;

//    //    if (isInLine)
//    //    {
//    //        if (!nearSlot.haveBlock)
//    //        {
//    //            AlignObjectWithTimeLine();
//    //            FindStartEndIdx();
//    //            originPos = rectTransform.position;
//    //            nearSlot.haveBlock = true;
//    //            savedSlot = nearSlot;
//    //        }

//    //        else
//    //        {
//    //            if(savedSlot != null)
//    //            {

//    //            }

//    //            else
//    //            {

//    //            }


//    //            //rectTransform.position = originPos;
//    //        }


//    //    }


//    //    else rectTransform.position = originPos;

//    //}



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

//        nearSlot.blockIdx = idx;
//    }



//    // 오브젝트를 타임라인에 맞춰 정렬하는 메서드
//    private void AlignObjectWithTimeLine()
//    {
//        float difference = 999999f;
//        float savedX = 0f;

//        for (int i = 0; i < nearSlot.length; i++)
//        {
//            if (Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x) < difference)
//            {
//                savedX = nearSlot.gridCenters[i].x;
//                difference = Math.Abs(nearSlot.gridCenters[i].x - rectTransform.position.x);
//            }
//            else break;
//        }

//        if (savedX == 0) return;

//        float startPosX = nearSlot.rectTransform.position.x - nearSlot.rectTransform.sizeDelta.x / 2;
//        float endPosX = nearSlot.rectTransform.position.x + nearSlot.rectTransform.sizeDelta.x / 2;

//        if (savedX - rectTransform.sizeDelta.x / 2 < startPosX)
//        {
//            for (int i = 0; i < nearSlot.length; i++)
//            {
//                if (nearSlot.gridCenters[i].x <= startPosX + rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }
//        else if (savedX + rectTransform.sizeDelta.x / 2 > endPosX)
//        {
//            for (int i = nearSlot.length - 1; i >= 0; i--)
//            {
//                if (nearSlot.gridCenters[i].x >= endPosX - rectTransform.sizeDelta.x / 2) savedX = nearSlot.gridCenters[i].x;
//                else break;
//            }
//        }

//        rectTransform.position = new Vector3(savedX, nearSlot.gridCenters[0].y, nearSlot.gridCenters[0].z);

//    }



//    private void OnTriggerStay2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            nearSlot = other.GetComponent<TimeLine>();
//            isInLine = true;

//            if (!isBeingHeld)
//            {
//                if(nearSlot != null)
//                {
//                    AlignObjectWithTimeLine();
//                    FindStartEndIdx();
//                    originPos = rectTransform.position;
//                }



//                //if(nearSlot.feb != null)
//                //{
//                //    FieldEffectBlock otherBlock = nearSlot.feb;
//                //    Vector3 tempPos = otherBlock.rectTransform.position;
//                //    otherBlock.rb2.MovePosition(tempPos);
//                //    rb2.MovePosition(tempPos);

//                //}


//            }

//            //if(isSet)
//            //{
//            //    if (!isBeingHeld)
//            //    {
//            //        //nearSlot.feb = this;
//            //        AlignObjectWithTimeLine();
//            //        FindStartEndIdx();
//            //        originPos = rectTransform.position;
//            //    }
//            //}

//                //FieldEffectBlock otherBlock = nearSlot.feb;
//                //Vector3 tempPos = otherBlock.rectTransform.position;
//                //otherBlock.rb2.MovePosition(tempPos);
//                //rb2.MovePosition(tempPos);
//                //originPos = rectTransform.position;

//                //if (slot.haveBlock) isInLine = false;
//                //else isInLine = true;

//        }
//    }

//    private void OnTriggerExit2D(Collider2D other)
//    {
//        if (other.CompareTag("TimeLine"))
//        {
//            isInLine = false;
//            nearSlot = null;
//        }
//    }

//    public void OnPointerDown(PointerEventData eventData)
//    {
//    }

//    public void OnDrop(PointerEventData eventData)
//    {
//    }


//}
