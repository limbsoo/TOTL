using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

[Serializable]
public class FieldEffect : MonoBehaviour, Spawn
{

    Vector3[] _gridCenters;
    Transform _mapTransform;

    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }

    //BlockInfo blockinfo;


    BlockData m_blockData;


    private GameObject EffectRange;

    private GameObject EffectTime;

    private Slider EffectTimer;

    private Slider EffectActiveTimer;

    private TMP_Text EffectClock;
    private GameObject EffectClockCanvas;

    public void Start()
    {
        EffectRange = transform.Find("EffectRange").gameObject;
        var asd = transform.Find("EffectCoolDownUI");

        if(asd != null) 
        {
            GameObject go = transform.Find("EffectCoolDownUI").gameObject;
            EffectTime = go.transform.Find("EffectCoolDown").gameObject;
            EffectTimer = EffectTime.GetComponent<Slider>();
            EffectActiveTimer = go.transform.Find("EffectActiveCoolDown").gameObject.GetComponent<Slider>();
        }

        EffectClockCanvas = transform.Find("EffectTime").gameObject;
        EffectClock = EffectClockCanvas.transform.Find("Text").GetComponent<TMP_Text>();
        EffectClock.text = m_blockData.start.ToString();

        RangeMaterial = EffectRange.gameObject.GetComponent<Renderer>().material;
        RangeMaterial.SetFloat("_curState", 0.1f);

        //StartCoroutine(StartEffectTimerFirst(m_blockData.start));


    }


    public BlockData GetBlockData()
    {
        return m_blockData;
    }


    public void Init(BlockData bd)
    {
        m_blockData = new BlockData();
        m_blockData = bd;
    }



    public Material RangeMaterial;

    Coroutine FieldEffectCoroutine = null;


    public float stack;

    private IEnumerator StackEffectActive(int start, int end)
    {
        EffectRange.SetActive(true);

        stack = 0;

        RangeMaterial.SetFloat("_curState", 1f);
        if (end > start) { yield return new WaitForSeconds(end - start); }
        else { yield return new WaitForSeconds(10 - end + start); }

        EffectRange.SetActive(false);
        RangeMaterial.SetFloat("_curState", 0.2f);
        FieldEffectCoroutine = null;

        stack = 0;
    }



    private IEnumerator EffectActive(float duration)
    {
        EffectRange.SetActive(true);
        EffectClock.text = SetTextColor("red",m_blockData.end.ToString());


        stack = 0;

        RangeMaterial.SetFloat("_curState", 1f);

        yield return new WaitForSeconds(duration);

        EffectRange.SetActive(false);
        RangeMaterial.SetFloat("_curState", 0.2f);
        FieldEffectCoroutine = null;

        EffectClock.text = m_blockData.start.ToString();

        stack = 0;
    }

    public string SetTextColor(string color, string value)
    {
        string s = "";
        s += "<color=";
        s += color;
        s += ">";
        s += value;
        s += "</color>";
        return s;
    }


    void Update()
    {
        if (StageManager.Sstate == StageState.Play)
        {
            //이걸로하지말고 이벤트로 하면 성능향상?
            if (StageManager.instance.waveTime == m_blockData.start)
            {
                if (FieldEffectCoroutine == null)
                {
                    switch (m_blockData.fieldKinds)
                    {
                        case (FieldKinds.Long):
                            FieldEffectCoroutine = StartCoroutine(EffectActive(m_blockData.fieldDuration));
                            break;

                        case (FieldKinds.Stack): 
                            FieldEffectCoroutine = StartCoroutine(EffectActive(m_blockData.fieldDuration));
                            break;

                        case (FieldKinds.Move): //2 Moving
                            FieldEffectCoroutine = StartCoroutine(moveRange(m_blockData.start, m_blockData.end, m_blockData.lineNum));
                            break;
                    }
                    //StartCoroutine(StartEffectTimer(5));
                }
            }



        }











        //if(StageManager.Sstate == StageState.Play)
        //{
        //    //이걸로하지말고 이벤트로 하면 성능향상?
        //    if(StageManager.instance.waveTime == m_blockData.start)
        //    {
        //        if (FieldEffectCoroutine == null)
        //        {
        //            switch (m_blockData.fieldKinds)
        //            {
        //                case (FieldKinds.Long): //0 Blink




        //                    FieldEffectCoroutine = StartCoroutine(blinkEffect(m_blockData.start, m_blockData.end));
        //                    //EffectRange.SetActive(true);
        //                    //RangeMaterial.SetFloat("_curState", 1f);

        //                    //FieldEffectCoroutine = StartCoroutine(EffectStart(startTime, endTime, () =>
        //                    //{
        //                    //    EffectRange.SetActive(false);
        //                    //    RangeMaterial.SetFloat("_curState", 0.2f);
        //                    //    FieldEffectCoroutine = null;
        //                    //}));


        //                    break;

        //                case (1): //1 Delay

        //                    //EffectRange.SetActive(true);
        //                    //RangeMaterial.SetFloat("_curState", 1f);


        //                    FieldEffectCoroutine = StartCoroutine(Delay(m_blockData.start, m_blockData.end));




        //                    break;

        //                case (2): //2 Moving
        //                    FieldEffectCoroutine = StartCoroutine(moveRange(m_blockData.start, m_blockData.end, m_blockData.lineNum));





        //                    break;
        //            }

        //            StartCoroutine(StartEffectTimer(5));

        //        }



        //    }



        //}

    }

    //private IEnumerator EffectStart(int start, int end, Action act)
    //{

    //    //EffectRange.SetActive(true);
    //    yield return new WaitForSeconds(end - start);

    //    act();

    //    //EffectRange.SetActive(false);



    //    //FieldEffectCoroutine = null;
    //}







    //private IEnumerator Delay(int start, int end)
    //{
    //    EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = true;

    //    RangeMaterial.SetFloat("_curState", 1f);

    //    //yield return new WaitForSeconds(end - start);
    //    yield return new WaitForSeconds(Math.Abs(end - start));
    //    EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = false;


    //    RangeMaterial.SetFloat("_curState", 0.2f);

    //    FieldEffectCoroutine = null;
    //}

    private IEnumerator moveRange(float start, float end, int idx)
    {
        RangeMaterial.SetFloat("_curState", 1f);
        EffectRange.SetActive(true);
        GameObject go = transform.Find("EffectTime").gameObject;

        //EffectClock.text = m_blockData.end.ToString();
        EffectClock.text = SetTextColor("red", m_blockData.end.ToString());

        // 이동 반경 설정
        float radius = 20;
        float boundaryLeft = StageManager.instance.ReturnGridCenters()[idx].x - radius;
        float boundaryRight = StageManager.instance.ReturnGridCenters()[idx].x + radius;
        float boundaryTop = StageManager.instance.ReturnGridCenters()[idx].z + radius;
        float boundaryBottom = StageManager.instance.ReturnGridCenters()[idx].z - radius;

        while (end != StageManager.instance.waveTime)
        {
            // 현재 위치를 저장
            Vector3 startPosition = EffectRange.transform.position;

            // 랜덤한 목표 위치 설정 (사각형 경계 내에서)
            float randXPos = UnityEngine.Random.Range(boundaryLeft, boundaryRight);
            float randZPos = UnityEngine.Random.Range(boundaryBottom, boundaryTop);
            Vector3 targetPosition = new Vector3(randXPos, EffectRange.transform.position.y, randZPos);

            float distance = Vector3.Distance(startPosition, targetPosition);
            float travelPercent = 0f;

            // 이동 완료할 때까지 반복
            while (travelPercent < 1f)
            {
                if (end == StageManager.instance.waveTime) break;

                // 현재 프레임에서 이동할 비율 계산
                travelPercent += m_blockData.fieldValue * Time.deltaTime / distance;

                // 새 위치 계산
                Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, travelPercent);

                // 경계 감지 후 튕기기
                if (newPos.x <= boundaryLeft || newPos.x >= boundaryRight)
                {
                    m_blockData.fieldValue = -m_blockData.fieldValue; // 속도 반전
                    ApplyRandomDirection(ref newPos, boundaryLeft, boundaryRight, boundaryTop, boundaryBottom); // 랜덤 방향 적용
                }
                if (newPos.z <= boundaryBottom || newPos.z >= boundaryTop)
                {
                    m_blockData.fieldValue = -m_blockData.fieldValue; // 속도 반전
                    ApplyRandomDirection(ref newPos, boundaryLeft, boundaryRight, boundaryTop, boundaryBottom); // 랜덤 방향 적용
                }

                // 새로운 위치를 적용
                EffectRange.transform.position = newPos;

                EffectClockCanvas.transform.position = new Vector3(newPos.x, EffectClockCanvas.transform.position.y, newPos.z);

                // 다음 프레임까지 대기
                yield return null;
            }
        }

        RangeMaterial.SetFloat("_curState", 0.2f);
        EffectRange.SetActive(false);
        EffectClock.text = m_blockData.start.ToString();

        FieldEffectCoroutine = null;
    }

    // 랜덤한 방향을 적용하는 함수
    void ApplyRandomDirection(ref Vector3 currentPosition, float boundaryLeft, float boundaryRight, float boundaryTop, float boundaryBottom)
    {
        float randomAngle = UnityEngine.Random.Range(-30f, 30f); // 랜덤 각도
        float cosAngle = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        // X축, Z축 방향을 랜덤하게 조정
        float newX = currentPosition.x * cosAngle - currentPosition.z * sinAngle;
        float newZ = currentPosition.x * sinAngle + currentPosition.z * cosAngle;

        // 경계를 넘지 않도록 조정
        newX = Mathf.Clamp(newX, boundaryLeft, boundaryRight);
        newZ = Mathf.Clamp(newZ, boundaryBottom, boundaryTop);

        currentPosition = new Vector3(newX, currentPosition.y, newZ);
    }

    //private IEnumerator moveRange(int start, int end, int idx)
    //{
    //    RangeMaterial.SetFloat("_curState", 1f);

    //    while (end != StageManager.instance.waveTime) 
    //    {

    //        // 현재 위치를 저장
    //        Vector3 startPosition = EffectRange.transform.position;


    //        float radius = 10;
    //        float randXPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].x, radius + StageManager.instance.gridCenters[idx].x);
    //        float randZPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].z, radius + StageManager.instance.gridCenters[idx].z);
    //        Vector3 targetPosition = new Vector3(randXPos, EffectRange.transform.position.y, randZPos);

    //        float distance = Vector3.Distance(startPosition, targetPosition);
    //        float travelPercent = 0f;

    //        // 이동 완료할 때까지 반복
    //        while (travelPercent < 1f)
    //        {
    //            if (end == StageManager.instance.waveTime) break;

    //            // 현재 프레임에서 이동할 비율 계산
    //            travelPercent += speed * Time.deltaTime / distance;

    //            // 새 위치 계산
    //            Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, travelPercent);

    //            EffectRange.transform.position = newPos;

    //            // 다음 프레임까지 대기
    //            yield return null;
    //        }



    //    }


    //    RangeMaterial.SetFloat("_curState", 0.2f);
    //    FieldEffectCoroutine = null;
    //}



    private IEnumerator shadowEffect(int start, int end, int idx)
    {

        //EffectRange.transform.position = new Vector3(randXPos, transform.position.y, randZPos);






        //moving = new Vector3(50, 0, 0);
        yield return new WaitForSeconds(end - start);
        FieldEffectCoroutine = null;



        //EffectRange.SetActive(true);

        //float radius = 10;
        //float randXPos = UnityEngine.Random.Range(- radius + StageManager.instance.gridCenters[idx].x, radius + StageManager.instance.gridCenters[idx].x);
        //float randZPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].z, radius + StageManager.instance.gridCenters[idx].z);

        //EffectRange.transform.position = new Vector3(randXPos,transform.position.y, randZPos);


        ////moving = new Vector3(50, 0, 0);
        //yield return new WaitForSeconds(end - start);
        //EffectRange.SetActive(false);
        //FieldEffectCoroutine = null;
    }


    //private IEnumerator blinkEffect(int start, int end)
    //{
    //    EffectRange.SetActive(true);
    //    RangeMaterial.SetFloat("_curState", 1f);
    //    yield return new WaitForSeconds(Math.Abs(end - start));


    //    EffectRange.SetActive(false);
    //    RangeMaterial.SetFloat("_curState", 0.2f);


    //    FieldEffectCoroutine = null;
    //}

    //private IEnumerator swampEffect(int start, int end)
    //{
    //    EffectRange.SetActive(true);

    //    yield return new WaitForSeconds(end - start);

    //    EffectRange.SetActive(false);
    //    FieldEffectCoroutine = null;
    //}

    public bool IsActivated()
    {
        if (FieldEffectCoroutine == null) { return false; }
        else { return true; }
    }



    //Coroutine BlinkingCoroutine = null;

    //void blinking()
    //{
    //    if (BlinkingCoroutine == null)
    //    {
    //        transform.GetChild(0).gameObject.SetActive(false);
    //        BlinkingCoroutine = StartCoroutine(blinkingIe());
    //    }
    //}

    //private IEnumerator blinkingIe()
    //{
    //    yield return new WaitForSeconds(3f);
    //    transform.GetChild(0).gameObject.SetActive(true);
    //    yield return new WaitForSeconds(3f);
    //    BlinkingCoroutine = null;


    //    //yield return new WaitForSecondsRealtime(3f);
    //    //transform.GetChild(0).gameObject.SetActive(true);
    //    //yield return new WaitForSecondsRealtime(3f);
    //    //BlinkingCoroutine = null;
    //}



}
