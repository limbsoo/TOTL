using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class DataManager : MonoBehaviour
{
    static GameObject container;

    static DataManager instance;
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

    string filePath;

    public GameData saveData;


    private void Awake()
    {
        filePath = Application.persistentDataPath + "GameData.json";
        saveData = new GameData();

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            saveData = JsonUtility.FromJson<GameData>(FromJsonData);
            print("불러오기 완료");
        }

        else { CreateFile(); }
    }

    void CreateFile()
    {
        saveData.playerCharacterIdx = -1;
        saveData.stageModeIdx = 0;
        saveData.maxWave = 1;
        saveData.gold = 1000;
        saveData.curWave = 1;
        saveData.playerStats = new PlayerStats();
        saveData.blockdata = new List<BlockData>();

        SaveFile();
    }

    public void SaveFile()
    {
        string ToJsonData = JsonUtility.ToJson(saveData, true);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "myJsonText");
        }

        File.WriteAllText(filePath, ToJsonData);
    }




    public GameData LoadData()
    {
        if (saveData == null) { print("no saveData"); }
        return saveData;
    }

    public void ResetProgressData()
    {
        //saveData = new GameData();

        saveData.playerCharacterIdx = -1;
        saveData.stageModeIdx = 0;
        saveData.gold = 1000;
        saveData.curWave = 1;
        saveData.playerStats = new PlayerStats();
        saveData.blockdata = new List<BlockData>();

        SaveFile();
    }


    public bool HaveProgressData()
    {
        if (saveData.playerCharacterIdx != -1) { return true; }
        return false;
    }



    public void SelectPlayer(int idx)
    {

        GameConstructSet gcs = Resources.Load<GameConstructSet>("ConstructSet/GCS");

        saveData.playerCharacterIdx = idx;
        LevelConstructSet lcs = gcs.LevelConstructSet[gcs.currentLevel];


        PlayerList playerList = Resources.Load<PlayerList>("ConstructSet/PlayerList");

        PlayerStats playerStat = playerList.lists[idx].Stats;


        //PlayerStat playerStat = lcs.player[idx].GetComponent<PlayerStat>();

        saveData.playerStats.health = playerStat.health;
        saveData.playerStats.moveSpeed = playerStat.moveSpeed;
        saveData.playerStats.playerSkillKind = playerStat.playerSkillKind;
        saveData.playerStats.coolDown = playerStat.coolDown;
        saveData.playerStats.effectValue = playerStat.effectValue;

        SaveFile();

    }

    public void UpdateStageProgress(int gold, int curWave, float health)
    {
        saveData.gold = gold;
        saveData.curWave = curWave;

        if (saveData.maxWave < curWave) { saveData.maxWave = curWave; }

        saveData.playerStats.health = health;

        SaveFile();
    }


    //public void InitData()
    //{
    //    //GameData GD = Resources.Load<GameData>("ConstructSet/GameDataConstructSet");


    //    saveData.playerCharacterIdx = -1;
    //    saveData.stageModeIdx = 0;
    //    saveData.maxWave = 1;
    //    saveData.gold = 1000;
    //    saveData.curWave = 1;

    //    saveData.playerStats = new PlayerStats();
    //    //saveData.playerStats = StageManager.instance.LCS.player[0].GetComponent<PlayerStats>();
    //    saveData.blockdata = new List<BlockData>();

    //    //임시
    //    SelectPlayer(0);

    //    SaveFile();
    //}




    //public void LoadGameData()
    //{
    //    //string filePath = Application.dataPath + "/Save/" + GameDataFileName;

    //    if (File.Exists(filePath))
    //    {
    //        string FromJsonData = File.ReadAllText(filePath);
    //        saveData = JsonUtility.FromJson<GameData>(FromJsonData);
    //        print("불러오기 완료");
    //    }

    //    else InitailizeData();
    //}


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







    //public void InitData()
    //{
    //    saveData.playerCharacterIdx = 0;
    //    saveData.stageModeIdx = 0;
    //    saveData.gold = 1000;
    //    saveData.curWave = 1;
    //    saveData.maxWave = 1;

    //    saveData.playerStats = new PlayerStats();
    //    saveData.blockdata = new List<BlockData>();

    //    SelectPlayer(0);
    //    SaveFile();
    //}

}
































