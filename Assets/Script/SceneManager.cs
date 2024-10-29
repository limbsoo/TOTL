using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;



// 씬 로딩 전체적인 게임
// 구독 정리


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

    ////    //현재 스테이지 0이면 continue X
    ////    // Start 누르면 플레이어 선택
    ////    // 선택한 플레이어의 스킬 사용

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



//// 씬 로딩 전체적인 게임
//// 구독 정리


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
//    //    // 오브젝트가 비활성화되었는지 확인
//    //    if (!gameObject.activeInHierarchy)
//    //    {
//    //        gameObject.SetActive(true);  // 다시 활성화
//    //        Debug.Log("GameManager was inactive, now reactivated.");
//    //    }
//    //}


//    // 컨티뉴 시 플레이어 데이터 읽어오기
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