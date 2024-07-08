using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GameSet : MonoBehaviour
{
    //void Awake()

    //public int waveCnt;
    //map size


    public GameObject endButton;


    public static Player playerInstance;
    public static LightSet lightInstance;
    public static Enemy enemyInstance;
    public static Controller controllerInstance;
    public static CollisionTest playerCollision;
    public static TextController textInstance;


    public GameConstructSet gameConstructSet;

    // 목표 설정 난이도 설정 

    // Start is called before the first frame update
    void Start()
    {
        endButton.SetActive(false);

        //목표와 난이도를 읽어오기

        playerInstance.createPlayer();
        lightInstance.createSpotlight();
        enemyInstance.createEnemy(gameConstructSet.levelConstructSets[gameConstructSet.targetIdx].enemyCnt);

        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

        //목표 완료 시 스테이지 클리어  

        //playerInstance.transform.Translate(controllerInstance.movePosition(playerInstance.transform));

        //if(!playerCollision.isContact)
        //{
        //    playerInstance.movePos();
        //}

        lightInstance.DetectSpotlightArea(playerInstance.currentObject.transform.position);
        //camearInstance.moveCamera(playerInstance.currentObject.transform.position);


    }


    void OnEnable()
    {
        CollisionTest.OnCollisionResult += HandleCollisionResult; // CollisionHandler의 이벤트에 대한 구독
    }

    void OnDisable()
    {
        CollisionTest.OnCollisionResult -= HandleCollisionResult; // 구독 해제
    }

    void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        //playerInstance.ChangePrefab();

        Destroy(collidedObject.gameObject);
        textInstance.cnt++;

        if(textInstance.cnt == gameConstructSet.levelConstructSets[gameConstructSet.targetIdx].enemyCnt)
        {
            endButton.SetActive(true);
            Time.timeScale = 0f;
        }

    }
}
