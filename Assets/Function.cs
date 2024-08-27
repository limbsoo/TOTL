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

                //// �ð������� Ȯ���ϱ� ���� �� �߽ɿ� �� ����
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = center;
                //sphere.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            }
        }

        return gridCenters;
    }



    public Vector3 GetRandomPositionInLightRange(GameObject go, Transform mapTransform)
    {
        // ������� ������ ���
        Transform trans = go.transform.GetChild(0).GetChild(0);
        float radius = trans.localScale.x * 2f; // ������� ������ (Unity �⺻ Cylinder�� ũ��� ���� 1, ������ 0.5)

        // �� ��� ���� ��ġ�� ����
        float mapWidth = mapTransform.localScale.x * 10f;
        float mapHeight = mapTransform.localScale.z * 10f;

        Vector3 randomPosition;

        do
        {
            // ���� ����
            float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

            // �� ���� ���� ������ �Ÿ�
            float distance = UnityEngine.Random.Range(0f, radius);

            // ���� ��ǥ ���
            float x = Mathf.Cos(angle) * distance + trans.position.x;
            float z = Mathf.Sin(angle) * distance + trans.position.z;

            // ����� �������� ���� ��ġ ����
            randomPosition = new Vector3(x, 0, z);

            // �� ��� ���� ��ġ�� ����
            randomPosition.x = Mathf.Clamp(randomPosition.x, mapTransform.position.x - mapWidth / 2, mapTransform.position.x + mapWidth / 2);
            randomPosition.z = Mathf.Clamp(randomPosition.z, mapTransform.position.z - mapHeight / 2, mapTransform.position.z + mapHeight / 2);



        } while (!IsInsideCircle(randomPosition, new Vector3(trans.position.x, 0, trans.position.z), radius));

        return randomPosition;
    }

    bool IsInsideCircle(Vector3 point, Vector3 center, float radius)
    {
        // �־��� ���� �� �ȿ� �ִ��� Ȯ��
        return (point - center).sqrMagnitude <= radius * radius;
    }
}
