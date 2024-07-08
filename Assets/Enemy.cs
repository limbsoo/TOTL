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

        // 지정된 개수만큼 오브젝트를 생성
        for (int i = 0; i < enemyCnt; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMap(mapSize);
            //Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

            enemies.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity));
        }

        
    }

    Vector3 GetMapSize()
    {
        // Plane의 크기를 계산
        float width = map.transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
        float height = map.transform.localScale.z * 10f; // Plane의 높이
        return new Vector3(width, 0, height);
    }

    Vector3 GetRandomPositionInMap(Vector3 mapSize)
    {
        // 맵의 크기를 기준으로 랜덤한 위치 생성
        float x = Random.Range(-mapSize.x / 2, mapSize.x / 2) + map.transform.position.x;
        float z = Random.Range(-mapSize.z / 2, mapSize.z / 2) + map.transform.position.z;
        return new Vector3(x, map.transform.position.y + 1, z);
    }

}
