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
    public GameData data = new GameData();


    // 불러오기
    public void LoadGameData()
    {
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(FromJsonData);
            print("불러오기 완료");
        }

        else InitData();
    }

    public void InitData()
    {
        data.name = "Player";



        data.maxWave = 0;
        data.curWave = 0;
        data.health = 100;
        data.moveSpeed = 20;
        data.damamge = 2;
        data.coolDown = 2;
        data.skill = 0;
        data.gold = 0;
        data.febs = new List<BlockData>();
        //string jsonString = JsonConvert.SerializeObject(human);

        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }



    // 저장하기
    public void SaveGameData()
    {
        data.curWave += 1;


        List<BlockData> bds = new List<BlockData>();

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count; i++)
        {
            BlockData bd = new BlockData();
            FieldEffectBlock feb = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>();

            bd.m_downerIdx = feb.m_downerIdx;
            bd.m_upperIdx = feb.m_upperIdx;
            bd.start = feb.start;
            bd.end = feb.end;
            bd.lineNum = feb.lineNum;
            bd.blockName = feb.name;
            bd.position = feb.GetPos();
            bd.weight = feb.weight;

            bds.Add(bd);
        }

        data.febs = bds;

        string ToJsonData = JsonUtility.ToJson(data, true);

        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }


    public bool HaveSaveData()
    {
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;

        if (File.Exists(filePath))
        {
            return true;
        }

        else return false;
    }


    public void InitailizeData(int playerIdx)
    {
        data.name = "Player";
        data.playerCharacterIdx = playerIdx;


        data.maxWave = 0;
        data.curWave = 0;
        data.health = 100;
        data.moveSpeed = 20;
        data.damamge = 2;
        data.coolDown = 2;
        data.skill = 0;
        data.gold = 0;
        data.febs = new List<BlockData>();
        //string jsonString = JsonConvert.SerializeObject(human);

        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.dataPath + "/Save/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }


}