using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightObstacle : MonoBehaviour
{
    //private GameObject obj;
    //public int health { get; private set; }
    //public string name { get; private set; }


    // ��Ȱ��ȭ, �����Ÿ�, �Ϻκ� �׸��� 

    //�����ð����� ���� ���� Ȥ�� ��������, Ű������ ���������� ����?

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

    // ���� ��ȸ�ϰ�


    //public static event Action<bool> inPlayerShotLight;


    ////public GameObject spotlightPrefab; // ����Ʈ����Ʈ ������

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

        // ����Ʈ����Ʈ�� ��ġ�� ����
        Vector3 spotlightPosition = transform.position;
        Vector3 spotlightDirection = transform.forward;


        // ����Ʈ����Ʈ�� �ݰ�� ����
        float spotAngle = spotLightComponent.spotAngle / 2.0f;
        float spotRange = spotLightComponent.range;

        // ���� ������Ʈ�� ����Ʈ����Ʈ�� ���� ���� �ִ��� üũ
        Vector3 toObject = StageManager.player.transform.position - spotlightPosition;
        float distanceToObject = toObject.magnitude;

        // ���� ���� �ִ��� Ȯ��
        if (distanceToObject <= spotRange)
        {
            // ���� ���� �ִ��� Ȯ��
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
    //    // ������ ������ ����Ʈ����Ʈ�� �ִٸ� �����մϴ�.
    //    //if (spotlightPrefab != null)
    //    //{
    //    //    Destroy(spotlightPrefab);
    //    //}



    //    // ����Ʈ����Ʈ �������� �ν��Ͻ�ȭ �մϴ�.
    //    spotlightPrefab = Instantiate(spotlightPrefab, spotlightPrefab.transform.position, spotlightPrefab.transform.rotation);
    //}

    //public void DetectSpotlightArea()
    //{

    //    Light spotLightComponent = GetComponent<Light>();

    //    // ����Ʈ����Ʈ�� ��ġ�� ����
    //    Vector3 spotlightPosition = transform.position;
    //    Vector3 spotlightDirection = transform.forward;


    //    // ����Ʈ����Ʈ�� �ݰ�� ����
    //    float spotAngle = spotLightComponent.spotAngle / 2.0f;
    //    float spotRange = spotLightComponent.range;

    //    // ���� ������Ʈ�� ����Ʈ����Ʈ�� ���� ���� �ִ��� üũ
    //    Vector3 toObject = StageManager.players[StageManager.currentPlayerIdx].transform.position - spotlightPosition;
    //    float distanceToObject = toObject.magnitude;

    //    // ���� ���� �ִ��� Ȯ��
    //    if (distanceToObject <= spotRange)
    //    {
    //        // ���� ���� �ִ��� Ȯ��
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
