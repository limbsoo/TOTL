using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



// �� �ε� ��ü���� ����


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

    // ��Ƽ�� �� �÷��̾� ������ �о����
    public void loadMainScene()
    {
        SceneManager.LoadScene("MainScene");

        //if (StageManager.Instance != null) StageManager.Instance.ResetInstance();

        EventManager.instance.TriggerOnLoadMainScene();
    }

    public void loadPrepartionScene()
    {
        SceneManager.LoadScene("prepartionScene");

        

        EventManager.instance.TriggerOnLoadPreparationScene();
    }
    public void loadPlayScene()
    {
        SceneManager.LoadScene("PlayScene");


        EventManager.instance.TriggerOnLoadPlayScene();
    }

    //public void loadLoadingScene()
    //{
    //    SceneManager.LoadScene("loadLoadingScene");
    //}





}