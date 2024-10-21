using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class FieldEffect : MonoBehaviour, Spawn
{

    Vector3[] _gridCenters;
    Transform _mapTransform;






    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {

        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }














    private int delayEffectAmount = 2;
    private int damage = 2;
    private int slow = 2;
    private int seal = 2;
    private float speed = 15f;

    private int endTime;
    private int startTime;
    private int gridIdx;

    public int m_upperIdx;
    public int m_downerIdx;

    private GameObject EffectRange;

    private GameObject EffectTime;

    private Slider EffectTimer;

    private Slider EffectActiveTimer;

    public void Start()
    {
        EffectRange = transform.Find("EffectRange").gameObject;




        //EffectRange = GameObject.Find("EffectRange");


        var asd = transform.Find("EffectCoolDownUI");

        if(asd != null) 
        {
            //asd = 



            GameObject go = transform.Find("EffectCoolDownUI").gameObject;


            //GameObject go1 = go.transform.Find("EffectCoolDown").gameObject;

            EffectTime = go.transform.Find("EffectCoolDown").gameObject;
            //EffectTime.GetComponent<Slider>();

            EffectTimer = EffectTime.GetComponent<Slider>();

            EffectActiveTimer = go.transform.Find("EffectActiveCoolDown").gameObject.GetComponent<Slider>();
        }



        RangeMaterial = EffectRange.gameObject.GetComponent<Renderer>().material;

        RangeMaterial.SetFloat("_curState", 0.1f);

        //RangeMaterial

        //EffectRange = transform.GetChild(0).gameObject;
    }

    public void Init(int start, int end, int idx, int upperIdx, int downerIdx)
    {
        m_upperIdx = upperIdx;
        m_downerIdx = downerIdx;

        startTime = start;
        endTime = end;
        gridIdx = idx;
    }


    public Material RangeMaterial;

    //private Vector3 moving;

    Coroutine FieldEffectCoroutine = null;

    private IEnumerator StartEffectTimer(float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            EffectTimer.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        EffectTimer.value = 0;
        //EffectTimer = null;



        elapsed = 0f;
        while (elapsed < 5)
        {
            elapsed += Time.deltaTime;

            EffectActiveTimer.value = Mathf.Clamp01(elapsed / duration);
            yield return null;
        }
        EffectActiveTimer.value = 0;

    }





    void Update()
    {
        if(StageManager.Sstate == StageState.Play)
        {
            //�̰ɷ��������� �̺�Ʈ�� �ϸ� �������?
            if(StageManager.instance.waveTime == startTime)
            {
                if (FieldEffectCoroutine == null)
                {
                    switch (m_upperIdx)
                    {
                        case (0): //0 Blink
                            FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));
                            //EffectRange.SetActive(true);
                            //RangeMaterial.SetFloat("_curState", 1f);

                            //FieldEffectCoroutine = StartCoroutine(EffectStart(startTime, endTime, () =>
                            //{
                            //    EffectRange.SetActive(false);
                            //    RangeMaterial.SetFloat("_curState", 0.2f);
                            //    FieldEffectCoroutine = null;
                            //}));


                            break;

                        case (1): //1 Delay

                            //EffectRange.SetActive(true);
                            //RangeMaterial.SetFloat("_curState", 1f);


                            FieldEffectCoroutine = StartCoroutine(Delay(startTime, endTime));



                            
                            break;

                        case (2): //2 Moving
                            FieldEffectCoroutine = StartCoroutine(moveRange(startTime, endTime, gridIdx));





                            break;
                    }

                    StartCoroutine(StartEffectTimer(5));

                }

                

            }



        }

    }

    //private IEnumerator EffectStart(int start, int end, Action act)
    //{

    //    //EffectRange.SetActive(true);
    //    yield return new WaitForSeconds(end - start);

    //    act();

    //    //EffectRange.SetActive(false);



    //    //FieldEffectCoroutine = null;
    //}







    private IEnumerator Delay(int start, int end)
    {
        EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = true;

        RangeMaterial.SetFloat("_curState", 1f);

        //yield return new WaitForSeconds(end - start);
        yield return new WaitForSeconds(Math.Abs(end - start));
        EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = false;


        RangeMaterial.SetFloat("_curState", 0.2f);

        FieldEffectCoroutine = null;
    }

    private IEnumerator moveRange(int start, int end, int idx)
    {
        RangeMaterial.SetFloat("_curState", 1f);

        // �̵� �ݰ� ����
        float radius = 20;
        float boundaryLeft = StageManager.instance.gridCenters[idx].x - radius;
        float boundaryRight = StageManager.instance.gridCenters[idx].x + radius;
        float boundaryTop = StageManager.instance.gridCenters[idx].z + radius;
        float boundaryBottom = StageManager.instance.gridCenters[idx].z - radius;

        while (end != StageManager.instance.waveTime)
        {
            // ���� ��ġ�� ����
            Vector3 startPosition = EffectRange.transform.position;

            // ������ ��ǥ ��ġ ���� (�簢�� ��� ������)
            float randXPos = UnityEngine.Random.Range(boundaryLeft, boundaryRight);
            float randZPos = UnityEngine.Random.Range(boundaryBottom, boundaryTop);
            Vector3 targetPosition = new Vector3(randXPos, EffectRange.transform.position.y, randZPos);

            float distance = Vector3.Distance(startPosition, targetPosition);
            float travelPercent = 0f;

            // �̵� �Ϸ��� ������ �ݺ�
            while (travelPercent < 1f)
            {
                if (end == StageManager.instance.waveTime) break;

                // ���� �����ӿ��� �̵��� ���� ���
                travelPercent += speed * Time.deltaTime / distance;

                // �� ��ġ ���
                Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, travelPercent);

                // ��� ���� �� ƨ���
                if (newPos.x <= boundaryLeft || newPos.x >= boundaryRight)
                {
                    speed = -speed; // �ӵ� ����
                    ApplyRandomDirection(ref newPos, boundaryLeft, boundaryRight, boundaryTop, boundaryBottom); // ���� ���� ����
                }
                if (newPos.z <= boundaryBottom || newPos.z >= boundaryTop)
                {
                    speed = -speed; // �ӵ� ����
                    ApplyRandomDirection(ref newPos, boundaryLeft, boundaryRight, boundaryTop, boundaryBottom); // ���� ���� ����
                }

                // ���ο� ��ġ�� ����
                EffectRange.transform.position = newPos;

                // ���� �����ӱ��� ���
                yield return null;
            }
        }

        RangeMaterial.SetFloat("_curState", 0.2f);
        FieldEffectCoroutine = null;
    }

    // ������ ������ �����ϴ� �Լ�
    void ApplyRandomDirection(ref Vector3 currentPosition, float boundaryLeft, float boundaryRight, float boundaryTop, float boundaryBottom)
    {
        float randomAngle = UnityEngine.Random.Range(-30f, 30f); // ���� ����
        float cosAngle = Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        float sinAngle = Mathf.Sin(randomAngle * Mathf.Deg2Rad);

        // X��, Z�� ������ �����ϰ� ����
        float newX = currentPosition.x * cosAngle - currentPosition.z * sinAngle;
        float newZ = currentPosition.x * sinAngle + currentPosition.z * cosAngle;

        // ��踦 ���� �ʵ��� ����
        newX = Mathf.Clamp(newX, boundaryLeft, boundaryRight);
        newZ = Mathf.Clamp(newZ, boundaryBottom, boundaryTop);

        currentPosition = new Vector3(newX, currentPosition.y, newZ);
    }

    //private IEnumerator moveRange(int start, int end, int idx)
    //{
    //    RangeMaterial.SetFloat("_curState", 1f);

    //    while (end != StageManager.instance.waveTime) 
    //    {

    //        // ���� ��ġ�� ����
    //        Vector3 startPosition = EffectRange.transform.position;


    //        float radius = 10;
    //        float randXPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].x, radius + StageManager.instance.gridCenters[idx].x);
    //        float randZPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].z, radius + StageManager.instance.gridCenters[idx].z);
    //        Vector3 targetPosition = new Vector3(randXPos, EffectRange.transform.position.y, randZPos);

    //        float distance = Vector3.Distance(startPosition, targetPosition);
    //        float travelPercent = 0f;

    //        // �̵� �Ϸ��� ������ �ݺ�
    //        while (travelPercent < 1f)
    //        {
    //            if (end == StageManager.instance.waveTime) break;

    //            // ���� �����ӿ��� �̵��� ���� ���
    //            travelPercent += speed * Time.deltaTime / distance;

    //            // �� ��ġ ���
    //            Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, travelPercent);

    //            EffectRange.transform.position = newPos;

    //            // ���� �����ӱ��� ���
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


    private IEnumerator blinkEffect(int start, int end)
    {

        EffectRange.SetActive(true);
        RangeMaterial.SetFloat("_curState", 1f);
        yield return new WaitForSeconds(Math.Abs(end - start));


        EffectRange.SetActive(false);
        RangeMaterial.SetFloat("_curState", 0.2f);


        FieldEffectCoroutine = null;
    }

    private IEnumerator swampEffect(int start, int end)
    {
        EffectRange.SetActive(true);

        yield return new WaitForSeconds(end - start);

        EffectRange.SetActive(false);
        FieldEffectCoroutine = null;
    }

    public bool IsActivated()
    {
        if (FieldEffectCoroutine == null)
        {
            return false;
        }

        else return true;
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
