using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StageManager : MonoBehaviour
{
    public static StageManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    // 적이 배회하게



    //프리팹 플레이어를 어디에 넣어야 좋을까

    public GameConstructSet GCS;

    public static List<GameObject> players;
    public static List<GameObject> enemies;
    public static List<GameObject> lightObstacles;

    public int currentScore;
    public static int currentPlayerIdx;



    public void Start()
    {
        LoadStage();
    }


    public void Update()
    {
        LevelConstructSet lc = GCS.levelConstructSets[GCS.targetIdx];

        if(instance.currentScore == lc.enemyCnt)
        {
            EventManager.instance.gameEnd();
        }
    }


    public void LoadStage()
    {
        players = new List<GameObject>();
        enemies = new List<GameObject>();
        lightObstacles = new List<GameObject>();


        InitializePlayer(GCS.levelConstructSets[GCS.targetIdx]);
        players[0].SetActive(true);
        currentPlayerIdx = 0;


        InitializeEnemies(GCS.levelConstructSets[GCS.targetIdx]);
        InitializeLightObstacles(GCS.levelConstructSets[GCS.targetIdx]);


        //currentPlayer = players[0];
    }



    private void OnEnable()
    {
        EventManager.instance.OnGameReStart += reLoadStage;


        EventManager.instance.OnStageStart += LoadStage;
    }

    private void OnDisable()
    {
        EventManager.instance.OnGameReStart -= reLoadStage;
        EventManager.instance.OnStageStart -= LoadStage;
    }

    void reLoadStage()
    {
        LoadStage();
    }


    private void InitializePlayer(LevelConstructSet LC)
    {
        GameObject playerObject;
        Player player;

        for (int i = 0; i < LC.player.Count; i++)
        {
            playerObject = null;
            playerObject = Instantiate(LC.player[i], LC.player[i].transform.position, LC.player[i].transform.rotation);
            playerObject.SetActive(false);

            player = null;
            player = playerObject.GetComponent<Player>();
            player.Initialize(100, "Player");
            players.Add(playerObject);
        }
    }

    private void InitializeEnemies(LevelConstructSet LC)
    {
        GameObject enemyObject;
        Enemy enemy;

        Vector3 mapSize = GetMapSize();

        for (int i = 0; i < LC.enemyCnt; i++)
        //for (int i = 0; i < LC.enemy.Count; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMap(mapSize);

            enemyObject = null;
            //enemyObject = Instantiate(LC.enemy[i], LC.enemy[i].transform.position, LC.enemy[i].transform.rotation);
            enemyObject = Instantiate(LC.enemy[0], randomPosition, LC.enemy[0].transform.rotation);

            enemy = null;
            enemy = enemyObject.GetComponent<Enemy>();
            enemy.Initialize(100, "enemy");
            enemies.Add(enemyObject);
        }
    }

    private void InitializeLightObstacles(LevelConstructSet LC)
    {
        GameObject lightObstacleObject;
        LightObstacle lightObstacle;

        Vector3 mapSize = GetMapSize();

        for (int i = 0; i < LC.obstacleCnt; i++)
        //for (int i = 0; i < LC.lightObstacle.Count; i++)
        {
            Vector3 randomPosition = GetRandomPositionInMap(mapSize);

            randomPosition.y = LC.lightObstacle[0].transform.position.y;

            lightObstacleObject = null;
            //enemyObject = Instantiate(LC.enemy[i], LC.enemy[i].transform.position, LC.enemy[i].transform.rotation);
            lightObstacleObject = Instantiate(LC.lightObstacle[0], randomPosition, LC.lightObstacle[0].transform.rotation);

            lightObstacle = null;
            lightObstacle = lightObstacleObject.GetComponent<LightObstacle>();
            lightObstacle.Initialize(100, "lightObstacle");
            lightObstacles.Add(lightObstacleObject);



            //lightObstacleObject = null;
            //lightObstacleObject = Instantiate(LC.lightObstacle[i], LC.lightObstacle[i].transform.position, LC.lightObstacle[i].transform.rotation);

            //lightObstacle = null;
            //lightObstacle = lightObstacleObject.GetComponent<LightObstacle>();
            //lightObstacle.Initialize(100, "lightObstacle");
            //lightObstacles.Add(lightObstacleObject);
        }
    }

    Vector3 GetMapSize()
    {
        Transform transform = GCS.levelConstructSets[GCS.targetIdx].map.transform;

        // Plane의 크기를 계산
        float width = transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
        float height = transform.localScale.z * 10f; // Plane의 높이
        return new Vector3(width, 0, height);
    }

    Vector3 GetRandomPositionInMap(Vector3 mapSize)
    {
        Transform transform = GCS.levelConstructSets[GCS.targetIdx].map.transform;

        // 맵의 크기를 기준으로 랜덤한 위치 생성
        float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + transform.position.x;
        float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + transform.position.z;
        return new Vector3(x, 0, z);
    }




    //private void InitializeLightObstacles(Vector3 position, int health, string enemyName)
    //{
    //    GameObject lightObstacleObject = Instantiate(GCS.levelConstructSets[0].lightObstacle[0], position, Quaternion.identity);
    //    LightObstacle lightObstacle = lightObstacleObject.GetComponent<LightObstacle>();
    //    lightObstacle.Initialize(health, enemyName);
    //}

    //public GameConstructSet GCS;
    //public GameObject Map;

    //public static int targetScore;
    //public static int timeConstrain;
    //public static int enemyCnt;

    //public static Transform transform;



    //public void loadStage()
    //{
    //    LevelConstructSet LC = GCS.levelConstructSets[GCS.targetIdx];

    //    targetScore = LC.targetScore;
    //    timeConstrain = LC.timeConstrain;
    //    enemyCnt = LC.enemyCnt;

    //    transform = Map.transform;

    //    //currentStage = stageNumber;
    //    //// 스테이지 로드 로직 (씬 전환 등)
    //    //Debug.Log("Stage " + stageNumber + " loaded.");
    //}


    //이벤트 모아야지
}
