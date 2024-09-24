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

        data.setBlocks = new List<SetBlock>();
        //string jsonString = JsonConvert.SerializeObject(human);

        string ToJsonData = JsonUtility.ToJson(data, true);
        string filePath = Application.dataPath + "/" + GameDataFileName;
        File.WriteAllText(filePath, ToJsonData);
    }



    // �����ϱ�
    public void SaveGameData()
    {
        data.curWave += 1;

        data.setBlocks = new List<SetBlock>();

        for (int i = 0; i < FieldEffectPopUpManager.instance.blocks.Count;i++)
        {
            SetBlock sb = new SetBlock();
            sb.position = FieldEffectPopUpManager.instance.blocks[i].GetComponent<RectTransform>().position;
            sb.lineNum = FieldEffectPopUpManager.instance.blocks[i].GetComponent<FieldEffectBlock>().lineNum;
            sb.blockName = FieldEffectPopUpManager.instance.blocks[i].name;

            data.setBlocks.Add(sb);
        }

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