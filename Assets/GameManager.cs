using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }

    private void Start()
    {
        //StageManager.instance.LoadStage();
    }

    public void loadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");
        //EventManager.instance.TriggerOnGameReStart();
        //StageManager.instance.LoadStage();

        //EventManager.instance.TriggerOnStageStart();

    }

    public void loadMainScene()
    {
        SceneManager.LoadScene("MainScene");

    }

    public void loadPrepartionScene()
    {
        SceneManager.LoadScene("prepartionScene");
    }

    public void loadLoadingScene()
    {
        SceneManager.LoadScene("loadLoadingScene");
    }





}