using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

namespace MKey
{
    //public class GameSet : MonoBehaviour
    //{
    //    //public int waveCnt;
    //    //map size

    //    public Action GameOverEvent;


    //    public GameObject endButton;


    //    public static Player playerInstance;
    //    public static LightSet lightInstance;
    //    public static Enemy enemyInstance;
    //    public static Controller controllerInstance;
    //    public static CollisionTest playerCollision;
    //    public static TextController textInstance;


    //    public GameConstructSet gcs;



    //    //public Player player { get; private set; }


    //    void Start()
    //    {

    //        //enemyInstance = GetComponent<Enemy>();
    //        //enemyInstance.createEnemy(gcs.levelConstructSets[gcs.targetIdx].enemyCnt);

    //        Debug.Log("Start");
    //    }

    //    void Update()
    //    {
    //        lightInstance.DetectSpotlightArea(playerInstance.currentObject.transform.position);


    //    }


    //    void OnEnable()
    //    {
    //        CollisionTest.OnCollisionResult += HandleCollisionResult; // CollisionHandler의 이벤트에 대한 구독
    //    }

    //    void OnDisable()
    //    {
    //        CollisionTest.OnCollisionResult -= HandleCollisionResult; // 구독 해제
    //    }

    //    void HandleCollisionResult(GameObject collidedObject)
    //    {
    //        Debug.Log("Received collision with: " + collidedObject.name);

    //        //playerInstance.ChangePrefab();

    //        Destroy(collidedObject.gameObject);
    //        textInstance.cnt++;

    //        if (textInstance.cnt == gcs.levelConstructSets[gcs.targetIdx].enemyCnt)
    //        {
    //            endButton.SetActive(true);
    //            Time.timeScale = 0f;
    //            GameOverEvent?.Invoke();
    //        }

    //    }


    //}

}

