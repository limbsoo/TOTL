using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    static GameObject container;

    // ---�̱������� ����--- //
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

    // --- ���� ������ �����̸� ���� ("���ϴ� �̸�(����).json") --- //
    string GameDataFileName = "GameData.json";

    // --- ����� Ŭ���� ���� --- //
    public GameData data = new GameData();


    // �ҷ�����
    public void LoadGameData()
    {
        string filePath = Application.dataPath + "/" + GameDataFileName;
        
        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;

        // ����� ������ �ִٸ�
        if (File.Exists(filePath))
        {
            // ����� ���� �о���� Json�� Ŭ���� �������� ��ȯ�ؼ� �Ҵ�
            string FromJsonData = File.ReadAllText(filePath);
            data = JsonUtility.FromJson<GameData>(FromJsonData);
            print("�ҷ����� �Ϸ�");
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



    // �����ϱ�
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

        // Ŭ������ Json �������� ��ȯ (true : ������ ���� �ۼ�)
        string ToJsonData = JsonUtility.ToJson(data, true);
        //string filePath = Application.persistentDataPath + "/" + GameDataFileName;
        string filePath = Application.dataPath + "/" + GameDataFileName;



        // �̹� ����� ������ �ִٸ� �����, ���ٸ� ���� ���� ����
        File.WriteAllText(filePath, ToJsonData);

        //// �ùٸ��� ����ƴ��� Ȯ�� (�����Ӱ� ����)
        //print("���� �Ϸ�");
        //for (int i = 0; i < data.isUnlock.Length; i++)
        //{
        //    print($"{i}�� é�� ��� ���� ���� : " + data.isUnlock[i]);
        //}
    }
}