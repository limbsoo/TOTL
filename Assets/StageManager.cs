using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UIElements;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.EventSystems.EventTrigger;
using Vector3 = UnityEngine.Vector3;

public class StageManager : MonoBehaviour //�ش� �������� �Ǵ��ϰ� ���� ����Ʈ��Ʈ����ؼ� �������� ��
{
    public static StageManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public GameConstructSet GCS;
    public static LevelConstructSet LCS;

    public GameObject preMap;
    public static GameObject map;
    public static GameObject prePlayer;
    public static GameObject player;
    public static List<GameObject> preEnemies;
    public static List<GameObject> enemies;
    public static List<GameObject> lightObstacles;

    public static int targetScore;
    public static int timeConstrain;
    public static int enemyCnt;
    public static int currentScore;
    //public static Vector3 mapSize;
    public static Transform mapTransform;


    public GameResult Gresult;
    public SceneState Sstate; // �ʱⰪ �����ؼ� ������ �Ұ��� ���� ����
    public static PlayState Pstate = PlayState.Wait;
    Vector3[] gridCenters;



    public GameObject initializeObjectPos (GameObject go, List<ObjectPool> list)
    {
        go.transform.position = list[0].obj.transform.position;
        go.transform.rotation = list[0].obj.transform.rotation;
        return go;
    }


    public void EndStage()
    {
        //DestoryObjects(lightObstacles);
        //DestoryObjects(preEnemies);
        //DestoryObjects(enemies);
        
        //player = initializeObjectPos(player, LCS.player);
        //initializeObjects();
    }


    List<List<int>> lightObstacleGrid;


    public void Start()
    {
        GenerateGrid(); // ���� ����


        LCS = GCS.levelConstructSets[GameManager.instance.playerData.curStage];

        targetScore = LCS.targetScore;

        map = new GameObject();
        map = LCS.map[0].obj;
        //mapSize = GetMapSize();
        mapTransform = map.transform;


       
        lightObstacleGrid = new List<List<int>>();
        lightObstacleGrid.Add(new List<int> { 3 });
        lightObstacleGrid.Add(new List<int> { 2, 3 });


        lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
        enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);
        //preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);


        //prePlayer = InstantiateObject<Prepartion>(LCS.prePlayer);
        player = InstantiateObject<Player>(LCS.player);

        //initializeObjects();

        //switchPreparationObjects(false);
        switchPlayObjects(true);

        Pstate = PlayState.Playing;

    }



    public void initializeStage()
    {
        Debug.Log("�� ����");

        for (int i = 0; i < enemies.Count;i++)
        {
            if (enemies[i] != null) Destroy(enemies[i]);
        }

        Debug.Log("�� �����Ϸ�");

        for (int i = 0; i < lightObstacles.Count; i++)
        {
            if (lightObstacles[i] != null) Destroy(lightObstacles[i]);
        }

        enemies = new List<GameObject>();
        lightObstacles = new List<GameObject>();

        
        currentScore = 0;

        lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
        Debug.Log("�� ���� ��");
        enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);
        //preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);
        Debug.Log("�� ���� �Ϸ�");
        switchPlayObjects(false);
        //switchPreparationObjects(false);



        StartCoroutine(Function.instance.CountDown(2, () => {
            switchPlayObjects(true);
        }));

        //switchPlayObjects(true);
        Pstate = PlayState.Playing;
    }






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
                //// �ð������� Ȯ���ϱ� ���� �� �߽ɿ� �� ���� (���� ����)
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = center;
                //sphere.transform.localScale = new Vector3(5.5f, 5.5f, 5.5f);
            }
        }
    }

    public UnityEvent OnStageClear;

    public void Update()
    {
        if (Pstate == PlayState.Playing)
        {
            if (targetScore <= currentScore)
            {
                Debug.Log("��ǥ�޼�");
                Pstate = PlayState.Wait;
                OnStageClear?.Invoke();
            }




            //switch (Sstate) //���� ��Ȳ �Ǵ� ���������� ������������
            //{
            //    case SceneState.Preparation:
            //        switchPlayObjects(false);
            //        switchPreparationObjects(true);

            //        //Sstate = StageState.Play;
            //        break;

            //    case SceneState.Play:
            //        switchPreparationObjects(false);
            //        switchPlayObjects(true);
            //        //Sstate = StageState.Preparation;
            //        break;
            //}

            //Pstate = PlayState.Playing;
        }

    }

    public void changeState()
    {
        //switch (Sstate) //���� ��Ȳ �Ǵ� ���������� ������������
        //{
        //    case SceneState.Preparation:
        //        switchPlayObjects(true);
        //        switchPreparationObjects(false);
        //        Sstate = SceneState.Play;
        //        break;

        //    case SceneState.Play:
        //        switchPreparationObjects(true);
        //        switchPlayObjects(false);
        //        Sstate = SceneState.Preparation;
        //        break;
        //}
    }

    public void switchPlayObjects(bool b)
    {
        //player.SetActive(b);
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
                Vector3 randomPos = GetRandomPositionInLightRange(lightObstacles[UnityEngine.Random.Range(0, lightObstacles.Count)]);
                randomPos.y = 1;

                GameObject go = Instantiate(list[i].obj, randomPos, list[i].obj.transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }
        return lgo;
    }

    Vector3 GetRandomPositionInLightRange(GameObject go)
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


    public List<GameObject> InstantiateLightObstacle<T>(List<ObjectPool> list)
    {
        List<GameObject> lgo = new List<GameObject>();

        

        for (int i = 0; i < list.Count; i++)
        {
            //for (int j = 0; j < LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt.Length; j++)
            //{
            //    Vector3 newPos = new(gridCenters[LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt[j]].x, list[0].obj.transform.position.y, gridCenters[LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt[j]].z);

            //    GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
            //    GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
            //    T p = go.GetComponent<T>();
            //    lgo.Add(go);
            //}



            for (int j = 0; j < lightObstacleGrid[GameManager.instance.playerData.curStage].Count; j++)
            //for (int j = 0; j < list[i].cnt; j++)
            {
                Vector3 newPos = new(gridCenters[lightObstacleGrid[GameManager.instance.playerData.curStage][j]].x, list[0].obj.transform.position.y, gridCenters[lightObstacleGrid[GameManager.instance.playerData.curStage][j]].z);

                GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
                //GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }
        return lgo;
    }


    //public void initializeObjects()
    //{
    //    //lightObstacles = InstantiateObjects<LightObstacle>(LCS.lightObstacles);
    //    lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
    //    preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);
    //    enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);
    //}

    //Vector3 GetMapSize()
    //{
    //    Transform transform = map.transform;
    //    float width = transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
    //    float height = transform.localScale.z * 10f; // Plane�� ����
    //    return new Vector3(width, 0, height);
    //}

    //Vector3 GetRandomPositionInMap(Vector3 mapSize)
    //{
    //    Transform transform = map.transform;
    //    // ���� ũ�⸦ �������� ������ ��ġ ����
    //    float x = UnityEngine.Random.Range(-mapSize.x / 2, mapSize.x / 2) + transform.position.x;
    //    float z = UnityEngine.Random.Range(-mapSize.z / 2, mapSize.z / 2) + transform.position.z;
    //    return new Vector3(x, 0, z);
    //}




    //�̺�Ʈ----------------------------------------------------------------------------------------//

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

