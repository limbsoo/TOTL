using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;
using Vector3 = UnityEngine.Vector3;

public class StageManager : MonoBehaviour //해당 스테이지 판단하고 레벨 컨스트럭트사용해서 여러가지 함
{
    //public static StageManager instance { get; private set; }
    //private void Awake()
    //{
    //    if (instance == null)
    //    {
    //        instance = this;
    //        DontDestroyOnLoad(gameObject);
    //    }
    //    else Destroy(gameObject);
    //}

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


    public static StageManager instance;
    public static StageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<StageManager>();
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(StageManager).ToString());
                    instance = singletonObject.AddComponent<StageManager>();
                    DontDestroyOnLoad(singletonObject);
                }
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // 메인화면으로 돌아왔을 때 인스턴스를 제거하는 메서드
    public void ResetInstance()
    {
        instance = null;
        Destroy(gameObject);
    }





    public StageState Sstate; // 초기값 지정해서 사전을 할건지 안할 건지
    public static PlayingState Pstate = PlayingState.Wait;


    public void Start()
    {


















        initializeObjects();

        switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        {
            case StageState.Preparation:
                switchPlayObjects(false);
                switchPreparationObjects(true);
                //Sstate = StageState.Play;
                break;

            case StageState.Play:
                switchPreparationObjects(false);
                switchPlayObjects(true);
                //Sstate = StageState.Preparation;
                break;
        }

        Pstate = PlayingState.Playing;
    }

    public void Update()
    {
        //if(Pstate == PlayingState.Wait)
        //{
        //    switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        //    {
        //        case StageState.Preparation:
        //            switchPlayObjects(false);
        //            switchPreparationObjects(true);
        //            Sstate = StageState.Play;
        //            break;

        //        case StageState.Play:
        //            switchPreparationObjects(false);
        //            switchPlayObjects(true);
        //            Sstate = StageState.Preparation;
        //            break;
        //    }


        //    //switchPreparationObjects(false);
        //    Pstate = PlayingState.Playing;
        //}

        //switch (Pstate)
        //{
        //    case PlayingState.Wait:
        //        switchPreparationObjects(false);
        //        Pstate = PlayingState.Playing;
        //        break;
        //    //case PlayingState.Playing:
        //    //    switchPlayObjects(true);
        //    //    break;
        //}

        //if (currentScore == targetScore /*|| UIManager.instance.timeLimit.text == "1"*/)
        //{
        //    EventManager.instance.gameEnd();
        //}
    }


    public void changeState()
    {
        switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        {
            case StageState.Preparation:
                switchPlayObjects(true);
                switchPreparationObjects(false);
                Sstate = StageState.Play;
                break;

            case StageState.Play:
                switchPreparationObjects(true);
                switchPlayObjects(false);
                Sstate = StageState.Preparation;
                break;
        }


        //if (Pstate == PlayingState.Playing)
        //{
        //    switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
        //    {
        //        case StageState.Preparation:
        //            //switchPlayObjects(false);
        //            //switchPreparationObjects(true);
        //            Sstate = StageState.Play;

        //            break;

        //        case StageState.Play:
        //            //switchPreparationObjects(false);
        //            //switchPlayObjects(true);
        //            Sstate = StageState.Preparation;
        //            break;
        //    }
        //}

        //Pstate = PlayingState.Wait;
    }

    public void switchPlayObjects(bool b)
    {
        player.SetActive(b);

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

        for (int i = 0; i < preEnemies.Count;i++)
        {
            if (preEnemies[i] != null) preEnemies[i].SetActive(b);
        }
    }



    public void initializeObjects()
    {
        LCS = GCS.levelConstructSets[GCS.idx];

        map = new GameObject();
        prePlayer = new GameObject();
        player = new GameObject();
        preEnemies = new List<GameObject>();
        enemies = new List<GameObject>();
        lightObstacles = new List<GameObject>();

        map = LCS.map[0].obj;
        mapSize = GetMapSize();
        mapTransform = map.transform;

        prePlayer = Instantiate(LCS.prePlayer[0].obj, LCS.prePlayer[0].obj.transform.position, LCS.prePlayer[0].obj.transform.rotation);
        Prepartion pre = prePlayer.GetComponent<Prepartion>();
        //prePlayer.SetActive(false);

        player = Instantiate(LCS.player[0].obj, LCS.player[0].obj.transform.position, LCS.player[0].obj.transform.rotation);
        Player p = player.GetComponent<Player>();
        //player.SetActive(false);

        for (int i = 0; i < LCS.preEnemies.Count; i++)
        {
            for (int j = 0; j < LCS.preEnemies[i].cnt; j++)
            {
                GameObject obj = Instantiate(LCS.preEnemies[i].obj, GetRandomPositionInMap(mapSize), LCS.preEnemies[i].obj.transform.rotation);
                Enemy e = obj.GetComponent<Enemy>();
                //obj.SetActive(false);
                preEnemies.Add(obj);
            }
        }

        for (int i = 0; i < LCS.enemies.Count; i++)
        {
            for (int j = 0; j < LCS.enemies[i].cnt; j++)
            {
                GameObject obj = Instantiate(LCS.enemies[i].obj, GetRandomPositionInMap(mapSize), LCS.enemies[i].obj.transform.rotation);
                Enemy e = obj.GetComponent<Enemy>();
                //obj.SetActive(false);
                enemies.Add(obj);
            }
        }

        for (int i = 0; i < LCS.lightObstacles.Count; i++)
        {
            for (int j = 0; j < LCS.lightObstacles[i].cnt; j++)
            {
                GameObject obj = Instantiate(LCS.lightObstacles[i].obj, GetRandomPositionInMap(mapSize), LCS.lightObstacles[i].obj.transform.rotation);
                LightObstacle lo = obj.GetComponent<LightObstacle>();
                //obj.SetActive(false);
                lightObstacles.Add(obj);
            }
        }
    }

    public void initializeMain()
    {
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

        //EventManager.instance.OnGameReStart += reLoadStage;
        //EventManager.instance.OnStageStart += LoadStage;
        //EventManager.instance.OnLoadPlayScene += loadStage;

    }

    private void OnDisable()
    {
        EventManager.instance.OnLoadPreparationScene -= changeState;
        EventManager.instance.OnLoadPlayScene -= changeState;


        //EventManager.instance.OnGameReStart -= reLoadStage;
        //EventManager.instance.OnStageStart -= LoadStage;
        //EventManager.instance.OnLoadPlayScene -= loadStage;
    }



}

