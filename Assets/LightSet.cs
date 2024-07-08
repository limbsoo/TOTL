using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightSet : MonoBehaviour
{
    public static event Action<bool> inPlayerShotLight;


    //public static Action lightSet;

    //private void Awake()
    //{
    //    lightSet = () =>
    //    {
    //        DetectSpotlightArea(new Vector3(0, 0, 0));
    //    };
    //}

    public GameObject spotlightPrefab; // 스포트라이트 프리팹
                                       //private GameObject currentSpotlight; // 현재 생성된 스포트라이트

    private void Awake()
    {
        if (GameSet.lightInstance == null) GameSet.lightInstance = this;
    }


    void Start()
    {
        //CreateSpotlight();


    }

    void Update()
    {
        //DetectSpotlightArea();
    }

    public void createSpotlight()
    {
        // 이전에 생성된 스포트라이트가 있다면 삭제합니다.
        //if (spotlightPrefab != null)
        //{
        //    Destroy(spotlightPrefab);
        //}



        // 스포트라이트 프리팹을 인스턴스화 합니다.
        spotlightPrefab = Instantiate(spotlightPrefab, spotlightPrefab.transform.position, spotlightPrefab.transform.rotation);
    }

    public void DetectSpotlightArea(Vector3 objectPosition)
    {

        if (spotlightPrefab == null) return;

        Light spotLightComponent = spotlightPrefab.GetComponent<Light>();

        if (spotLightComponent == null || spotLightComponent.type != UnityEngine.LightType.Spot) return;

        // 스포트라이트의 위치와 방향
        Vector3 spotlightPosition = spotlightPrefab.transform.position;
        Vector3 spotlightDirection = spotlightPrefab.transform.forward;


        // 스포트라이트의 반경과 각도
        float spotAngle = spotLightComponent.spotAngle / 2.0f;
        float spotRange = spotLightComponent.range;

        // 게임 오브젝트가 스포트라이트의 범위 내에 있는지 체크
        Vector3 toObject = objectPosition - spotlightPosition;
        float distanceToObject = toObject.magnitude;

        // 범위 내에 있는지 확인
        if (distanceToObject <= spotRange)
        {
            // 각도 내에 있는지 확인
            float angleToObject = Vector3.Angle(spotlightDirection, toObject);
            if (angleToObject <= spotAngle)
            {
                Debug.Log("Object is within the spotlight area: " + transform.name);

                inPlayerShotLight?.Invoke(true);
            }

            else
            {
                inPlayerShotLight?.Invoke(false);
            }
        }
    }

}
