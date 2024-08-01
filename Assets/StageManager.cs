using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;
using Vector3 = UnityEngine.Vector3;

public class StageManager : MonoBehaviour //해당 스테이지 판단하고 레벨 컨스트럭트사용해서 여러가지 함
{
    public static StageManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //animator = GetComponent<Animator>();
            //animator.SetBool("isRotate", false);
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public GameConstructSet GCS;
    public static LevelConstructSet LCS;

    public static GameObject map;
    public static GameObject prePlayer;
    public static GameObject player;


    public GameObject preMap;
    //public GameObject map;
    //public GameObject prePlayer;
    //public GameObject player;



    public static List<GameObject> preEnemies;
    public static List<GameObject> enemies;
    public static List<GameObject> lightObstacles;

    public static int targetScore;
    public static int timeConstrain;
    public static int enemyCnt;
    public static int currentScore;
    public static Vector3 mapSize;
    public static Transform mapTransform;





    //public static StageManager instance;
    //public static StageManager Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = FindObjectOfType<StageManager>();
    //            if (instance == null)
    //            {
    //                GameObject singletonObject = new GameObject(typeof(StageManager).ToString());
    //                instance = singletonObject.AddComponent<StageManager>();
    //                DontDestroyOnLoad(singletonObject);
    //            }
    //        }
    //        return instance;
    //    }
    //}

    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else if (instance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    // 메인화면으로 돌아왔을 때 인스턴스를 제거하는 메서드
    //public void ResetInstance()
    //{
    //    instance = null;
    //    Destroy(gameObject);
    //}


    public GameObject initializeObjectPos (GameObject go, List<ObjectPool> list)
    {
        go.transform.position = list[0].obj.transform.position;
        go.transform.rotation = list[0].obj.transform.rotation;
        return go;
    }
    //private Animator animator;


    public void EndStage()
    {
        DestoryObjects(lightObstacles);
        DestoryObjects(preEnemies);
        DestoryObjects(enemies);
        
        player = initializeObjectPos(player, LCS.player);
        initializeObjects();
    }


    public GameResult Gresult;

    public SceneState Sstate; // 초기값 지정해서 사전을 할건지 안할 건지
    public static PlayState Pstate = PlayState.Wait;


    public void Start()
    {
        


        GenerateGrid();

        LCS = GCS.levelConstructSets[GameManager.instance.playerData.curStage];
        map = new GameObject();
        map = LCS.map[0].obj;
        mapSize = GetMapSize();
        mapTransform = map.transform;

        prePlayer = InstantiateObject<Prepartion>(LCS.prePlayer);
        player = InstantiateObject<Player>(LCS.player);

        initializeObjects();

        

    }

    //Vector3[,] gridCenters;

    Vector3[] gridCenters;

    void GenerateGrid()
    {
        float cellSize = 180f / 3;
        //gridCenters = new Vector3[3, 3];
        gridCenters = new Vector3[9];

        int cnt = 0;

        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                float x = (j * cellSize) - (180f / 2) + (cellSize / 2);
                float z = (i * cellSize) - (180f / 2) + (cellSize / 2);
                Vector3 center = new Vector3(x, 0, z);
                gridCenters[cnt] = center;
                cnt++;
                //// 시각적으로 확인하기 위해 각 중심에 구 생성 (선택 사항)
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = center;
                //sphere.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            }
        }
    }

    public void Update()
    {
        

        switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        {
            case SceneState.Preparation:
                switchPlayObjects(false);
                switchPreparationObjects(true);

                //Sstate = StageState.Play;
                break;

            case SceneState.Play:
                switchPreparationObjects(false);
                switchPlayObjects(true);
                //Sstate = StageState.Preparation;
                break;
        }

        Pstate = PlayState.Playing;
    }



    public void changeState()
    {
        switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        {
            case SceneState.Preparation:
                switchPlayObjects(true);
                switchPreparationObjects(false);
                Sstate = SceneState.Play;
                break;

            case SceneState.Play:
                switchPreparationObjects(true);
                switchPlayObjects(false);
                Sstate = SceneState.Preparation;
                break;
        }
    }

    public void switchPlayObjects(bool b)
    {
        player.SetActive(b);
        //player.GetComponent<Player>();

        for (int i = 0; i < enemies.Count; i++)
        {
            if (enemies[i] != null) enemies[i].SetActive(b);
        }

        for (int i = 0; i < lightObstacles.Count; i++)
        {
            if (lightObstacles[i] != null) lightObstacles[i].SetActive(b);
        }
    }

    public void switchPreparationObjects(bool b)
    {
        prePlayer.SetActive(b);
        //prePlayer.GetComponent<Prepartion>();

        for (int i = 0; i < preEnemies.Count;i++)
        {
            if (preEnemies[i] != null) preEnemies[i].SetActive(b);
        }
    }


