using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObstacle : MonoBehaviour
{
    public int health { get; private set; }
    public string name { get; private set; }

    public void Initialize(int health, string name)
    {
        this.health = health;
        this.name = name;
        Debug.Log("Player initialized with health: " + health + " and name: " + name);
    }



    //public static event Action<bool> inPlayerShotLight;


    ////public GameObject spotlightPrefab; // 스포트라이트 프리팹

    //void Start()
    //{
    //    createSpotlight();


    //}

    //void Update()
    //{
    //    DetectSpotlightArea();
    //}

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
