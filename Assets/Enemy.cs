using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;


public class Enemy : MonoBehaviour
{
    //public static Enemy instance { get; private set; }
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        nmAgent = GetComponent<NavMeshAgent>();
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else Destroy(gameObject);
    //}


    Material[] mat = new Material[2];




    public bool onceDamaged = false;






    ////private GameObject obj;
    public float health;
    //public string name { get; private set; }

    [SerializeField]
    NavMeshAgent nmAgent;
    [SerializeField]
    Transform target;
    private Collider colliderrrr;

    //private void Awake()
    //{
    //    health = 2;
    //    nmAgent = GetComponent<NavMeshAgent>();
    //}

    private ParticleSystem psystem;

    private void Start()
    {
        psystem = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
        //psystem = GetComponentInChildren<ParticleSystem>();
        health = 2;
        nmAgent = GetComponent<NavMeshAgent>();
        target = Player.instance.transform;
        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = true;

        mat = this.GetComponent<Renderer>().materials;

    }


    public void startEffect()
    {
        psystem.Play();
    }



    public void setNullNMagent()
    {
        nmAgent = null;
        Destroy(this.gameObject);
    }


    public void updateDestination(Transform tf)
    {
        if (StageManager.Pstate == PlayState.Playing)
        {
            if (this.nmAgent == null) return;
            //if (tf == null) return;
            //if(this == null) return;

            //Debug.Log("transform : " + tf.position.x + ",  " + tf.position.y + ",  " + tf.position.z);

            if (tf.position.y > 1)
            {
                nmAgent.SetDestination(new Vector3(tf.position.x, 1, tf.position.z));
            }

            else nmAgent.SetDestination(tf.position);

        }


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
        if(StageManager.Pstate == PlayState.Playing)
        {
            //if (this.nmAgent == null) return;

            //if (detectObject == null) return;

            Enemy enemy = detectObject.transform.parent.GetComponent<Enemy>();

            

            //if (enemy == null) return;

            //if (enemy.onceDamaged) return;



            //if (StageManager.currentPlayerIdx != 0) return;



            //Rigidbody rb = StageManager.player.GetComponent<Rigidbody>();
            //enemy.updateDestination(rb.transform);


            if (StageManager.player == null) return;
            enemy.updateDestination(StageManager.player.transform);
        }


    }



    public void damaged(float damamge, Vector3 forward)
    {
        if(health - damamge <= 0)
        {
            StageManager.currentScore++;
            setNullNMagent();
        }

        else
        {
            startEffect();
            health -= damamge;
            transform.position += new Vector3(forward.x * 2, 0, forward.z * 2);
            updateDestination(transform);
            onceDamaged = true;

            gameObject.GetComponent<MeshRenderer>().material = mat[1];

            StartCoroutine(Function.instance.CountDown(1f, () =>
            {
                onceDamaged = false;
                gameObject.GetComponent<MeshRenderer>().material = mat[0];
            }));
        }
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

    //    // Plane의 크기를 계산
    //    float width = transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
    //    float height = transform.localScale.z * 10f; // Plane의 높이
    //    return new Vector3(width, 0, height);
    //}

    //Vector3 GetRandomPositionInMap(Vector3 mapSize)
    //{
    //    Transform transform = StageManager.transform;

    //    // 맵의 크기를 기준으로 랜덤한 위치 생성
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

//            // 지정된 개수만큼 오브젝트를 생성
//            for (int i = 0; i < enemyCnt; i++)
//            {
//                Vector3 randomPosition = GetRandomPositionInMap(mapSize);
//                //Instantiate(enemyPrefab, randomPosition, Quaternion.identity);

//                enemies.Add(Instantiate(enemyPrefab, randomPosition, Quaternion.identity));
//            }


//        }

//        Vector3 GetMapSize()
//        {
//            // Plane의 크기를 계산
//            float width = map.transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
//            float height = map.transform.localScale.z * 10f; // Plane의 높이
//            return new Vector3(width, 0, height);
//        }

//        Vector3 GetRandomPositionInMap(Vector3 mapSize)
//        {
//            // 맵의 크기를 기준으로 랜덤한 위치 생성
//            float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + map.transform.position.x;
//            float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + map.transform.position.z;
//            return new Vector3(x, map.transform.position.y + 1, z);
//        }

//    }
