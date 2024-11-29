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
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;
using Vector3 = UnityEngine.Vector3;

public class StageManager : MonoBehaviour //�ش� �������� �Ǵ��ϰ� ���� ����Ʈ��Ʈ����ؼ� �������� ��
{
    public static StageManager instance { get; private set; }
    private void Awake()
    {
        if (this.name == "StageManager")
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public GameConstructSet GCS;
    public LevelConstructSet LCS;

    GameObject m_player;
    List<GameObject> m_enemies;
    List<GameObject> m_fieldEffects;
    List<GameObject> m_items;

    int m_curWave;
    int m_gold;





    public Action<float> OnPlayerDamaged;
    public Action<PopupType> OnPopUpOpen;












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
    //Coroutine waveTimeCoroutine = null;
    //Coroutine stageTimeCoroutine = null;


    public List<int> goalList;



    public Action OnContinueWave;

    public bool IsArriveGate = false;


    //public int gold;


    public Action<string, float> OnUpadateText;



    public void Start()
    {
        LoadData();
        InitStage();
    }

    public void LoadData()
    {
        LCS = GCS.LevelConstructSet[curStage];
        mapTransform = LCS.map[0].transform;
        targetScore = LCS.numOfInitkeysRequired;
        GameData data = DataManager.Instance.LoadData();
        m_gold = data.gold;
        m_curWave = data.curWave;
    }

    public void InitStage()
    {
        m_enemies = new List<GameObject>();
        m_fieldEffects = new List<GameObject>();
        m_items = new List<GameObject>();
        goals = new List<GameObject>();

        gridCenters = Function.instance.GenerateGrid(divide, mapTransform);

        for (int i = 0; i < gridCenters.Length; i++)
        {
            Vector3 pos = new Vector3(gridCenters[i].x, gridCenters[i].y + 1, gridCenters[i].z);
            goals.Add(InstantiateObject<Goal>(LCS.Goal[0], pos, LCS.Goal[0].transform.rotation));
            goals[i].SetActive(false);
        }

        StageUI.instance.OnDecideBlock = () =>
        {
            SetGoals();
            SpwanObjects();
        };
    }


    void SetGoals()
    {
        goalList.Clear();

        for (int i = 0; i < targetScore; i++)
        {
            while (true)
            {
                int randNum = UnityEngine.Random.Range(0, 9);

                while (randNum == 4) { randNum = UnityEngine.Random.Range(0, 9); }

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
                    m_curWave++;

                    //DataManager.Instance.saveData.curWave = m_curWave;
                    //DataManager.Instance.UpdateStageData();
                    //DataManager.Instance.SaveGameData();

                    currentScore = 0;


                    DestroyObjects(m_enemies);
                    DestroyObjects(m_fieldEffects);

                    IsArriveGate = false;
                    


                    OnStageEnd?.Invoke();
                }

                break;
        }

    }

    public float GetPlayerHealth()
    {
        return m_player.GetComponent<Player>().GetPlayerStats().health;
    }

    public PlayerStats GetPlayerStats()
    {
        return m_player.GetComponent<Player>().GetPlayerStats();
    }


    public int GetCurWave() { return m_curWave; }



    public void TriggerPlayerDamaged(float f)
    {
        if (f <= 0) { OnPopUpOpen.Invoke(PopupType.GameOver); }
        else { OnPlayerDamaged.Invoke(f); }
    }


    public void SpwanObjects()
    {
        if (m_player == null)
        {
            m_player = InstantiateObject<Player>(LCS.player[0], LCS.player[0].transform.position, LCS.player[0].transform.rotation);
            Player p = m_player.GetComponent<Player>();

            p.OnPlayerDamaged += TriggerPlayerDamaged;

        }
 
        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            MapPlacementBlock mpb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<MapPlacementBlock>();
            BlockData bd = mpb.GetBlockData();


            Vector3 pos = new Vector3(gridCenters[bd.lineNum].x, LCS.FieldEffect[0].transform.position.y, gridCenters[bd.lineNum].z);
            GameObject go = InstantiateObject<FieldEffect>(LCS.FieldEffect[(int)bd.fieldKinds - 1], pos, LCS.FieldEffect[0].transform.rotation);

            FieldEffect fe = go.GetComponent<FieldEffect>();
            //fe.Init(feb.start, feb.end, feb.lineNum, feb.m_upperIdx, feb.m_downerIdx);

            fe.Init(bd);


            m_fieldEffects.Add(go);
        }

        //if (fieldEffects.Count != 0) enemies = InstantiateObjectsInLightRange<Enemy>(LCS.enemy, LCS.enemyCnt);

        if (m_fieldEffects.Count != 0)
        {
            //enemies = InstantiateEnemies<Enemy>(LCS.enemy, LCS.enemyCnt + LCS.increaseEnemyPerWave * (m_curWave + 1));
            m_enemies = InstantiateEnemies<Enemy>(LCS.enemy, LCS.numOfInitEnemy + LCS.enemyIncreasePerWave * (m_curWave + 1));
        }

        //�굵 ������ ��ġ�� ������



        for(int i = 0; i < LCS.Item.Count;i++)
        {
            Vector3 randomPos = Function.instance.GetRandomPositionInMap(LCS.Item[i], mapTransform);
            randomPos.y = LCS.Item[i].transform.position.y;

            GameObject go1 = Instantiate(LCS.Item[i], randomPos, LCS.Item[i].transform.rotation);
            m_items.Add(go1);
        }


        Sstate = StageState.Play;

    }




