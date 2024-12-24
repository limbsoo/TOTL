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
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEditor.Experimental.GraphView.GraphView;
//using static UnityEditor.Progress;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UI.CanvasScaler;
using Vector3 = UnityEngine.Vector3;

public class StageManager : MonoBehaviour //해당 스테이지 판단하고 레벨 컨스트럭트사용해서 여러가지 함
{
    public static StageManager instance { get; private set; }
    private void Awake()
    {
        if (instance == null) { instance = this; }
        else Destroy(gameObject);

        //if (this.name == "StageManager")
        //{
        //    if (instance == null)
        //    {
        //        instance = this;
        //        DontDestroyOnLoad(gameObject);
        //    }
        //}
    }

    public GameConstructSet GCS;
    public PlayerList PlayerList;


    LevelConstructSet _LCS;

    Transform _mapTransform;
    int _targetScore;
    int _curWave;
    int _goldCnt;
    int _divide;
    Vector3[] _gridCenters;


    GameObject _player;
    List<GameObject> _enemies;
    List<GameObject> _fieldEffects;
    List<GameObject> _gold;
    List<GameObject> _keys;


    void Start()
    {
        //SoundManager.instance.PlaySound2D("Stage");
        SoundManager.instance.StopAllSound();
        SoundManager.instance.Play("PlayScene", SoundCatecory.BGM, true);


        SetStageFromLevelSet();
        SetStageFromSaveData();
        InitStage();
    }


    void Update()
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
                    _curWave++;

                    //DataManager.Instance.saveData.curWave = m_curWave;
                    //DataManager.Instance.UpdateStageData();
                    //DataManager.Instance.SaveGameData();

                    currentScore = 0;

                    StageUI.instance.UpdateText("curScore",0);
                    DataManager.Instance.UpdateStageProgress(_goldCnt, _curWave, _player.GetComponent<Player>().GetPlayerStats().health);


                    DestroyObjects(_enemies);
                    DestroyObjects(_fieldEffects);
                    DestroyObjects(_gold);

                    lifeCycle = 0;

                    IsArriveGate = false;


                    OnStageOver?.Invoke();


