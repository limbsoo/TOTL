using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScene : MonoBehaviour
{
    public static MainScene instance { get; private set; }

    //public ButtonController controller;


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
        DataManager.Instance.InitailizeData(playerIdx);
        //나중에 게임모드도

        SceneManager.instance.LoadScene("PlayScene");
    }

    public void LoadGame()
    {
        SceneManager.instance.LoadScene("PlayScene");
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
