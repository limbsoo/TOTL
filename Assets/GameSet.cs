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

    // ��ǥ ���� ���̵� ���� 

    // Start is called before the first frame update
    void Start()
    {
        endButton.SetActive(false);

        //��ǥ�� ���̵��� �о����

        playerInstance.createPlayer();
        lightInstance.createSpotlight();
        enemyInstance.createEnemy(gameConstructSet.levelConstructSets[gameConstructSet.targetIdx].enemyCnt);

        Debug.Log("Start");
    }

    // Update is called once per frame
    void Update()
    {

        //��ǥ �Ϸ� �� �������� Ŭ����  

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
        CollisionTest.OnCollisionResult += HandleCollisionResult; // CollisionHandler�� �̺�Ʈ�� ���� ����
    }

    void OnDisable()
    {
        CollisionTest.OnCollisionResult -= HandleCollisionResult; // ���� ����
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
