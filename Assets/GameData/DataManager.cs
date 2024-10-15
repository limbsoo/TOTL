using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    // ---싱글톤으로 선언--- //
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
        string filePath = Application.dataPath + "/" + GameDataFileName;
        
        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
            // 저장된 파일 읽어오고 Json을 클래스 형식으로 전환해서 할당
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
        string filePath = Application.dataPath + "/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }



    // 저장하기
    public void SaveGameData()
    {
        data.curWave += 1;


        List<BlockData> bds = new List<BlockData>();

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count;i++)
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


            bds.Add(bd);
        }

        data.febs = bds;

        //data.febs = FieldEffectPopUpManager.instance.blocks;



        //data.setBlocks = new List<SetBlock>();





        //data.timeLines = FieldEffectPopUpManager.instance.TL;


        //for (int i = 0; i < FieldEffectPopUpManager.instance.TL.Length;i++)
        //{
        //    if (FieldEffectPopUpManager.instance.TL[i].haveBlock) data.slots.Add(0);
        //    else data.slots.Add(1);
        //}


        //for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count;i++)
        //{
        //    SetBlock sb = new SetBlock();
        //    sb.position = FieldEffectPopUpManager.instance.blocks[i].GetComponent<RectTransform>().position;
        //    sb.lineNum = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>().lineNum;
        //    sb.blockName = FieldEffectPopUpManager.instance.blocks[i].name;

        //    sb.upperIdx = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>().m_upperIdx;
        //    sb.DownerIdx = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>().m_downerIdx;
        //    data.setBlocks.Add(sb);
        //}

        // 클래스를 Json 형식으로 전환 (true : 가독성 좋게 작성)
        string ToJsonData = JsonUtility.ToJson(data, true);
        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        string filePath = Application.dataPath + "/" + GameDataFileName;



        // 이미 저장된 파일이 있다면 덮어쓰고, 없다면 새로 만들어서 저장
        File.WriteAllText(filePath, ToJsonData);

        //// 올바르게 저장됐는지 확인 (자유롭게 변형)
        //print("저장 완료");
        //for (int i = 0; i < data.isUnlock.Length; i++)
        //{
        //    print($"{i}번 챕터 잠금 해제 여부 : " + data.isUnlock[i]);
        //}
    }
}