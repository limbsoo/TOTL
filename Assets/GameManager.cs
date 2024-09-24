using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



// �� �ε� ��ü���� ����
// ���� ����


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
        //DataManager.Instance.LoadGameData();
        //if (DataManager.Instance.data == null) DataManager.Instance.SaveGameData();
    }












    //public PlayerData playerData;

    //private void Start()
    //{
    //    //playerData = new PlayerData();
    //    //if (PlayerPrefs.HasKey("PlayerData")) LoadPlayerData();
    //    //SavePlayerData();
    //}

    //private void LoadPlayerData()
    //{
    //    string jsonData = PlayerPrefs.GetString("PlayerData");
    //    playerData = JsonUtility.FromJson<PlayerData>(jsonData);
    //}

    //private void SavePlayerData()
    //{
    //    string jsonData = JsonUtility.ToJson(playerData);
    //    PlayerPrefs.SetString("PlayerData", jsonData);
    //    PlayerPrefs.Save();
    //}



    // ��Ƽ�� �� �÷��̾� ������ �о����
    public void loadMainScene()
    {
        SceneManager.LoadScene("MainScene");

        //if (StageManager.Instance != null) StageManager.Instance.ResetInstance();

        EventManager.instance.TriggerOnLoadMainScene();
    }

    public void loadPrepartionScene()
    {
        EventManager.instance.TriggerOnLoadPreparationScene();
        SceneManager.LoadScene("prepartionScene");
    }
    public void loadPlayScene()
    {
        //EventManager.instance.TriggerOnLoadPlayScene();
        SceneManager.LoadScene("PlayScene");
    }

    //public void loadLoadingScene()
    //{
    //    SceneManager.LoadScene("loadLoadingScene");
    //}





}