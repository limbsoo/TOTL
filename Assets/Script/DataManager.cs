using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    static GameObject container;

    static DataManager instance;


    string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "GameData.json";
    }

    public static DataManager Instance
    {
        get
        {
            if (!instance)
            {
                container = new GameObject();
                container.name = "DataManager";
                instance = container.AddComponent(typeof(DataManager)) as DataManager;
                DontDestroyOnLoad(container);
            }
            return instance;
        }
    }

    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
    //string GameDataFileName = "GameData.json";

    // --- 저장용 클래스 변수 --- //
    public GameData saveData = new GameData();


    public void LoadGameData()
    {
        //string filePath = Application.dataPath + "/Save/" + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<GameData>(FromJsonData);
            print("불러오기 완료");
        }

        else InitailizeData();
    }


    public void UpdateBlockData()
    {
        List<BlockData> BlockDatas = new List<BlockData>();

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            BlockData bd = new BlockData();
            MapPlacementBlock mpb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<MapPlacementBlock>();
            bd = mpb.GetBlockData();
            BlockDatas.Add(bd);
        }

        saveData.blockdata = BlockDatas;

        SaveFile();
    }


    // 저장하기
    public void UpdateStageData()
    {
        saveData.gold = StageManager.instance.RetunrGold();
        saveData.curWave = StageManager.instance.GetCurWave();

        if (saveData.maxWave < saveData.curWave) { saveData.maxWave = saveData.curWave; }
        saveData.playerStats.health = StageManager.instance.GetPlayerHealth();
    }


    public bool HaveSaveData()
    {
        //string filePath = Application.dataPath + "/Save/" + GameDataFileName;

        if (File.Exists(filePath)) { return true; }
        return false;
    }

    public void InitailizeData()
    {
        saveData.playerCharacterIdx = 0;
        saveData.stageModeIdx = 0;
        saveData.maxWave = 1;
        saveData.gold = 1000;
        saveData.curWave = 1;

        saveData.playerStats = new PlayerStats();
        //saveData.playerStats = StageManager.instance.LCS.player[0].GetComponent<PlayerStats>();
        saveData.blockdata = new List<BlockData>();

        //임시
        SelectPlayer(0);

        SaveFile();
    }

    public void InitSelectCharacter(int idx)
    {
        saveData.playerCharacterIdx = 0;
        saveData.stageModeIdx = 0;
        saveData.maxWave = 1;
        saveData.gold = 1000;
        saveData.curWave = 1;

        saveData.playerStats = new PlayerStats();
        //saveData.playerStats = StageManager.instance.LCS.player[0].GetComponent<PlayerStats>();
        saveData.blockdata = new List<BlockData>();

        //임시
        SelectPlayer(idx);

        SaveFile();
    }

    public void ResetStage()
    {
        saveData.playerCharacterIdx = 0;
        saveData.stageModeIdx = 0;
        saveData.gold = 1000;
        saveData.curWave = 1;

        saveData.playerStats = new PlayerStats();
        saveData.blockdata = new List<BlockData>();


        SaveFile();
    }



    public void SelectPlayer(int idx)
    {
        //string filePath = Application.dataPath + "ContstructSet/GCS";

        GameConstructSet gcs = Resources.Load<GameConstructSet>("ConstructSet/GCS");



        LevelConstructSet lcs = gcs.LevelConstructSet[gcs.currentLevel];

        PlayerStat playerStat = lcs.player[idx].GetComponent<PlayerStat>();

        saveData.playerStats.health = playerStat.health;
        saveData.playerStats.moveSpeed = playerStat.moveSpeed;
        saveData.playerStats.playerSkillKind = playerStat.playerSkillKind;
        saveData.playerStats.coolDown = playerStat.coolDown;

        //MainScene.instance.test.text = "data";

        //saveData.playerStats = StageManager.instance.LCS.player[idx].GetComponent<PlayerStat>();
    }



    public void SaveFile()
    {

        //if (!File.Exists(filePath))
        //{
        //    File.Create(filePath).Close();
        //}

        string ToJsonData = JsonUtility.ToJson(saveData, true);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "myJsonText");
        }



        //string ToJsonData = JsonUtility.ToJson(saveData, true);
        //string filePath = Application.dataPath + "/Save/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);

    }


    public GameData LoadData()
    {
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<GameData>(FromJsonData);
            print("불러오기 완료");
        }

        else { InitData(); }

        return saveData;
    }

    public void InitData()
    {
        saveData.playerCharacterIdx = 0;
        saveData.stageModeIdx = 0;
        saveData.gold = 1000;
        saveData.curWave = 1;
        saveData.maxWave = 1;

        saveData.playerStats = new PlayerStats();
        saveData.blockdata = new List<BlockData>();

        SelectPlayer(0);
        SaveFile();
    }

}
































