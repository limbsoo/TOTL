using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObstacle : MonoBehaviour
{
    //private GameObject obj;
    //public int health { get; private set; }
    //public string name { get; private set; }


    // 비활성화, 깜빡거림, 일부분 그림자 

    //일정시간마다 새가 버프 혹은 공겨지원, 키누르면 일직선으로 공격?

    public static bool weakening;


    public LightCondition lcd;


    public static float minusDamage = 0.5f;


    public static string naming;

    //LightCondition lcd = LightCondition.Blink;



    public void Start()
    {
        naming = gameObject.name;
    }


    private void Awake()
    {

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

    void Update()
    {

        switch (naming)
        {
            case ("blink"):
                blinking();
                break;

            case ("shadow"):
                break;

            case ("disable"):
                break;
        }


        

        DetectSpotlightArea();
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
