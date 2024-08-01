using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EventManager : MonoBehaviour
{
    public static EventManager instance { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }


    //씬 호출
    public event Action OnLoadMainScene;
    public event Action OnLoadPreparationScene;
    public event Action OnLoadPlayScene;

    public void TriggerOnLoadMainScene()
    {
        if (OnLoadMainScene != null) OnLoadMainScene();
    }
    public void TriggerOnLoadPreparationScene()
    {
        if (OnLoadPreparationScene != null) OnLoadPreparationScene();
    }
    public void TriggerOnLoadPlayScene()
    {
        if (OnLoadPlayScene != null) OnLoadPlayScene();
    }


    public event Action OnStageEnd;
    public event Action OnPreparationStageReady;
    public event Action OnMainStageReady;

    public void TriggerOnOnStageEnd()
    {
        if (OnStageEnd != null) OnStageEnd();
    }

    public void TriggerOnPreparationStageReady()
    {
        if (OnPreparationStageReady != null) OnPreparationStageReady();
    }
    public void TriggerOnMainStageReady()
    {
        if (OnMainStageReady != null) OnMainStageReady();
    }



    public event Action<GameObject> OnPrePlayerCollisionPreEnemy;

    public void prePlayerCollisionPreEnemy(GameObject g)
    {
        if (OnPrePlayerCollisionPreEnemy != null) OnPrePlayerCollisionPreEnemy(g);
    }

    public event Action<GameObject> OnEnemyInAttackRange;

    public void EnemyInAttackRange(GameObject g)
    {
        if (OnEnemyInAttackRange != null) OnEnemyInAttackRange(g);
    }









    private void Start()
    {
    }

    //플레이어 스킬
    public static event Action<float> OnSkillUsed;
    public static event Action OnCooldownFinished;


    //public static event Action OnStageStart;
    public event Action OnStageStart;

    public void TriggerOnStageStart()
    {
        if (OnStageStart != null) OnStageStart();
    }


    public static void TriggerSkillUsed(float cooldownDuration)
    {
        OnSkillUsed?.Invoke(cooldownDuration);
    }

    public static void TriggerCooldownFinished()
    {
        OnCooldownFinished?.Invoke();
    }






    public event Action OnGameReStart;

    public void TriggerOnGameReStart()
    {
        OnGameReStart?.Invoke();
    }










    public event Action OnPlayerDied;
    public event Action<int> OnScoreChanged;

    public event Action OnPlayerEnterTherLight;
    public event Action<GameObject> OnCollisionResult;

    public event Action OnGameEnd;


    public event Action OnPlayerEnterTheLightRange;

    public event Action<GameObject> OnPlayerDetectedMonster;

    public event Action OnPlayerTeleportCoolDown;


    public event Action OnUseTeleport;

    public void UseTeleport()
    {
        if (OnUseTeleport != null) OnUseTeleport();
    }


    public void PlayerTeleportCoolDown()
    {
        if (OnPlayerTeleportCoolDown != null) OnPlayerTeleportCoolDown();
    }



    public void playerDetectedMonster(GameObject g)
    {
        if (OnPlayerDetectedMonster != null) OnPlayerDetectedMonster(g);
    }


    public void playerEnterTheLightRange()
    {
        if (OnPlayerEnterTheLightRange != null) OnPlayerEnterTheLightRange();
    }

    public void gameEnd()
    {
        if (OnGameEnd != null) OnGameEnd();
    }

    public void playerCollisionEnemy(GameObject g)
    {
        if (OnCollisionResult != null) OnCollisionResult(g);
    }





    public void playerEnterTherLight()
    {
        if (OnPlayerEnterTherLight != null) OnPlayerEnterTherLight();
    }

    public void PlayerDied()
    {
        if (OnPlayerDied != null) OnPlayerDied();
    }

    public void ScoreChanged(int newScore)
    {
        if (OnScoreChanged != null) OnScoreChanged(newScore);
    }


    ////public GameSet gs { get; private set; }

    //// Start is called before the first frame update
    //void Start()
    //{
    //    gs.GameOverEvent += gameoverResultHandler;
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}


    //void gameoverResultHandler()
    //{
    //    //게임오버창등등등
    //}


    //void OnEnable()
    //{
    //    gs.GameOverEvent += gameoverResultHandler; // CollisionHandler의 이벤트에 대한 구독
    //}

}

//CustomEventManager 클래스

//using System;
//using System.Collections.Generic;
//using UnityEngine;

//public class CustomEventManager : MonoBehaviour
//{
//    private Dictionary<string, Action<object>> eventDictionary;

//    public static CustomEventManager Instance;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//            eventDictionary = new Dictionary<string, Action<object>>();
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    public void StartListening(string eventName, Action<object> listener)
//    {
//        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
//        {
//            thisEvent += listener;
//            eventDictionary[eventName] = thisEvent;
//        }
//        else
//        {
//            thisEvent += listener;
//            eventDictionary.Add(eventName, thisEvent);
//        }
//    }

//    public void StopListening(string eventName, Action<object> listener)
//    {
//        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
//        {
//            thisEvent -= listener;
//            eventDictionary[eventName] = thisEvent;
//        }
//    }

//    public void TriggerEvent(string eventName, object eventParam = null)
//    {
//        if (eventDictionary.TryGetValue(eventName, out Action<object> thisEvent))
//        {
//            thisEvent.Invoke(eventParam);
//        }
//    }
//}

//// 이벤트 발행
//CustomEventManager.Instance.TriggerEvent("PlayerDied");

//// 이벤트 구독
//CustomEventManager.Instance.StartListening("PlayerDied", OnPlayerDied);

//void OnPlayerDied(object param)
//{
//    Debug.Log("Player died event received");
//}

