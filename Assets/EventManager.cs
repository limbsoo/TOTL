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

    public static event Action<float> OnSkillUsed; // ��ų ��� �̺�Ʈ
    public static event Action OnCooldownFinished; // ��Ÿ�� ���� �̺�Ʈ

    public static void TriggerSkillUsed(float cooldownDuration)
    {
        OnSkillUsed?.Invoke(cooldownDuration);
    }

    public static void TriggerCooldownFinished()
    {
        OnCooldownFinished?.Invoke();
    }



















    public event Action OnPlayerDied;
    public event Action<int> OnScoreChanged;

    //public event Action OnPlayerEnterTherLight;
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





    //public void playerEnterTherLight()
    //{
    //    if (OnPlayerEnterTherLight != null) OnPlayerEnterTherLight();
    //}

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
    //    //���ӿ���â����
    //}


    //void OnEnable()
    //{
    //    gs.GameOverEvent += gameoverResultHandler; // CollisionHandler�� �̺�Ʈ�� ���� ����
    //}

}

//CustomEventManager Ŭ����

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

//// �̺�Ʈ ����
//CustomEventManager.Instance.TriggerEvent("PlayerDied");

//// �̺�Ʈ ����
//CustomEventManager.Instance.StartListening("PlayerDied", OnPlayerDied);

//void OnPlayerDied(object param)
//{
//    Debug.Log("Player died event received");
//}

