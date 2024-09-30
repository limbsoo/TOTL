using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
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
    //public static List<GameObject> fieldEffectLights;

    public static List<GameObject> fieldEffects;


    public static int curStage;
    public static int targetScore;
    public static int currentScore;

    public Vector3[] gridCenters;
    public static Transform mapTransform;


    public Action stageLevelSetEnd;


    public static List<GameObject> goals;



    //GridLayoutGroup glg;
    //TimeLine[] t;

    public static StageState Sstate = StageState.Edit;

    public Action OnStageEnd;

    //public Action callback;


    //public static int waveTime;

    public int waveTime = 0;
    public int stageTime = 0;
    Coroutine waveTimeCoroutine = null;
    Coroutine stageTimeCoroutine = null;


    public List<int> goalList;

    public int m_curWave;

    public Action OnContinueWave;


    public void Start()
    {
        DataManager.Instance.LoadGameData();
        m_curWave = DataManager.Instance.data.curWave;

        fieldEffects = new List<GameObject>();
        enemies = new List<GameObject>();

        //하나로 만드는게
        goals = new List<GameObject>();
        goalList = new List<int> { };

        LoadLevelSet();
        gridCenters = Function.instance.GenerateGrid(divide, mapTransform);
        for (int i = 0; i < gridCenters.Length; i++) goals.Add(InstantiateGoal(i));

        UIManager.instance.OnDecideBlock = () =>
        {
            if(player == null)
            {
                player = new GameObject();
                player = InstantiateObject<Player>(LCS.player, 0);
            }
            initializeStage();
        };
    }

    public double timereee = 0;

    public void Update()
    {
        switch (Sstate)
        {
            case StageState.Edit:
                waveTime = 0;
                break;

            case StageState.Play:
                if (targetScore <= currentScore)
                {
                    Sstate = StageState.Edit;
                    currentScore = 0;
                    DestroyObjects(enemies);
                    //DestroyObjects(fieldEffectLights);
                    DestroyObjects(fieldEffects);

                    DataManager.Instance.SaveGameData();

                    OnStageEnd?.Invoke();
                }

                //if (waveTimeCoroutine == null)
                //{
                //    waveTimeCoroutine = StartCoroutine(Function.instance.CountDown(1, () =>
                //    {

                //        timereee += Time.deltaTime;

                //        waveTime = (int) timereee;

                //        if (waveTime == 10)
                //        {
                //            waveTime = 0;
                //            timereee = 0;
                //        }
                        
                //        waveTimeCoroutine = null;
                //    }));
                //}

                //if (stageTimeCoroutine == null)
                //{
                //    stageTimeCoroutine = StartCoroutine(Function.instance.CountDown(1, () =>
                //    {
                //        stageTime++;
                //        stageTimeCoroutine = null;
                //    }));
                //}


                break;
        }

    }



    public void LoadLevelSet()
    {
        LCS = GCS.LevelConstructSet[curStage];
        mapTransform = LCS.map[0].transform;
        targetScore = LCS.targetScore;
    }
    public GameObject InstantiateGoal(int idx)
    {
        Vector3 newPos = gridCenters[idx];
        newPos.y += 1;
        GameObject go = Instantiate(LCS.Goal[0], newPos, LCS.Goal[0].transform.rotation);
        go.SetActive(false);
        return go;
    }










    //public void makeStage()
    //{
    //    //stageLevelSetEnd?.Invoke();
    //    player = InstantiateObject<Player>(LCS.player, 0);
    //}





    //public void InitializeLevelSet()
    //{
    //    idx = 0;
    //    curStage = DataManager.Instance.data.curStage;
    //    LCS = GCS.LevelConstructSet[curStage];
    //    mapTransform = LCS.map[0].transform;
    //    gridCenters = Function.instance.GenerateGrid(divide, mapTransform);
    //    targetScore = LCS.targetScore;

    //    for (int i = 0; i < 9; i++)
    //    {
    //        goals.Add(InstantiateGoals(i));
    //    }
    //}





    //public void initStage()
    //{
    //    player = InstantiateObject<Player>(LCS.player, 0);

    //    for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
    //    {
    //        FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();
    //        GameObject go = InstantiateLO<LightObstacle>(LCS.FieldEffectLight, feb.lineNum, feb.name);
    //        fieldEffectLights.Add(go);
    //    }

    //    if (fieldEffectLights.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);

    //    Sstate = StageState.Play;
    //}

    public void arriveGoal(GameObject g)
    {
        g.gameObject.SetActive(false);
        currentScore++;

    }




    //public int startTime;
    //public int endTime;
    public void initializeStage()
    {
        goalList.Clear();

        for (int i = 0; i < LCS.targetScore;i++)
        {
            while(true)
            {
                int randNum = UnityEngine.Random.Range(1,9);
                int num = goalList.Find(x => x == randNum);

                if(num == 0)
                {
                    goalList.Add(randNum);
                    goals[randNum].SetActive(true);
                    break;
                }
            }
        }



        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();

            GameObject go = InstantiateFieldEffect(LCS.FieldEffect, feb);



            //GameObject go = InstantiateFieldEffect(LCS.FieldEffect, feb.lineNum, feb.name, FieldEffectPopUpManager.instance.blocks[i].name);

            FieldEffect fe = go.GetComponent<FieldEffect>();
            fe.Init(feb.start, feb.end, feb.lineNum, feb.m_upperIdx, feb.m_downerIdx);
            fieldEffects.Add(go);

            //transform.GetChild(0)

            //fieldEffectLights.Add(go);
        }


        //if (fieldEffects.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);
        if (fieldEffects.Count != 0) enemies = InstantiateEnemies<Enemy>(LCS.enemy, LCS.enemyCnt + LCS.increaseEnemyPerWave *( m_curWave + 1));

        Sstate = StageState.Play;


    }

    public List<GameObject> InstantiateEnemies<T>(List<GameObject> list, int cnt)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < cnt; i++)
        {
            int randEnemyIdx = UnityEngine.Random.Range(0, list.Count);
            //Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffectLights[UnityEngine.Random.Range(0, fieldEffectLights.Count)], mapTransform);
            Vector3 randomPos = Function.instance.GetRandomPositionInMap(fieldEffects[UnityEngine.Random.Range(0, fieldEffects.Count)], mapTransform);




            randomPos.y = list[randEnemyIdx].transform.position.y;
            GameObject go = Instantiate(list[randEnemyIdx], randomPos, list[randEnemyIdx].transform.rotation);
            T p = go.GetComponent<T>();
            lgo.Add(go);

        }

        return lgo;
    }





    public int idx = 0;

    public GameObject InstantiateFieldEffect(List<GameObject> list, FieldEffectBlock feb)
    {
        Vector3 newPos = new Vector3(gridCenters[feb.lineNum].x, list[0].transform.position.y, gridCenters[feb.lineNum].z);
        GameObject go = Instantiate(list[feb.m_upperIdx], newPos, list[feb.m_upperIdx].transform.rotation);
        return go;
    }


    //public GameObject InstantiateLO(List<GameObject> list, int idx, string name, string tag)
    //{
    //    Vector3 newPos = new Vector3();
    //    newPos.x = gridCenters[idx].x;
    //    newPos.y = list[0].transform.position.y;
    //    newPos.z = gridCenters[idx].z;

    //    int cnt = Function.instance.checkIdx(name);


    //    //switch (name)
    //    //{
    //    //    case ("blink"):

    //    //        cnt = 0;

    //    //        //blinking();
    //    //        break;

    //    //    case ("shadow"):

    //    //        cnt = 1;

    //    //        break;

    //    //    case ("Disable(Clone)"):
    //    //        break;
    //    //}



    //    GameObject go = Instantiate(list[cnt], newPos, list[cnt].transform.rotation);

    //    //go.tr

    //    //LightObstacle p = go.transform.GetChild(0).GetComponent<LightObstacle>();

    //    return go;
    //}

    //public GameObject InstantiateLO<LightObstacle>(List<GameObject> list, int idx, string name)
    //{
    //    Vector3 newPos = new Vector3();
    //    newPos.x = gridCenters[idx].x;
    //    newPos.y = list[0].transform.position.y;
    //    newPos.z = gridCenters[idx].z;

    //    int cnt = 0;


    //    switch (name)
    //    {
    //        case ("blink"):
    //            cnt = 0;
    //            break;

    //        case ("shadow"):
    //            cnt = 1;
    //            break;

    //        case ("disable"):
    //            cnt = 2;
    //            break;
    //    }



    //    GameObject go = Instantiate(list[cnt], newPos, list[cnt].transform.rotation);

    //    //go.tr

    //    //LightObstacle p = go.transform.GetChild(0).GetComponent<LightObstacle>();

    //    return go;
    //}










    //public void DestroyObjects<T>(List<GameObject> list)
    public void DestroyObjects(List<GameObject> list)
    {
        if (list == null) return;

        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }

        list.Clear();
    }




    public GameObject InstantiateObject<T>(List<GameObject> list, int idx)
    {
        GameObject go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);
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
            //Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffectLights[UnityEngine.Random.Range(0, fieldEffectLights.Count)], mapTransform);
            Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffects[UnityEngine.Random.Range(0, fieldEffects.Count)], mapTransform);
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

    //public void switchPlayObjects(bool b)
    //{
    //    //player.SetActive(b);
    //    //player.GetComponent<Player>();

    //    for (int i = 0; i < enemies.Count; i++)
    //    {
    //        if (enemies[i] != null) enemies[i].SetActive(b);
    //    }

    //    for (int i = 0; i < fieldEffectLights.Count; i++)
    //    {
    //        if (fieldEffectLights[i] != null) fieldEffectLights[i].SetActive(b);
    //    }
    //}




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

