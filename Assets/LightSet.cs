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

    public GameObject spotlightPrefab; // ����Ʈ����Ʈ ������
                                       //private GameObject currentSpotlight; // ���� ������ ����Ʈ����Ʈ

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
        // ������ ������ ����Ʈ����Ʈ�� �ִٸ� �����մϴ�.
        //if (spotlightPrefab != null)
        //{
        //    Destroy(spotlightPrefab);
        //}



        // ����Ʈ����Ʈ �������� �ν��Ͻ�ȭ �մϴ�.
        spotlightPrefab = Instantiate(spotlightPrefab, spotlightPrefab.transform.position, spotlightPrefab.transform.rotation);
    }

    public void DetectSpotlightArea(Vector3 objectPosition)
    {

        if (spotlightPrefab == null) return;

        Light spotLightComponent = spotlightPrefab.GetComponent<Light>();

        if (spotLightComponent == null || spotLightComponent.type != UnityEngine.LightType.Spot) return;

        // ����Ʈ����Ʈ�� ��ġ�� ����
        Vector3 spotlightPosition = spotlightPrefab.transform.position;
        Vector3 spotlightDirection = spotlightPrefab.transform.forward;


        // ����Ʈ����Ʈ�� �ݰ�� ����
        float spotAngle = spotLightComponent.spotAngle / 2.0f;
        float spotRange = spotLightComponent.range;

        // ���� ������Ʈ�� ����Ʈ����Ʈ�� ���� ���� �ִ��� üũ
        Vector3 toObject = objectPosition - spotlightPosition;
        float distanceToObject = toObject.magnitude;

        // ���� ���� �ִ��� Ȯ��
        if (distanceToObject <= spotRange)
        {
            // ���� ���� �ִ��� Ȯ��
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
