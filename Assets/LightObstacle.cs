using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class LightObstacle : MonoBehaviour
{
    //private GameObject obj;
    //public int health { get; private set; }
    //public string name { get; private set; }


    // 비활성화, 깜빡거림, 일부분 그림자 

    //일정시간마다 새가 버프 혹은 공겨지원, 키누르면 일직선으로 공격?
    //스킬봉인

    public static bool weakening;


    public LightCondition lcd;


    public static float minusDamage = 0.5f;


    public static string naming;

    //LightCondition lcd = LightCondition.Blink;

    //public static int startTime;
    public static int endTime;


    public int startTime { get; private set; }


    public void setTime(int start, int end)
    {
        startTime = start;
        endTime = end;
    }


    public void Start()
    {
        moving = Vector3.zero;

        //naming = gameObject.name;

        ////임시로 순서대로
        //if(FieldEffectPopUpManager.instance.blocks.Count > 0 ) 
        //{
        //    FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[0].GetComponent<FieldEffectBlock>();
        //    startTime = feb.start;
        //    endTime = feb.end;
        //    StageManager.instance.idx++;
        //}

    }


    private void Awake()
    {
        naming = gameObject.transform.parent.name;

        //naming = gameObject.name;

        //startTime = StageManager.instance.startTime;
        //endTime = StageManager.instance.endTime;

    }




    //public void Initialize(LevelConstructSet LC, int idx, Vector3 vec)
    //{
    //    obj = Instantiate(LC.lightObstacle[idx], vec, LC.lightObstacle[idx].transform.rotation);
    //    obj.SetActive(false);
    //}

    // 적이 배회하게


    //public static event Action<bool> inPlayerShotLight;


    ////public GameObject spotlightPrefab; // 스포트라이트 프리팹

    //void Start()
    //{
    //    createSpotlight();


    //}


    private Vector3 moving;

    Coroutine FieldEffectCoroutine = null;


    void Update()
    {

        if (StageManager.instance.waveTime == startTime)
        {
            if (FieldEffectCoroutine == null)
            {
                switch (gameObject.tag)
                {
                    case ("Blink"):

                        FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));

                        //blinking();
                        break;

                    case ("Shadow"):

                        FieldEffectCoroutine = StartCoroutine(shadowEffect(startTime, endTime));

                        break;

                    case ("Disable(Clone)"):
                        break;
                }

                //switch (naming)
                //{
                //    case ("Blink(Clone)"):

                //        FieldEffectCoroutine = StartCoroutine(blinkEffect(startTime, endTime));

                //        //blinking();
                //        break;

                //    case ("Shadow(Clone)"):

                //        FieldEffectCoroutine = StartCoroutine(shadowEffect(startTime, endTime));

                //        break;

                //    case ("Disable(Clone)"):
                //        break;
                //}
            }
            //half


        }

        if (moving != Vector3.zero)
        {
            GameObject go = transform.GetChild(0).gameObject;
            Vector3 newPos = go.transform.position + moving * Time.fixedDeltaTime;
            go.transform.position = newPos;
        }


        

        DetectSpotlightArea();
    }

    private IEnumerator shadowEffect(int start, int end)
    {
        moving = new Vector3(50,0,0);
        yield return new WaitForSeconds(end - start);
        FieldEffectCoroutine = null;
        //GameObject go = transform.GetChild(0).gameObject;

        //Vector3 vector3 = new Vector3(10, 0, 0);
        ////vector3 *= Time.deltaTime;

        //yield return new WaitForSeconds(end - start);
        //go.transform.position = go.transform.position + vector3;

        //transform.GetChild(0).gameObject.SetActive(false);
        //yield return new WaitForSeconds(end - start);
        //transform.GetChild(0).gameObject.SetActive(true);
        //FieldEffectCoroutine = null;
    }



    private IEnumerator blinkEffect(int start, int end)
    {
        transform.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(end - start);
        transform.GetChild(0).gameObject.SetActive(true);
        FieldEffectCoroutine = null;
    }





    Coroutine BlinkingCoroutine = null;

    void blinking()
    {
        if(BlinkingCoroutine == null)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            BlinkingCoroutine = StartCoroutine(blinkingIe());
        }
    }

    private IEnumerator blinkingIe()
    {
        yield return new WaitForSeconds(3f);
        transform.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        BlinkingCoroutine = null;


        //yield return new WaitForSecondsRealtime(3f);
        //transform.GetChild(0).gameObject.SetActive(true);
        //yield return new WaitForSecondsRealtime(3f);
        //BlinkingCoroutine = null;
    }


    public void DetectSpotlightArea()
    {

        Light spotLightComponent = GetComponent<Light>();

        // 스포트라이트의 위치와 방향
        Vector3 spotlightPosition = transform.position;
        Vector3 spotlightDirection = transform.forward;


        // 스포트라이트의 반경과 각도
        float spotAngle = spotLightComponent.spotAngle / 2.0f;
        float spotRange = spotLightComponent.range;

        // 게임 오브젝트가 스포트라이트의 범위 내에 있는지 체크
        Vector3 toObject = StageManager.player.transform.position - spotlightPosition;
        float distanceToObject = toObject.magnitude;

        // 범위 내에 있는지 확인
        if (distanceToObject <= spotRange)
        {
            // 각도 내에 있는지 확인
            float angleToObject = Vector3.Angle(spotlightDirection, toObject);
            if (angleToObject <= spotAngle)
            {
                //Debug.Log("Object is within the spotlight area: " + transform.name);

                EventManager.instance.playerEnterTherLight();
            }

            //else
            //{
            //    EventManager.instance.playerEnterTherLight(false);
            //}
        }
    }




















    //public void createSpotlight()
    //{
    //    // 이전에 생성된 스포트라이트가 있다면 삭제합니다.
    //    //if (spotlightPrefab != null)
    //    //{
    //    //    Destroy(spotlightPrefab);
    //    //}



    //    // 스포트라이트 프리팹을 인스턴스화 합니다.
    //    spotlightPrefab = Instantiate(spotlightPrefab, spotlightPrefab.transform.position, spotlightPrefab.transform.rotation);
    //}

    //public void DetectSpotlightArea()
    //{

    //    Light spotLightComponent = GetComponent<Light>();

    //    // 스포트라이트의 위치와 방향
    //    Vector3 spotlightPosition = transform.position;
    //    Vector3 spotlightDirection = transform.forward;


    //    // 스포트라이트의 반경과 각도
    //    float spotAngle = spotLightComponent.spotAngle / 2.0f;
    //    float spotRange = spotLightComponent.range;

    //    // 게임 오브젝트가 스포트라이트의 범위 내에 있는지 체크
    //    Vector3 toObject = StageManager.players[StageManager.currentPlayerIdx].transform.position - spotlightPosition;
    //    float distanceToObject = toObject.magnitude;

    //    // 범위 내에 있는지 확인
    //    if (distanceToObject <= spotRange)
    //    {
    //        // 각도 내에 있는지 확인
    //        float angleToObject = Vector3.Angle(spotlightDirection, toObject);
    //        if (angleToObject <= spotAngle)
    //        {
    //            //Debug.Log("Object is within the spotlight area: " + transform.name);

    //            EventManager.instance.playerEnterTherLight();
    //        }

    //        //else
    //        //{
    //        //    EventManager.instance.playerEnterTherLight(false);
    //        //}
    //    }
    //}
}
