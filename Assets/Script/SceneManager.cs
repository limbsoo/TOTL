using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



// �� �ε� ��ü���� ����
// ���� ����


public class SceneManager : MonoBehaviour
{
    public static SceneManager instance { get; private set; }

    private void Awake()
    {
        if (this.name == "SceneManager")
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone) yield return null;
    }


    //public void loadMainScene()
    //{
    //    StartCoroutine(LoadMainSceneAsync());
    //}

    //private IEnumerator LoadMainSceneAsync()
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
    //    while (!asyncLoad.isDone) yield return null;
    //}

    //public void loadPlayScene()
    //{
    //    StartCoroutine(LoadPlaySceneAsync());
    //}

    //private IEnumerator LoadPlaySceneAsync()
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayScene");
    //    while (!asyncLoad.isDone) yield return null;
    //}


    //public static SceneManager instance { get; private set; }

    //private void Awake()
    //{
    //    if (this.name == "SceneManager")
    //    {
    //        if (instance == null)
    //        {
    //            instance = this;
    //            DontDestroyOnLoad(gameObject);
    //        }
    //    }
    //}

    ////private void Start()
    ////{
    ////    DataManager.Instance.LoadGameData();

    ////    //���� �������� 0�̸� continue X
    ////    // Start ������ �÷��̾� ����
    ////    // ������ �÷��̾��� ��ų ���

    ////}

    //public void LoadScene(string sceneName)
    //{
    //    StartCoroutine(LoadSceneAsync(sceneName));
    //}

    //private IEnumerator LoadSceneAsync(string sceneName)
    //{
    //    AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
    //    while (!asyncLoad.isDone) yield return null;
    //}


    ////public void loadMainScene()
    ////{
    ////    StartCoroutine(LoadMainSceneAsync());
    ////}

    ////private IEnumerator LoadMainSceneAsync()
    ////{
    ////    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
    ////    while (!asyncLoad.isDone) yield return null;
    ////}

    ////public void loadPlayScene()
    ////{
    ////    StartCoroutine(LoadPlaySceneAsync());
    ////}

    ////private IEnumerator LoadPlaySceneAsync()
    ////{
    ////    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayScene");
    ////    while (!asyncLoad.isDone) yield return null;
    ////}

}































//using System;
//using System.Collections;
//using System.Collections.Generic;
//using TMPro;
////using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.SceneManagement;



//// �� �ε� ��ü���� ����
//// ���� ����


//public class GameManager : MonoBehaviour
//{
//    public static GameManager instance { get; private set; }

//    private void Awake()
//    {
//        if(this.name == "GameManager")
//        {
//            if (instance == null)
//            {
//                instance = this;
//                DontDestroyOnLoad(gameObject);
//            }
//            //else Destroy(gameObject);
//        }


//    }


//    private void Start()
//    {
//        //DataManager.Instance.LoadGameData();
//        //if (DataManager.Instance.data == null) DataManager.Instance.SaveGameData();
//    }












//    //public PlayerData playerData;

//    //private void Start()
//    //{
//    //    //playerData = new PlayerData();
//    //    //if (PlayerPrefs.HasKey("PlayerData")) LoadPlayerData();
//    //    //SavePlayerData();
//    //}

//    //private void LoadPlayerData()
//    //{
//    //    string jsonData = PlayerPrefs.GetString("PlayerData");
//    //    playerData = JsonUtility.FromJson<PlayerData>(jsonData);
//    //}

//    //private void SavePlayerData()
//    //{
//    //    string jsonData = JsonUtility.ToJson(playerData);
//    //    PlayerPrefs.SetString("PlayerData", jsonData);
//    //    PlayerPrefs.Save();
//    //}


//    //void OnEnable()
//    //{
//    //    // ������Ʈ�� ��Ȱ��ȭ�Ǿ����� Ȯ��
//    //    if (!gameObject.activeInHierarchy)
//    //    {
//    //        gameObject.SetActive(true);  // �ٽ� Ȱ��ȭ
//    //        Debug.Log("GameManager was inactive, now reactivated.");
//    //    }
//    //}


//    // ��Ƽ�� �� �÷��̾� ������ �о����
//    public void loadMainScene()
//    {
//        StartCoroutine(LoadMainSceneAsync());
//    }

//    private IEnumerator LoadMainSceneAsync()
//    {
//        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("MainScene");
//        while (!asyncLoad.isDone) yield return null;
//    }


//    //public void loadPrepartionScene()
//    //{
//    //    EventManager.instance.TriggerOnLoadPreparationScene();
//    //    SceneManager.LoadScene("prepartionScene");
//    //}

//    public void loadPlayScene()
//    {
//        StartCoroutine(LoadPlaySceneAsync());
//    }

//    private IEnumerator LoadPlaySceneAsync()
//    {
//        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PlayScene");
//        while (!asyncLoad.isDone) yield return null;
//        //StageManager.instance.InitStage();
//    }

//    //public void loadPlayScene()
//    //{
//    //    //EventManager.instance.TriggerOnLoadPlayScene();
//    //    SceneManager.LoadScene("PlayScene");
//    //    EventManager.instance.TriggerOnLoadPlayScene();
//    //}

//    //public void loadLoadingScene()
//    //{
//    //    SceneManager.LoadScene("loadLoadingScene");
//    //}





//}