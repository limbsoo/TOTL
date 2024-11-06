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

    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
    string GameDataFileName = "GameData.json";

    // --- 저장용 클래스 변수 --- //
    public GameData saveData = new GameData();


    public void LoadGameData()
    {
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

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
        saveData.gold = StageManager.instance.gold;
        saveData.curWave = StageManager.instance.GetCurWave();

        if (saveData.maxWave < saveData.curWave) { saveData.maxWave = saveData.curWave; }
        saveData.playerStats.health = StageManager.instance.GetPlayerHealth();
    }


    public bool HaveSaveData()
    {
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

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

        //saveData.playerStats = StageManager.instance.LCS.player[idx].GetComponent<PlayerStat>();
    }



    public void SaveFile()
    {
        string ToJsonData = JsonUtility.ToJson(saveData, true);
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }


    //public void InitailizeData(int playerIdx)
    //{

    //    saveData.playerCharacterIdx = 0;

    //    saveData.stageModeIdx = 0;
    //    saveData.maxWave = 0;
    //    saveData.gold = 1000;
    //    saveData.curWave = 0;

    //    saveData.playerStats = new PlayerStats();
    //    saveData.blockdata = new List<BlockData>();

    //    string ToJsonData = JsonUtility.ToJson(saveData, true);
    //    string filePath = Application.dataPath + "/Save/" + GameDataFileName;
    //    File.WriteAllText(filePath, ToJsonData);
    //}


}



































//public class DataManager : MonoBehaviour

//{
//    static GameObject container;

//    static DataManager instance;
//    public static DataManager Instance
//    {
//        get
//        {
//            if (!instance)
//            {
//                container = new GameObject();
//                container.name = "DataManager";
//                instance = container.AddComponent(typeof(DataManager)) as DataManager;
//                DontDestroyOnLoad(container);
//            }
//            return instance;
//        }
//    }

//    // --- 게임 데이터 파일이름 설정 ("원하는 이름(영문).json") --- //
//    string GameDataFileName = "GameData.json";

//    // --- 저장용 클래스 변수 --- //
//    public GameData data = new GameData();


//    // 불러오기
//    public void LoadGameData()
//    {
//        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

//        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;

//        if (File.Exists(filePath))
//        {
//            string FromJsonData = File.ReadAllText(filePath);
//            data = JsonUtility.FromJson<GameData>(FromJsonData);
//            print("불러오기 완료");
//        }

//        else InitData();
//    }

//    //public void InitData()
//    //{
//    //    data.name = "Player";



//    //    data.maxWave = 0;
//    //    data.curWave = 1;
//    //    data.health = 100;
//    //    data.moveSpeed = 30;
//    //    data.damamge = 2;
//    //    data.coolDown = 2;
//    //    data.skill = 0;
//    //    data.gold = 1000;
//    //    data.blockdata = new List<BlockData>();
//    //    //string jsonString = JsonConvert.SerializeObject(human);

//    //    string ToJsonData = JsonUtility.ToJson(data, true);
//    //    string filePath = Application.dataPath + "/Save/" + GameDataFileName;
//    //    File.WriteAllText(filePath, ToJsonData);
//    //}



//    // 저장하기
//    public void SaveGameData()
//    {
//        //data.curWave += 1;


//        List<BlockData> bds = new List<BlockData>();

//        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
//        {
//            BlockData bd = new BlockData();
//            MapPlacementBlock mpb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<MapPlacementBlock>();

//            bd = mpb.GetBlockData();

//            //bd.m_downerIdx = feb.m_downerIdx;
//            //bd.m_upperIdx = feb.m_upperIdx;
//            //bd.start = feb.start;
//            //bd.end = feb.end;
//            //bd.lineNum = feb.lineNum;
//            //bd.blockName = feb.name;
//            //bd.position = feb.GetPos();
//            //bd.weight = feb.weight;

//            bds.Add(bd);
//        }

//        data.blockdata = bds;

//        string ToJsonData = JsonUtility.ToJson(data, true);

//        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

//        File.WriteAllText(filePath, ToJsonData);
//    }


//    public bool HaveSaveData()
//    {
//        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