                    OnStageEnd?.Invoke();
                }

                break;
        }

    }


    LevelConstructSet LoadLevelSet(int idx) { return GCS.LevelConstructSet[idx]; }
    GameData LoadSaveData() { return DataManager.Instance.LoadData(); }
    void SetStageFromLevelSet()
    {
        _LCS = LoadLevelSet(0);
        _mapTransform = _LCS.map[0].transform;
        _targetScore = _LCS.numOfInitkeysRequired;
        _divide = 3; //tmp
    }

    void SetStageFromSaveData()
    {
        GameData data = LoadSaveData();
        _goldCnt = data.gold;
        _curWave = data.curWave;
    }

    void InitStage()
    {
        _gridCenters = Function.instance.GenerateGrid(_divide, _mapTransform);
        CreateKeys();

        StageUI.instance.OnDecideBlock = () =>
        {
            SetGoals();
            //CreatePlayer(_LCS.player[0]);

            CreatePlayer(PlayerList.lists[DataManager.Instance.saveData.playerCharacterIdx].player);


            CreateFieldEffects();
            CreateEnemies();
            CreateGolds();

            Sstate = StageState.Play;
        };
    }





    public LevelConstructSet ReturnLevelSet() { return _LCS; }

    public Transform ReturnMapTransform() { return _mapTransform; }

    public Vector3[] ReturnGridCenters() { return _gridCenters; }

    public int ReturnTargetScore() { return _targetScore; }











    public Action<float> OnPlayerDamaged;
    public Action<PopupType> OnPopUpOpen;
    public int curStage;
    public int currentScore;
    public int lifeCycle;
    public static StageState Sstate = StageState.Edit;
    public Action OnStageEnd;
    public int waveTime = 0;
    public int stageTime = 0;
    public List<int> goalList;
    public bool IsArriveGate = false;
    public Action<string, float> OnUpadateText;
    public Action<float, float> OnEnemyCnt;




    void CreateKeys()
    {
        _keys = new List<GameObject>();

        for (int i = 0; i < _gridCenters.Length; i++)
        {
            Vector3 pos = new Vector3(_gridCenters[i].x, _gridCenters[i].y + 1, _gridCenters[i].z);
            _keys.Add(InstantiateObject<Goal>(_LCS.Goal[0], pos, _LCS.Goal[0].transform.rotation));
            _keys[i].SetActive(false);
        }
    }




    void CreatePlayer(GameObject go)
    {
        if (_player == null)
        {
            _player = InstantiateObject<Player>(go, go.transform.position, go.transform.rotation);
            Player playerComponent = _player.GetComponent<Player>();

            playerComponent.OnPlayerDamaged += TriggerPlayerDamaged;
            playerComponent.OnPlayerEvent += PlayerEventHandler;
            //playerComponent.OnnnUseSkill += PlayerUseSkillHandler;


            playerComponent.OnSkillEffect += (skillKind, value, coolDown) => OnPlayerUseSkill?.Invoke(skillKind, value, coolDown);

            OnStageOver += playerComponent.InitState;
        }
    }

    //public void PlayerUseSkillHandler(PlayerSkillKinds playerSkillKinds)
    //{
    //    OnPlayerUseSkill.Invoke(playerSkillKinds);
    //}

    public Action OnClickSkillButton;

    public event Action OnStageOver;

    public event Action <PlayerSkillKinds, float, float> OnPlayerUseSkill;


    void CreateFieldEffects()
    {
        _fieldEffects = new List<GameObject>();

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            MapPlacementBlock mpb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<MapPlacementBlock>();
            BlockData bd = mpb.GetBlockData();

            Vector3 pos = new Vector3(_gridCenters[bd.lineNum].x, _LCS.FieldEffect[0].transform.position.y, _gridCenters[bd.lineNum].z);
            GameObject go = InstantiateObject<FieldEffect>(_LCS.FieldEffect[(int)bd.fieldKinds - 1], pos, _LCS.FieldEffect[0].transform.rotation);

            FieldEffect fe = go.GetComponent<FieldEffect>();

            fe.Init(bd);

            _fieldEffects.Add(go);
        }
    }



    void CreateEnemies()
    {
        _enemies = new List<GameObject>();
        int eliteCnt = _curWave - _LCS.endArrage;
        int cnt = _LCS.numOfInitEnemy + _LCS.enemyIncreasePerWave * (_curWave + 1);

        if (eliteCnt > 0) 
        {
            for (int i = 0; i < eliteCnt; i++)
            {
                //UnityEngine.Quaternion quat = new UnityEngine.Quaternion(_LCS.enemy[1].transform.rotation.x, UnityEngine.Random.Range(-180, 180), _LCS.enemy[1].transform.rotation.z);

                //Vector3 vector3 = new Vector3(_LCS.enemy[1].transform.rotation.x, UnityEngine.Random.Range(-180, 180), _LCS.enemy[1].transform.rotation.z);
                _enemies.Add(InstantiateObject<Enemy>(_LCS.enemy[1], GetRandomPosBasedOnCenterMap(_LCS.enemy[1]), _LCS.enemy[1].transform.rotation));
            }

            cnt -= eliteCnt;
        }


        for (int i = 0; i < cnt; i++)
        {
            _enemies.Add(InstantiateObject<Enemy>(_LCS.enemy[0], GetRandomPosBasedOnCenterMap(_LCS.enemy[0]), UnityEngine.Quaternion.Euler(0, UnityEngine.Random.Range(0f, 360f), 0)));
        }

        OnEnemyCnt.Invoke(cnt, eliteCnt < 0 ? 0 : eliteCnt);
    }

    void CreateGolds()
    {
        _gold = new List<GameObject>();

        for (int i = 0; i < _LCS.Item.Count; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                Vector3 randomPos = Function.instance.GetRandomPositionInMap(_LCS.Item[i], _mapTransform);
                randomPos.y = _LCS.Item[i].transform.position.y;
                GameObject go1 = Instantiate(_LCS.Item[i], randomPos, _LCS.Item[i].transform.rotation);
                _gold.Add(go1);
            }


        }
    }



    GameObject InstantiateObject<T>(GameObject gob, Vector3 pos, UnityEngine.Quaternion rot) where T : UnityEngine.Component, Spawn
    {
        GameObject go = Instantiate(gob, pos, rot);
        T t = go.GetComponent<T>();
        t?.Init(_gridCenters, _mapTransform);
        return go;
    }

    void SetGoals()
    {
        goalList.Clear();

        for (int i = 0; i < _targetScore; i++)
        {
            while (true)
            {
                int randNum = UnityEngine.Random.Range(0, 9);

                while (randNum == 4) { randNum = UnityEngine.Random.Range(0, 9); }

                int num = goalList.Find(x => x == randNum);

                if (num == 0)
                {
                    goalList.Add(randNum);
                    _keys[randNum].SetActive(true);
                    break;
                }
            }
        }
    }



    Vector3 GetRandomPosBasedOnCenterMap(GameObject go)
    {
        Vector3 randomPos = Function.instance.GetRandomPositionInMap(_fieldEffects[UnityEngine.Random.Range(0, _fieldEffects.Count)], _mapTransform);

        while (Function.instance.IsInsideCircle(randomPos, new Vector3(_gridCenters[4].x, 0, _gridCenters[4].z), 10f))
        {
            randomPos = Function.instance.GetRandomPositionInMap(_fieldEffects[UnityEngine.Random.Range(0, _fieldEffects.Count)], _mapTransform);
        }

        randomPos.y = go.transform.position.y;

        return randomPos;
    }





    public Action<PlayerEvent, float> OnPlayerEvent;

    void PlayerEventHandler(PlayerEvent playerEvent, float data)
    {
        //switch (playerEvent) 
        //{
        //    case PlayerEvent.Damaged:
        //        break;

        //    case PlayerEvent.GetGold:
        //        break;

        //    case PlayerEvent.GetKey:
        //        break;

        //    case PlayerEvent.ArriveGoal:
        //        break;

        //    case PlayerEvent.SkillCoolDownUpdate:
        //        break;

        //    case PlayerEvent.Die:
        //        break;
        //}


        OnPlayerEvent.Invoke(playerEvent, data);


        //if (f <= 0) { OnPopUpOpen.Invoke(PopupType.GameOver); }
        //else { OnPlayerDamaged.Invoke(f); }
    }



    


    void TriggerPlayerDamaged(float f)
    {
        if (f <= 0)
        {
            SoundManager.instance.Play("GameOver", SoundCatecory.Effect, false);
            OnPopUpOpen.Invoke(PopupType.GameOver);
            DataManager.Instance.ResetProgressData();
        }
        else { OnPlayerDamaged.Invoke(f); }
    }




    public float GetPlayerHealth()
    {
        return _player.GetComponent<Player>().GetPlayerStats().health;
    }

    public PlayerStats GetPlayerStats()
    {
        return _player.GetComponent<Player>().GetPlayerStats();
    }


    public int GetCurWave() { return _curWave; }








    public void arriveGoal(GameObject g)
    {
        g.gameObject.SetActive(false);
        currentScore++;
        SoundManager.instance.Play("eatItem", SoundCatecory.Effect, false);
        //TextManager.instance.UpdateTexts();

        OnUpadateText.Invoke("curScore", currentScore);

        if (currentScore == _targetScore)
        {
            Instantiate(_LCS.gate[0], _LCS.gate[0].transform.position, _LCS.gate[0].transform.rotation);
        }

    }

    public void arriveGate(GameObject g)
    {
        IsArriveGate = true;
        Destroy(g);
    }



    public void UseGold(int cost)
    {
        _goldCnt -= cost;
        StageUI.instance.UpdateText("gold", _goldCnt);
    }

    public bool IsUnderCost(int cost)
    {
        if (_goldCnt >= cost) { return false; }
        return true;
    }
    
    public int RetunrGold()
    {
        return _goldCnt;
    }

    public void UpdateGold(int value)
    {
        SoundManager.instance.Play("eatItem", SoundCatecory.Effect, false);
        _goldCnt += value;
        StageUI.instance.UpdateText("gold", _goldCnt);
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

}

