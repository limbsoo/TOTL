using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public static MainScene instance { get; private set; }

    //public ButtonController controller;

    public TMP_Text test;


    private void Awake()
    {
        if (this.name == "MainScene")
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public int playerIdx;

    public void StartGame()
    {
        test.text = "ĳ���� ����";

        DataManager.Instance.InitSelectCharacter(playerIdx);
        //���߿� ���Ӹ�嵵

        test.text = "�� �ε� ����";

        SceneManager.instance.LoadScene("PlayScene");
    }

    public void LoadGame()
    {
        SceneManager.instance.LoadScene("PlayScene");
    }

    public void ResetData()
    {
        DataManager.Instance.ResetStage();
        //gameObject.transform.parent.gameObject.SetActive(false);
    }


    private void OnEnable()
    {
        ButtonController.OnLoadScene += LoadGame;
    }

    private void OnDisable()
    {
        ButtonController.OnLoadScene -= LoadGame;

        //EventManager.instance.OnStageEnd -= EndStage;
    }


    //private void Start()
    //{

    //}




    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