    public void DestoryObjects(List<GameObject> list)
    {
        for(int i= 0; i < list.Count;i++)
        {
            if (list[i] != null) Destroy(list[i]);
        }
    }




    public GameObject InstantiateObject<T>(List<ObjectPool> list)
    {
        GameObject go = new GameObject();
        go = Instantiate(list[0].obj, list[0].obj.transform.position, list[0].obj.transform.rotation);
        T p = go.GetComponent<T>();
        return go;
    }

    public List<GameObject> InstantiateObjects<T>(List<ObjectPool> list)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].cnt; j++)
            {
                GameObject go = Instantiate(list[i].obj, list[0].obj.transform.position, list[i].obj.transform.rotation);
                //GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }
        return lgo;
    }

    public List<GameObject> InstantiateObjectsInLightRange<T>(List<ObjectPool> list)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].cnt; j++)
            {

                GameObject go = Instantiate(list[i].obj, GetRandomPositionInLightRange(lightObstacles[UnityEngine.Random.Range(0, lightObstacles.Count)]), list[i].obj.transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }
        return lgo;
    }

    Vector3 GetRandomPositionInLightRange(GameObject go)
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


        //// 랜덤 각도
        //float angle = UnityEngine.Random.Range(0f, Mathf.PI * 2);

        //// 원 안의 랜덤 반지름 거리
        //float distance = UnityEngine.Random.Range(0f, radius);

        //// 원주 좌표 계산
        //float x = Mathf.Cos(angle) * distance + trans.position.x;
        //float z = Mathf.Sin(angle) * distance + trans.position.z;

        //// 원기둥 기준으로 랜덤 위치 생성
        //Vector3 randomPosition = new Vector3(x, 0, z);


        //randomPosition.x = Mathf.Clamp(randomPosition.x, map.transform.position.x - mapWidth / 2, map.transform.position.x + mapWidth / 2);
        //randomPosition.z = Mathf.Clamp(randomPosition.z, map.transform.position.z - mapHeight / 2, map.transform.position.z + mapHeight / 2);



        //Transform trans = go.transform.GetChild(0);
        //float x = UnityEngine.Random.Range(-trans.localScale.x, trans.localScale.x) + trans.position.x;
        //float z = UnityEngine.Random.Range(-trans.localScale.z, trans.localScale.z) + trans.position.z;


        return randomPosition;
    }

    bool IsInsideCircle(Vector3 point, Vector3 center, float radius)
    {
        // 주어진 점이 원 안에 있는지 확인
        return (point - center).sqrMagnitude <= radius * radius;
    }


    public List<GameObject> InstantiateLightObstacle<T>(List<ObjectPool> list)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < list.Count; i++)
        {
            for (int j = 0; j < list[i].cnt; j++)
            {
                Vector3 newPos = new(gridCenters[7].x, list[0].obj.transform.position.y, gridCenters[7].z);

                GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
                //GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }
        return lgo;
    }


    public void initializeObjects()
    {
        //lightObstacles = InstantiateObjects<LightObstacle>(LCS.lightObstacles);
        lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
        

        preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);
        enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);
        
    }

    Vector3 GetMapSize()
    {
        Transform transform = map.transform;
        float width = transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
        float height = transform.localScale.z * 10f; // Plane의 높이
        return new Vector3(width, 0, height);
    }

    Vector3 GetRandomPositionInMap(Vector3 mapSize)
    {
        Transform transform = map.transform;
        // 맵의 크기를 기준으로 랜덤한 위치 생성
        float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + transform.position.x;
        float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + transform.position.z;
        return new Vector3(x, 0, z);
    }




    //이벤트----------------------------------------------------------------------------------------//

    private void OnEnable()
    {
        EventManager.instance.OnLoadPreparationScene += changeState;
        EventManager.instance.OnLoadPlayScene += changeState;
        EventManager.instance.OnStageEnd += EndStage;

        //EventManager.instance.OnGameReStart += reLoadStage;
        //EventManager.instance.OnStageStart += LoadStage;
        //EventManager.instance.OnLoadPlayScene += loadStage;

    }

    private void OnDisable()
    {
        EventManager.instance.OnLoadPreparationScene -= changeState;
        EventManager.instance.OnLoadPlayScene -= changeState;
        EventManager.instance.OnStageEnd -= EndStage;

        //EventManager.instance.OnGameReStart -= reLoadStage;
        //EventManager.instance.OnStageStart -= LoadStage;
        //EventManager.instance.OnLoadPlayScene -= loadStage;
    }



}

