using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject map;

    public GameObject enemyPrefab;
    public GameObject currentObject;

    public List<GameObject> enemies;

    private void Awake()
    {
        if (GameSet.enemyInstance == null) GameSet.enemyInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void createEnemy(int enemyCnt)
    {
        Vector3 mapSize = GetMapSize();

        // ������ ������ŭ ������Ʈ�� ����
        for (int i = 0; i < enemyCnt; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMap(mapSize);
            //Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            enemies.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity));
        }

        
    }

    Vector3 GetMapSize()
    {
        // Plane�� ũ�⸦ ���
        float width = map.transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
        float height = map.transform.localScale.z * 10f; // Plane�� ����
        return new Vector3(width, 0, height);
    }

    Vector3 GetRandomPositionInMap(Vector3 mapSize)
    {
        // ���� ũ�⸦ �������� ������ ��ġ ����
        float x = Random.Range(-mapSize.x / 2, mapSize.x / 2) + map.transform.position.x;
        float z = Random.Range(-mapSize.z / 2, mapSize.z / 2) + map.transform.position.z;
        return new Vector3(x, map.transform.position.y + 1, z);
    }

}
