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
using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
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
    public LevelConstructSet LCS;

    public GameObject player;
    public List<GameObject> enemies;
    public List<GameObject> fieldEffects;


    public List<GameObject> items;


    public int curStage;
    public int targetScore;
    public int currentScore;
    public Vector3[] gridCenters;
    public Transform mapTransform;



    public int lifeCycle;



    public int divide;





    public Action stageLevelSetEnd;


    public List<GameObject> goals;


    public static StageState Sstate = StageState.Edit;

    public Action OnStageEnd;



    public int waveTime = 0;
    public int stageTime = 0;
    Coroutine waveTimeCoroutine = null;
    Coroutine stageTimeCoroutine = null;


    public List<int> goalList;

    public int m_curWave;

    public Action OnContinueWave;

    public bool IsArriveGate = false;


    public void Start()
    {
        DataManager.Instance.LoadGameData();
        m_curWave = DataManager.Instance.data.curWave;
        InitStage();
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
                if (IsArriveGate)
                {
                    Sstate = StageState.Edit;
                    currentScore = 0;
                    DestroyObjects(enemies);
                    //DestroyObjects(fieldEffectLights);
                    DestroyObjects(fieldEffects);

                    IsArriveGate = false;

                    DataManager.Instance.SaveGameData();

                    OnStageEnd?.Invoke();
                }

                break;
        }

    }


    void InitStage()
    {
        //fieldEffects = new List<GameObject>();
        //enemies = new List<GameObject>();
        //items = new List<GameObject>();
        
        ////하나로 만드는게
        //goals = new List<GameObject>();
        //goalList = new List<int> { };

        LoadLevelSet();
        gridCenters = Function.instance.GenerateGrid(divide, mapTransform);

        goals = new List<GameObject>();

        for (int i = 0; i < gridCenters.Length; i++)
        {
            Vector3 pos = new Vector3(gridCenters[i].x, gridCenters[i].y + 1, gridCenters[i].z);
            goals.Add(InstantiateObject<Goal>(LCS.Goal[0], pos, LCS.Goal[0].transform.rotation));
            goals[i].SetActive(false);
        }


        UIManager.instance.OnDecideBlock = () => 
        {
            SetGoals();
            SpwanObjects(); 
        };
    }

    public void SpwanObjects()
    {
        if (player == null)
        {
            player = InstantiateObject<Player>(LCS.player[0], LCS.player[0].transform.position, LCS.player[0].transform.rotation);
        }
 
        

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();
            Vector3 pos = new Vector3(gridCenters[feb.lineNum].x, LCS.FieldEffect[0].transform.position.y, gridCenters[feb.lineNum].z);
            GameObject go = InstantiateObject<FieldEffect>(LCS.FieldEffect[feb.m_upperIdx], pos, LCS.FieldEffect[0].transform.rotation);

            FieldEffect fe = go.GetComponent<FieldEffect>();
            fe.Init(feb.start, feb.end, feb.lineNum, feb.m_upperIdx, feb.m_downerIdx);
            fieldEffects.Add(go);
        }

        //if (fieldEffects.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);

        if (fieldEffects.Count != 0)
        {
            //enemies = InstantiateEnemies<Enemy>(LCS.enemy, LCS.enemyCnt + LCS.increaseEnemyPerWave * (m_curWave + 1));
            enemies = InstantiateEnemies<Enemy>(LCS.enemy, LCS.numOfInitEnemy + LCS.enemyIncreasePerWave * (m_curWave + 1));
        }
        
        int randomnumber = UnityEngine.Random.Range(0, 9);

        Vector3 newPos = new Vector3(gridCenters[randomnumber].x, LCS.Item[0].transform.position.y, gridCenters[randomnumber].z);
        GameObject go1 = Instantiate(LCS.Item[0], newPos, LCS.Item[0].transform.rotation);
        items.Add(go1);


        Sstate = StageState.Play;

    }

    public GameObject InstantiateObject<T>(GameObject gob, Vector3 pos, UnityEngine.Quaternion rot) where T : UnityEngine.Component, Spawn
    {
        GameObject go = Instantiate(gob, pos, rot);
        T t = go.GetComponent<T>();
        if (t != null) t.Init(gridCenters, mapTransform);
        return go;
    }


    public List<GameObject> InstantiateEnemies<T>(List<GameObject> list, int cnt)
    {
        List<GameObject> lgo = new List<GameObject>();

        for (int i = 0; i < cnt; i++)
        {
            int randIdx = UnityEngine.Random.Range(0, list.Count);
         
            Vector3 randomPos = Function.instance.GetRandomPositionInMap(fieldEffects[UnityEngine.Random.Range(0, fieldEffects.Count)], mapTransform);

            while (Function.instance.IsInsideCircle(randomPos, new Vector3(gridCenters[4].x, 0, gridCenters[4].z), 10f))
            {
                randomPos = Function.instance.GetRandomPositionInMap(fieldEffects[UnityEngine.Random.Range(0, fieldEffects.Count)], mapTransform);
            }


            randomPos.y = list[randIdx].transform.position.y;
            GameObject go = Instantiate(list[randIdx], randomPos, list[randIdx].transform.rotation);
            T p = go.GetComponent<T>();
            lgo.Add(go);
        }
        return lgo;
    }




    void SetGoals()
    {
        goalList.Clear();

        for (int i = 0; i < targetScore; i++)
        {
            while (true)
            {
                int randNum = UnityEngine.Random.Range(0, 9);

                while (randNum == 4)
                {
                    randNum = UnityEngine.Random.Range(0, 9);
                }

                
                int num = goalList.Find(x => x == randNum);

                if (num == 0)
                {
                    goalList.Add(randNum);
                    goals[randNum].SetActive(true);
                    break;
                }
            }
        }
    }


    public void LoadLevelSet()
    {
        LCS = GCS.LevelConstructSet[curStage];
        mapTransform = LCS.map[0].transform;
        targetScore = LCS.numOfInitkeysRequired;
    }




    public void arriveGoal(GameObject g)
    {
        g.gameObject.SetActive(false);
        currentScore++;

        if(currentScore == targetScore)
        {
            Instantiate(LCS.gate[0], LCS.gate[0].transform.position, LCS.gate[0].transform.rotation);
        }

    }

    public void arriveGate(GameObject g)
    {
        IsArriveGate = true;
        Destroy(g );
    }



    //public int idx = 0;

    //public GameObject InstantiateFieldEffect(List<GameObject> list, FieldEffectBlock feb)
    //{
    //    Vector3 newPos = new Vector3(gridCenters[feb.lineNum].x, list[0].transform.position.y, gridCenters[feb.lineNum].z);
    //    GameObject go = Instantiate(list[feb.m_upperIdx], newPos, list[feb.m_upperIdx].transform.rotation);
    //    return go;
    //}


    //public GameObject InstantiateObject<T>(List<GameObject> list, int idx) where T : UnityEngine.Component, Spawn
    //{
    //    GameObject go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);
    //    T t = go.GetComponent<T>();
    //    if (t != null) t.Init(gridCenters, mapTransform);
    //    return go;
    //}

    //public GameObject InstantiateGoal(int idx)
    //{
    //    Vector3 newPos = gridCenters[idx];
    //    newPos.y += 1;
    //    GameObject go = Instantiate(LCS.Goal[0], newPos, LCS.Goal[0].transform.rotation);
    //    go.SetActive(false);
    //    return go;
    //}




    //public GameObject InstantiateObject<T>(List<GameObject> list, int idx) where T : UnityEngine.Component
    //{
    //    // 해당 인덱스의 게임 오브젝트를 Instantiate
    //    GameObject go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);

    //    // T 타입의 컴포넌트를 가져옴
    //    T p = go.GetComponent<T>();

    //    // 만약 p가 null이 아니라면, Init 메소드 호출
    //    if (p != null)
    //    {
    //        p.Init(gridCenters, mapTransform); // T는 Init 메소드를 가진 타입이어야 함
    //    }

    //    return go;
    //}


    //public GameObject InstantiateObject<T>(List<GameObject> list, int idx) where T : class
    //{

    //    GameObject go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);
    //    T p = go.GetComponent<T>();
    //    p.Init(gridCenters, mapTransform);

    //    return go;
    //}




    //public GameObject InstantiateObject<T>(List<GameObject> list, int idx)
    //{





    //    //GameObject go = Instantiate(list[idx], list[idx].transform.position, list[idx].transform.rotation);
    //    //T p = go.GetComponent<T>();

    //    //p.Init(gridCenters, mapTransform);

    //    //return go;
    //}




















    //public int startTime;
    //public int endTime;













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

    //public List<GameObject> InstantiateObjectsInLightRange<T>(List<GameObject> list, int cnt)
    //{
    //    List<GameObject> lgo = new List<GameObject>();

    //    for (int i = 0; i < cnt; i++)
    //    {
    //        int randEnemyIdx = UnityEngine.Random.Range(0, list.Count);
    //        //Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffectLights[UnityEngine.Random.Range(0, fieldEffectLights.Count)], mapTransform);
    //        Vector3 randomPos = Function.instance.GetRandomPositionInLightRange(fieldEffects[UnityEngine.Random.Range(0, fieldEffects.Count)], mapTransform);
    //        randomPos.y = list[randEnemyIdx].transform.position.y;
    //        GameObject go = Instantiate(list[randEnemyIdx], randomPos, list[randEnemyIdx].transform.rotation);
    //        T p = go.GetComponent<T>();
    //        lgo.Add(go);

    //    }

    //    return lgo;
    //}










    //public void EndStage()
    //{
    //    currentScore = 10;
    //}




    public UnityEvent OnStageClear;

 





    //이벤트----------------------------------------------------------------------------------------//

    private void OnEnable()
    {
        //EventManager.instance.OnStageEnd += EndStage;
    }

    private void OnDisable()
    {
        //EventManager.instance.OnStageEnd -= EndStage;
    }



}

