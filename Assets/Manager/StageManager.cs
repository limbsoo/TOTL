using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
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
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    public GameConstructSet GCS;
    public int divide;

    public static LevelConstructSet LCS;
    
    public static GameObject player;
    public static List<GameObject> enemies;
    public static List<GameObject> fieldEffectLights;

    public static int curStage;
    public static int targetScore;
    public static int currentScore;

    Vector3[] gridCenters;
    public static Transform mapTransform;


    public Action stageLevelSetEnd;




    GridLayoutGroup glg;
    TimeLine[] t;

    public static StageState Sstate = StageState.Edit;

    public Action OnStageEnd;

    //public Action callback;



    public void Start()
    {
        fieldEffectLights = new List<GameObject>();
        enemies = new List<GameObject>();

        player = new GameObject();

        InitializeLevelSet();
        //player = InstantiateObject<Player>(LCS.player, 0);


        //if(UIManager.isReady)
        //{
        //    EventManager.instance.callbacks.Add(OnStageEnd);
        //}

        UIManager.instance.OnDecideBlock += initializeStage;
        UIManager.instance.OnInitializeUI += EndStage;
        UIManager.instance.OnUIisReady += makeStage;


        //stageLevelSetEnd?.Invoke();

        /////////////////////////
        //glg = FieldEffectPopUpManager.instance.slots.GetComponent<GridLayoutGroup>();
        //t = glg.GetComponentsInChildren<TimeLine>();


    }


    public void makeStage()
    {
        stageLevelSetEnd?.Invoke();
        player = InstantiateObject<Player>(LCS.player, 0);
    }


    public void Update()
    {
        switch (Sstate)
        {
            case StageState.Edit:
                break;

            case StageState.Play:
                if(targetScore <= currentScore)
                {
                    Sstate = StageState.Edit;
                    currentScore = 0;
                    DestroyObjects(enemies);
                    DestroyObjects(fieldEffectLights);

                    enemies.Clear();
                    fieldEffectLights.Clear();

                    OnStageEnd?.Invoke();

                }

                break;
        }

    }




    public void InitializeLevelSet()
    {
        curStage = DataManager.Instance.data.curStage;
        LCS = GCS.LevelConstructSet[curStage];
        mapTransform = LCS.map[0].transform;
        gridCenters = Function.instance.GenerateGrid(divide, mapTransform);
        targetScore = LCS.targetScore;
    }

    public void initStage()
    {
        player = InstantiateObject<Player>(LCS.player, 0);

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();
            GameObject go = InstantiateLO<LightObstacle>(LCS.FieldEffectLight, feb.lineNum, feb.name);
            fieldEffectLights.Add(go);
        }

        if (fieldEffectLights.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);

        Sstate = StageState.Play;
    }


    public void initializeStage()
    {
        //DestroyObjects(enemies);
        //DestroyObjects(lightObstacles);

        //InitializeLevelSet();

        //if(player == null) player = InstantiateObject<Player>(LCS.player, 0);



        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();
            GameObject go = InstantiateLO<LightObstacle>(LCS.FieldEffectLight, feb.lineNum, feb.name);
            fieldEffectLights.Add(go);
        }

        if (fieldEffectLights.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);

        Sstate = StageState.Play;


    }


    public GameObject InstantiateLO<T>(List<GameObject> list, int idx, string name)
    {
        Vector3 newPos = new Vector3();
        newPos.x = gridCenters[idx].x;
        newPos.y = list[0].transform.position.y;
        newPos.z = gridCenters[idx].z;

        int cnt = 0;


        switch (name)
        {
            case ("blink"):
                cnt = 0;
                break;

            case ("shadow"):
                cnt = 1;
                break;

            case ("disable"):
                cnt = 2;
                break;
        }



        GameObject go = Instantiate(list[cnt], newPos, list[cnt].transform.rotation);
        T p = go.GetComponent<T>();

        return go;
    }










    //public void DestroyObjects<T>(List<GameObject> list)
    public void DestroyObjects(List<GameObject> list)
    {
        if (list == null) return;

        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }
    }




    public GameObject InstantiateObject<T>(List<GameObject> list, int idx)
    {
        GameObject go = new GameObject();
        go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);
        T p = go.GetComponent<T>();
        return go;
    }

    //public List<GameObject> InstantiateLightObstacle<T>(List<ObjectPool> list)
    //{
    //    List<GameObject> lgo = new List<GameObject>();

    //    for (int i = 0; i < GCS.lightObstacleGrid[curStage].cnt.Length; i++)
    //    {
    //        Vector3 newPos = new Vector3();
    //        newPos.x = gridCenters[GCS.lightObstacleGrid[curStage].cnt[i]].x;
    //        newPos.y = list[0].obj.transform.position.y;
    //        newPos.z = gridCenters[GCS.lightObstacleGrid[curStage].cnt[i]].z;

    //        GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
    //        T p = go.GetComponent<T>();
    //        lgo.Add(go);
    //    }
    //    return lgo;
    //}

    public List<GameObject> InstantiateObjectsInLightRange<T>(List<GameObject> list, int cnt)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < cnt; i++)
        {
            int randEnemyIdx = UnityEngine.Random.Range(0, list.Count);
             Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffectLights[UnityEngine.Random.Range(0, fieldEffectLights.Count)], mapTransform);
            randomPos.y = list[randEnemyIdx].transform.position.y;
            GameObject go = Instantiate(list[randEnemyIdx], randomPos, list[randEnemyIdx].transform.rotation);
            T p = go.GetComponent<T>();
            lgo.Add(go);

        }

        return lgo;
    }















    


    public GameResult Gresult;
    //public SceneState Sstate; // 초기값 지정해서 사전을 할건지 안할 건지
    




    //public void Pause()
    //{
    //    Pstate = PlayState.Wait;
    //    Time.timeScale = 0;
    //}

    //public void Resume()
    //{
    //    Pstate = PlayState.Playing;
    //    Time.timeScale = 1f;
    //}






    




    //public GameObject initializeObjectPos (GameObject go, List<ObjectPool> list)
    //{
    //    go.transform.position = list[0].obj.transform.position;
    //    go.transform.rotation = list[0].obj.transform.rotation;
    //    return go;
    //}


    public void EndStage()
    {
        currentScore = 10;
        //MapStatusPopUpmanager.instance.CreateBlock();



        //DestoryObjects(lightObstacles);
        //DestoryObjects(preEnemies);
        //DestoryObjects(enemies);

        //player = initializeObjectPos(player, LCS.player);
        //initializeObjects();
    }


    //List<List<int>> lightObstacleGrid;


    //public void Start()
    //{
    //    LCS = GCS.levelConstructSets[GameManager.instance.playerData.curStage];
    //    mapTransform = LCS.map[0].obj.transform;
    //    gridCenters = Function.instance.GenerateGrid(divide, mapTransform);
    //    targetScore = LCS.targetScore;
    //    curStage = GameManager.instance.playerData.curStage;

    //    player = InstantiateObject<Player>(LCS.player);
    //    lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
    //    enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);

        

    //    //lightObstacleGrid = new List<List<int>>();
    //    //lightObstacleGrid.Add(new List<int> { 3 });
    //    //lightObstacleGrid.Add(new List<int> { 2, 3 });



    //    //preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);


    //    //prePlayer = InstantiateObject<Prepartion>(LCS.prePlayer);


    //    //initializeObjects();

    //    //switchPreparationObjects(false);
    //    //switchPlayObjects(true);

    //    Pstate = PlayState.Playing;

    //}



    //public void initializeStage()
    //{
    //    Debug.Log("적 삭제");

    //    for (int i = 0; i < enemies.Count;i++)
    //    {
    //        if (enemies[i] != null) Destroy(enemies[i]);
    //    }

    //    Debug.Log("적 삭제완료");

    //    for (int i = 0; i < lightObstacles.Count; i++)
    //    {
    //        if (lightObstacles[i] != null) Destroy(lightObstacles[i]);
    //    }

    //    enemies = new List<GameObject>();
    //    lightObstacles = new List<GameObject>();

        
    //    currentScore = 0;

    //    lightObstacles = InstantiateLightObstacle<LightObstacle>(LCS.lightObstacles);
    //    Debug.Log("적 생성 전");
    //    enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemies);
    //    //preEnemies = InstantiateObjects<Enemy>(LCS.preEnemies);
    //    Debug.Log("적 생성 완료");
    //    switchPlayObjects(false);
    //    //switchPreparationObjects(false);



    //    StartCoroutine(Function.instance.CountDown(2, () => {
    //        switchPlayObjects(true);
    //    }));

    //    //switchPlayObjects(true);
    //    Pstate = PlayState.Playing;
    //}


    public UnityEvent OnStageClear;

    //public void Update()
    //{
    //    if (Pstate == PlayState.Playing)
    //    {
    //        if (targetScore <= currentScore)
    //        {
    //            Debug.Log("목표달성");
    //            Pstate = PlayState.Wait;
    //            OnStageClear?.Invoke();
    //        }




    //        //switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
    //        //{
    //        //    case SceneState.Preparation:
    //        //        switchPlayObjects(false);
    //        //        switchPreparationObjects(true);

    //        //        //Sstate = StageState.Play;
    //        //        break;

    //        //    case SceneState.Play:
    //        //        switchPreparationObjects(false);
    //        //        switchPlayObjects(true);
    //        //        //Sstate = StageState.Preparation;
    //        //        break;
    //        //}

    //        //Pstate = PlayState.Playing;
    //    }

    //}

    public void changeState()
    {
        //switch (Sstate) //현재 상황 판단 본게임인지 사전게임인지
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

        for (int i = 0; i < fieldEffectLights.Count; i++)
        {
            if (fieldEffectLights[i] != null) fieldEffectLights[i].SetActive(b);
        }
    }




    public void DestoryObjects(List<GameObject> list)
    {
        for(int i= 0; i < list.Count;i++)
        {
            if (list[i] != null) Destroy(list[i]);
        }
    }







    //public List<GameObject> InstantiateObjects<T>(List<ObjectPool> list)
    //{
    //    List<GameObject> lgo = new List<GameObject>();

    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        for (int j = 0; j < list[i].cnt; j++)
    //        {
    //            GameObject go = Instantiate(list[i].obj, list[0].obj.transform.position, list[i].obj.transform.rotation);
    //            //GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
    //            T p = go.GetComponent<T>();
    //            lgo.Add(go);
    //        }
    //    }
    //    return lgo;
    //}


    //public List<GameObject> InstantiateLightObstacle<T>(List<ObjectPool> list)
    //{
    //    List<GameObject> lgo = new List<GameObject>();



    //    for (int i = 0; i < list.Count; i++)
    //    {
    //        //for (int j = 0; j < LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt.Length; j++)
    //        //{
    //        //    Vector3 newPos = new(gridCenters[LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt[j]].x, list[0].obj.transform.position.y, gridCenters[LCS.lightObstacleGrid[GameManager.instance.playerData.curStage].cnt[j]].z);

    //        //    GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
    //        //    GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
    //        //    T p = go.GetComponent<T>();
    //        //    lgo.Add(go);
    //        //}



    //        for (int j = 0; j < lightObstacleGrid[GameManager.instance.playerData.curStage].Count; j++)
    //        //for (int j = 0; j < list[i].cnt; j++)
    //        {
    //            Vector3 newPos = new(gridCenters[lightObstacleGrid[GameManager.instance.playerData.curStage][j]].x, list[0].obj.transform.position.y, gridCenters[lightObstacleGrid[GameManager.instance.playerData.curStage][j]].z);

    //            GameObject go = Instantiate(list[i].obj, newPos, list[i].obj.transform.rotation);
    //            //GameObject go = Instantiate(list[i].obj, GetRandomPositionInMap(mapSize), list[i].obj.transform.rotation);
    //            T p = go.GetComponent<T>();
    //            lgo.Add(go);
    //        }
    //    }
    //    return lgo;
    //}






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

