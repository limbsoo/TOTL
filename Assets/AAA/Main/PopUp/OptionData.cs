using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;

public class OptionData : MonoBehaviour
{
    string filePath;

    private void Awake()
    {
        filePath = Application.persistentDataPath + "OptionData.json";
    }

    public Options opionData = new Options();


    void OnEnable()
    {
        if (File.Exists(filePath))
        {
            string FromJsonData = File.ReadAllText(filePath);
            opionData = JsonUtility.FromJson<Options>(FromJsonData);

            Slider[] sliders = transform.GetComponentsInChildren<Slider>();

            foreach (Slider sl in sliders)
            {
                switch (sl.name)
                {
                    case "Master":
                        sl.value = opionData.Master;
                        break;

                    case "BGM":
                        sl.value = opionData.BGM;
                        break;

                    case "Effect":
                        sl.value = opionData.Effect;
                        break;
                }

            }



            print("옵션 불러오기 완료");
        }

        else InitailizeData();
    }


    public void InitailizeData()
    {
        opionData.InitData();
        SaveFile();
    }

    public void SaveFile()
    {
        string ToJsonData = JsonUtility.ToJson(opionData, true);

        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
            File.WriteAllText(filePath, "myJsonText");
        }
        File.WriteAllText(filePath, ToJsonData);
    }

    private void OnDestroy()
    {
        Slider[] sliders = transform.GetComponentsInChildren<Slider>();

        foreach (Slider sl in sliders) 
        {
            switch(sl.name)
            {
                case "Master":
                    opionData.Master = sl.value;
                    break;

                case "BGM":
                    opionData.BGM = sl.value;
                    break;

                case "Effect":
                    opionData.Effect = sl.value;
                    break;
            }

        }

        SaveFile();

        //FieldEffect fe = gameObject.transform.parent.GetComponent<FieldEffect>();
    }

}

public class Options
{
    public float Master;
    public float BGM;
    public float Effect;

    public void InitData()
    {
        Master = 0;
        BGM = 0;
        Effect = 0;
    }



}



