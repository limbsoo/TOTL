using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;


public class Enemy : MonoBehaviour
{
    public static Enemy instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            nmAgent = GetComponent<NavMeshAgent>();
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }














    //private GameObject obj;
    public int health { get; private set; }
    public string name { get; private set; }


    NavMeshAgent nmAgent;
    public Transform target;

    //private void Awake()
    //{
    //    nmAgent = GetComponent<NavMeshAgent>();
    //}

    public void updateDestination(Transform tf)
    {
        nmAgent.SetDestination(tf.position);
    }

    private void OnEnable()
    {
        EventManager.instance.OnPlayerDetectedMonster += chasePlayer;
    }

    private void OnDisable()
    {
        EventManager.instance.OnPlayerDetectedMonster -= chasePlayer;
    }

    public void chasePlayer(GameObject detectObject)
    {
        //if (StageManager.currentPlayerIdx != 0) return;

        Enemy enemy = detectObject.transform.parent.GetComponent<Enemy>();

        //Rigidbody rb = StageManager.player.GetComponent<Rigidbody>();
        //enemy.updateDestination(rb.transform);
        enemy.updateDestination(StageManager.player.transform);
    }



    //public List<GameObject> enemies;


    //void Start()
    //{
    //    createEnemy(StageManager.enemyCnt);
    //}

    //public void createEnemy(int enemyCnt)
    //{
    //    Vector3 mapSize = GetMapSize();

    //    for (int i = 0; i < enemyCnt; i++)
    //    {
    //        Vector3 randomPosition = GetRandomPositionInMap(mapSize);
    //        enemies.Add(Instantiate(enemies[0], randomPosition, Quaternion.identity));
    //    }
    //}

    //Vector3 GetMapSize()
    //{
    //    Transform transform = StageManager.transform;

    //    // Plane�� ũ�⸦ ���
    //    float width = transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
    //    float height = transform.localScale.z * 10f; // Plane�� ����
    //    return new Vector3(width, 0, height);
    //}

    //Vector3 GetRandomPositionInMap(Vector3 mapSize)
    //{
    //    Transform transform = StageManager.transform;

    //    // ���� ũ�⸦ �������� ������ ��ġ ����
    //    float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + transform.position.x;
    //    float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + transform.position.z;
    //    return new Vector3(x, 1, z);
    //}

}



//    public class Enemy : MonoBehaviour
//    {
//        public GameObject map;

//        public GameObject enemyPrefab;
//        public GameObject currentObject;

//        public List<GameObject> enemies;
//        public GameConstructSet gcs;

//        //private void Awake()
//        //{
//        //    if (GameSet.enemyInstance == null) GameSet.enemyInstance = this;
//        //}

//        void Start()
//        {
//            //GameConstructSet gcs = GetComponent<GameConstructSet>();

//            createEnemy(gcs.levelConstructSets[gcs.targetIdx].enemyCnt);
//        }



//        public void createEnemy(int enemyCnt)
//        {
//            Vector3 mapSize = GetMapSize();

//            // ������ ������ŭ ������Ʈ�� ����
//            for (int i = 0; i < enemyCnt; i++)
//            {
//                Vector3 randomPosition = GetRandomPositionInMap(mapSize);
//                //Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

//                enemies.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity));
//            }


//        }

//        Vector3 GetMapSize()
//        {
//            // Plane�� ũ�⸦ ���
//            float width = map.transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
//            float height = map.transform.localScale.z * 10f; // Plane�� ����
//            return new Vector3(width, 0, height);
//        }

//        Vector3 GetRandomPositionInMap(Vector3 mapSize)
//        {
//            // ���� ũ�⸦ �������� ������ ��ġ ����
//            float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + map.transform.position.x;
//            float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + map.transform.position.z;
//            return new Vector3(x, map.transform.position.y + 1, z);
//        }

//    }
