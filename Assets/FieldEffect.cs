using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FieldEffect : MonoBehaviour
{
    private int delayEffectAmount = 2;
    private int damage = 2;
    private int slow = 2;
    private int seal = 2;
    private float speed = 3f;

    private int endTime;
    private int startTime;
    private int gridIdx;

    public int m_upperIdx;
    public int m_downerIdx;

    private GameObject EffectRange;

    private GameObject EffectTime;

    private Slider EffectTimer;



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
        }



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
    }



    void Update()
    {
        if(StageManager.Sstate == StageState.Play)
        {
            //이걸로하지말고 이벤트로 하면 성능향상?
            if(StageManager.instance.waveTime == startTime)
            {
                if (FieldEffectCoroutine == null)
                {
                    switch (m_upperIdx)
                    {
                        case (0): //0 Blink
                            FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));

                            //if (EffectTime != null)
                            //{
                            //    StartCoroutine(StartEffectTimer(5));
                            //}

                            break;

                        case (1): //1 Delay
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

    private IEnumerator Delay(int start, int end)
    {
        EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = true;

        yield return new WaitForSeconds(end - start);
        EffectRange.gameObject.GetComponent<CapsuleCollider>().enabled = false;

        FieldEffectCoroutine = null;
    }


    private IEnumerator moveRange(int start, int end, int idx)
    {
        while(end != StageManager.instance.waveTime) 
        {

            // 현재 위치를 저장
            Vector3 startPosition = EffectRange.transform.position;


            float radius = 10;
            float randXPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].x, radius + StageManager.instance.gridCenters[idx].x);
            float randZPos = UnityEngine.Random.Range(-radius + StageManager.instance.gridCenters[idx].z, radius + StageManager.instance.gridCenters[idx].z);
            Vector3 targetPosition = new Vector3(randXPos, EffectRange.transform.position.y, randZPos);

            float distance = Vector3.Distance(startPosition, targetPosition);
            float travelPercent = 0f;

            // 이동 완료할 때까지 반복
            while (travelPercent < 1f)
            {
                if (end == StageManager.instance.waveTime) break;

                // 현재 프레임에서 이동할 비율 계산
                travelPercent += speed * Time.deltaTime / distance;

                // 새 위치 계산
                Vector3 newPos = Vector3.Lerp(startPosition, targetPosition, travelPercent);

                EffectRange.transform.position = newPos;

                // 다음 프레임까지 대기
                yield return null;
            }



        }



        FieldEffectCoroutine = null;
    }



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
        yield return new WaitForSeconds(end - start);


        EffectRange.SetActive(false);



        FieldEffectCoroutine = null;
    }

    private IEnumerator swampEffect(int start, int end)
    {
        EffectRange.SetActive(true);

        yield return new WaitForSeconds(end - start);

        EffectRange.SetActive(false);
        FieldEffectCoroutine = null;
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
