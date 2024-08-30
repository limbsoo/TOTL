using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Progress;

public class Function : MonoBehaviour
{
    public static Function instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public IEnumerator CountDownPerTime(bool b, float f)
    {
        while(f > 0)
        {
            f--;
            yield return new WaitForSecondsRealtime(1);
        }

        b = !b;
    }

    public IEnumerator CountDown(float delay, Action act)
    {
        yield return new WaitForSeconds(delay);
        act();
    }

    public IEnumerator CountDownRealTime(float delay, Action act)
    {
        yield return new WaitForSecondsRealtime(delay);
        act();
    }

    //internal IEnumerator CountDown(bool b, float f)



    public Vector3[] GenerateGrid(int divide, Transform trans)
    {
        Vector3[] gridCenters = new Vector3[divide * divide];
        float mapXSize = trans.localScale.x * 10f;
        float mapZSize = trans.localScale.z * 10f;
        float cellXSize = mapXSize / divide;
        float cellZSize = mapZSize / divide;
        int cnt = 0;

        for (int i = divide - 1; i >= 0; i--)
        //for (int i = 0; i < divide; i++)
        {
            for (int j = 0; j < divide; j++)
            {
                float x = (j * cellXSize) - (mapXSize / 2) + (cellXSize / 2);
                float z = (i * cellZSize) - (mapZSize / 2) + (cellZSize / 2);

                Vector3 center = new Vector3(x, 0, z);
                gridCenters[cnt] = center;
                cnt++;

                //// 시각적으로 확인하기 위해 각 중심에 구 생성
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = center;
                //sphere.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            }
        }

        return gridCenters;
    }



    public Vector3 GetRandomPositionInLightRange(GameObject go, Transform mapTransform)
    {
        // 원기둥의 반지름 계산
        Transform trans = go.transform.GetChild(0).GetChild(0);
        float radius = trans.localScale.x * 2f; // 원기둥의 반지름 (Unity 기본 Cylinder의 크기는 직경 1, 반지름 0.5)

        // 맵 경계 내로 위치를 제한
        float mapWidth = mapTransform.localScale.x * 10f;
        float mapHeight = mapTransform.localScale.z * 10f;

        Vector3 randomPosition;

        do
        {
            // 랜덤 각도
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

            // 원 안의 랜덤 반지름 거리
            float distance = UnityEngine.Random.Range(0f, radius);

            // 원주 좌표 계산
            float x = Mathf.Cos(angle) * distance + trans.position.x;
            float z = Mathf.Sin(angle) * distance + trans.position.z;

            // 원기둥 기준으로 랜덤 위치 생성
            randomPosition = new Vector3(x, 0, z);

            // 맵 경계 내로 위치를 제한
            randomPosition.x = Mathf.Clamp(randomPosition.x, mapTransform.position.x - mapWidth / 2, mapTransform.position.x + mapWidth / 2);
            randomPosition.z = Mathf.Clamp(randomPosition.z, mapTransform.position.z - mapHeight / 2, mapTransform.position.z + mapHeight / 2);



        } while (!IsInsideCircle(randomPosition, new Vector3(trans.position.x, 0, trans.position.z), radius));

        return randomPosition;
    }

    bool IsInsideCircle(Vector3 point, Vector3 center, float radius)
    {
        // 주어진 점이 원 안에 있는지 확인
        return (point - center).sqrMagnitude <= radius * radius;
    }
}