//        if (File.Exists(filePath))
//        {
//            return true;
//        }

//        else return false;
//    }


//    public void InitailizeData(int playerIdx)
//    {
//        data.name = "Player";
//        data.playerCharacterIdx = playerIdx;


//        data.maxWave = 0;
//        data.curWave = 0;
//        data.health = 100;
//        data.moveSpeed = 20;
//        data.damamge = 2;
//        data.coolDown = 2;
//        data.skill = 0;
//        data.gold = 0;
//        data.blockdata = new List<BlockData>();
//        //string jsonString = JsonConvert.SerializeObject(human);

//        string ToJsonData = JsonUtility.ToJson(data, true);
//        string filePath = Application.dataPath + "/Save/" + GameDataFileName;
//        File.WriteAllText(filePath, ToJsonData);
//    }


//}















//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;
//using Newtonsoft.Json;
//using System;

////[Serializable]
//public class DataManager : MonoBehaviour
//{
//    static GameObject container;
//    static DataManager instance;
//    public static DataManager Instance
//    {
//        get
//        {
//            if (!instance)
//            {
//                container = new GameObject();
//                container.name = "DataManager";
//                instance = container.AddComponent(typeof(DataManager)) as DataManager;
//                DontDestroyOnLoad(container);
//            }
//            return instance;
//        }
//    }
//    static string GameDataFileName = "GameData.json";
//    static string filePath = Application.persistentDataPath + "/Save/" + "GameData.json";
//    public GameData data = new GameData();

//    public void LoadGameData()
//    {


//        if (File.Exists(filePath))
//        {
//            //string data = File.ReadAllText(Application.persistentDataPath + "/data.json");
//            data = JsonConvert.DeserializeObject<GameData>(File.ReadAllText(Application.persistentDataPath + "/Save/" + "GameData.json"));





//            //string FromJsonData = File.ReadAllText(filePath);
//            //data = JsonUtility.FromJson<GameData>(FromJsonData);
//            //print("불러오기 완료");
//        }

//        else InitData();
//    }

//    public void InitData()
//    {
//        data = new GameData();

//        data.name = "Player";
//        data.maxWave = 0;
//        data.curWave = 0;
//        data.health = 100;
//        data.moveSpeed = 20;
//        data.damamge = 2;
//        data.coolDown = 2;
//        data.skill = 0;
//        data.gold = 0;
//        data.febs = new List<BlockData>();


//        data.FEs = new List<FieldEffect>();
//        string jsonString = JsonConvert.SerializeObject(data);
//        File.WriteAllText(Application.persistentDataPath + "/Save/" + "GameData.json", jsonString);
//    }



//    // 저장하기
//    public void SaveGameData()
//    {
//        data.curWave += 1;

//        data.FEs = new List<FieldEffect>();

//        for (int i = 0; i < StageManager.instance.fieldEffects.Count;i++)
//        {
//            data.FEs.Add(StageManager.instance.fieldEffects[i].GetComponent<FieldEffect>());
//        }



//        //data.FEs = 
//        //string jsonString = JsonConvert.SerializeObject(data);

//        string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented, new JsonSerializerSettings
//        {
//            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
//        });

//        File.WriteAllText(Application.persistentDataPath + "/Save/" + "GameData.json", jsonString);


//        //List<BlockData> bds = new List<BlockData>();

//        //for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
//        //{
//        //    BlockData bd = new BlockData();
//        //    FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();

//        //    bd.m_downerIdx = feb.m_downerIdx;
//        //    bd.m_upperIdx = feb.m_upperIdx;
//        //    bd.start = feb.start;
//        //    bd.end = feb.end;
//        //    bd.lineNum = feb.lineNum;
//        //    bd.blockName = feb.name;
//        //    bd.position = feb.GetPos();


//        //    bds.Add(bd);
//        //}

//        //data.febs = bds;

//        //string ToJsonData = JsonUtility.ToJson(data, true);

//        //string filePath = Application.dataPath + "/Save/" + GameDataFileName;

//        //File.WriteAllText(filePath, ToJsonData);
//    }
//}