    public GameObject InstantiateObject<T>(GameObject gob, Vector3 pos, UnityEngine.Quaternion rot) where T : UnityEngine.Component, Spawn
    {
        GameObject go = Instantiate(gob, pos, rot);
        T t = go.GetComponent<T>();
        if (t != null) t.Init(gridCenters, mapTransform);
        return go;
    }


    public Action<float, float> OnEnemyCnt;


    public List<GameObject> InstantiateEnemies<T>(List<GameObject> list, int cnt)
    {
        int eliteCnt = m_curWave - LCS.endArrage;
        List<GameObject> lgo = new List<GameObject>();


        //TextManager.instance.UpdateEnemyCnt(cnt, eliteCnt < 0 ? 0 : eliteCnt);

        if (eliteCnt > 0) 
        {
            for (int i = 0; i < cnt - eliteCnt; i++)
            {
                int randIdx = 0;

                Vector3 randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);

                while (Function.instance.IsInsideCircle(randomPos, new Vector3(gridCenters[4].x, 0, gridCenters[4].z), 10f))
                {
                    randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);
                }


                randomPos.y = list[randIdx].transform.position.y;
                GameObject go = Instantiate(list[randIdx], randomPos, list[randIdx].transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }

            for (int i = 0; i < eliteCnt; i++)
            {
                int randIdx = 1;

                Vector3 randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);

                while (Function.instance.IsInsideCircle(randomPos, new Vector3(gridCenters[4].x, 0, gridCenters[4].z), 10f))
                {
                    randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);
                }


                randomPos.y = list[randIdx].transform.position.y;
                GameObject go = Instantiate(list[randIdx], randomPos, list[randIdx].transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }

        else
        {
            for (int i = 0; i < cnt; i++)
            {
                int randIdx = 0;

                Vector3 randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);

                while (Function.instance.IsInsideCircle(randomPos, new Vector3(gridCenters[4].x, 0, gridCenters[4].z), 10f))
                {
                    randomPos = Function.instance.GetRandomPositionInMap(m_fieldEffects[UnityEngine.Random.Range(0, m_fieldEffects.Count)], mapTransform);
                }


                randomPos.y = list[randIdx].transform.position.y;
                GameObject go = Instantiate(list[randIdx], randomPos, list[randIdx].transform.rotation);
                T p = go.GetComponent<T>();
                lgo.Add(go);
            }
        }

        OnEnemyCnt.Invoke(cnt, eliteCnt < 0 ? 0 : eliteCnt);

        return lgo;
    }







    public void arriveGoal(GameObject g)
    {
        g.gameObject.SetActive(false);
        currentScore++;
        //TextManager.instance.UpdateTexts();

        OnUpadateText.Invoke("curScore", currentScore);

        if (currentScore == targetScore)
        {
            Instantiate(LCS.gate[0], LCS.gate[0].transform.position, LCS.gate[0].transform.rotation);
        }

    }

    public void arriveGate(GameObject g)
    {
        IsArriveGate = true;
        Destroy(g );
    }



    public void UseGold(int cost)
    {
        m_gold -= cost;
    }

    public bool IsUnderCost(int cost)
    {
        if (m_gold >= cost) { return false; }
        return true;
    }
    
    public int RetunrGold()
    {
        return m_gold;
    }

    public void UpdateGold(int value)
    {
        m_gold += value;
    }


    public void DestroyObjects(List<GameObject> list)
    {
        if (list == null) return;

        for (int i = 0; i < list.Count; i++)
        {
            Destroy(list[i]);
        }

        list.Clear();
    }







    public UnityEvent OnStageClear;

}

