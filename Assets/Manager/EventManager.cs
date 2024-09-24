using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.UI.Button;


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


    //public List<Action> callbacks = new List<Action>();



    //public UnityEvent selectComplete;

    //public ButtonClickEvent buttonClickEvent;

    //public void Start()
    //{
    //    //buttonClickEvent = GetComponent<ButtonClickEvent>();
    //    buttonClickEvent.clikkkkkk += clickEvent;
    //}


    //public event Action TriggerClickEvent;

    //public void clickEvent(Button clickedButton)
    //{
    //    TriggerClickEvent.Invoke();

    //    Debug.Log("Player died event received");
    //}

    //public void UseTeleport()
    //{
    //    Player.instance.UseTeleport();
    //    UIManager.instance.StartCooldown(Player.instance.coolDown);
    //}

    public void UseAttack()
    {
        Player.instance.UseAttack();
    }

    public void playerUnderAttack()
    {
        Player.instance.underAttack();
    }

    public void stageClear()
    {
        //GameManager.instance.playerData.curStage = 1;
        //StageManager.instance.initializeStage();
        //UIManager.instance.stageClear();




        //Player.instance.underAttack();
    }


    //public void selectButton()
    //{
    //    if(Select.instance.isSelect) Select.instance.setGray();
    //    else  Select.instance.setOrigin();


    //    Select.instance.isSelect = !Select.instance.isSelect;
    //}













    //플레이어 스킬
    //public static event Action<float> OnSkillUsed;
    //public static void TriggerSkillUsed(float cooldownDuration)
    //{
    //    OnSkillUsed?.Invoke(cooldownDuration);
    //}




    public static event Action OnCooldownFinished;


    //public static event Action OnStageStart;
    public event Action OnStageStart;

    public void TriggerOnStageStart()
    {
        if (OnStageStart != null) OnStageStart();
    }





    public static void TriggerCooldownFinished()
    {
        OnCooldownFinished?.Invoke();
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


    //public event Action OnUseTeleport;

    //public void UseTeleport()
    //{
    //    if (OnUseTeleport != null) OnUseTeleport();
    //}


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






    //private void OnEnable()
    //{
    //    EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
    //    EventManager.instance.OnCollisionResult += HandleCollisionResult;
    //    EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    //}

    //private void OnDisable()
    //{
    //    EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
    //    EventManager.instance.OnCollisionResult -= HandleCollisionResult;
    //    EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    //}






    // 씬 호출 ------------------------------------------------------------------------------------------------------------------------- //
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

