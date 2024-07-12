using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Player : MonoBehaviour
{
    public int health { get; private set; }
    public string name { get; private set; }

    private Vector3 movement;
    public float moveSpeed;
    private Rigidbody rb;

    Coroutine attackCoroutine = null;

    public void Initialize(int health, string name)
    {
        rb = GetComponent<Rigidbody>();
        this.health = health;
        this.name = name;
    }
















    
    
    private IEnumerator attack()
    {
        yield return new WaitForSecondsRealtime(1);
        StageManager.players[0].transform.GetChild(0).gameObject.SetActive(false);
    }

    private bool teleported = false;

    void Update()
    {
        //teleported = false;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            EventManager.instance.PlayerTeleportCoolDown();
        }


        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;

        if (Input.GetKeyDown(KeyCode.Space)) // 임의로 스페이스 키를 눌렀을 때 데미지 받는 예시
        {
            if (StageManager.currentPlayerIdx == 0)
            {
                StageManager.players[0].transform.GetChild(0).gameObject.SetActive(true);
                //attack();
                if (attackCoroutine != null)
                {
                    StopCoroutine(attackCoroutine);
                }

                attackCoroutine = StartCoroutine(attack());

            }
            //TakeDamage(10);
        }



    }

    void TeleportForward()
    {
        // 플레이어의 현재 위치와 바라보는 방향을 기반으로 텔레포트할 위치 계산
        Vector3 teleportPosition = rb.position + rb.transform.forward * 3f;
        rb.MovePosition(teleportPosition);
        teleported = false;

        CameraController.Vector3 = rb.transform.position;
    }


    void FixedUpdate()
    {
        rb = StageManager.players[StageManager.currentPlayerIdx].GetComponent<Rigidbody>();
        //CameraController.Vector3 = rb.transform.position;

        if (teleported) TeleportForward();
        Move();
    }

    void LateUpdate()
    {
        // 모든 Update()가 완료된 후 카메라 위치 업데이트
        if (CameraController.instance.transform != null)
        {
            //new Vector3(Vector3.x, Vector3.y + 50, Vector3.z - 30);
            CameraController.instance.transform.position = new Vector3(rb.position.x, rb.position.y + 50, rb.position.z - 30);

            //CameraController.instance.transform.position = rb.position; // 카메라를 플레이어와 동기화
            //CameraController.instance.transform.rotation = rb.rotation; // 카메라의 회전도 동기화 (선택 사항)
        }
    }


    public void Move()
    {
        Transform transform = StageManager.instance.GCS.levelConstructSets[StageManager.instance.GCS.targetIdx].map.transform;

        if (movement != Vector3.zero)
        {
            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            float width = transform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
            float height = transform.localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, transform.position.x - width / 2, transform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, transform.position.z - height / 2, transform.position.z + height / 2);
            rb.MovePosition(newPos); // 물리적 이동 처리

            // 움직이는 방향으로 회전
            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);
            }
        }

        else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤


        CameraController.Vector3 = rb.transform.position;
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Player Died.");
        EventManager.instance.PlayerDied();
    }

    private void OnEnable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        EventManager.instance.OnCollisionResult += HandleCollisionResult; // CollisionHandler의 이벤트에 대한 구독
        EventManager.instance.OnPlayerTeleportCoolDown += CoolDownTeleport;
    }

    private void OnDisable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        EventManager.instance.OnCollisionResult -= HandleCollisionResult;
        EventManager.instance.OnPlayerTeleportCoolDown -= CoolDownTeleport;
    }

    private IEnumerator coolDowning()
    {
        yield return new WaitForSecondsRealtime(2);
        teleportCorutine = null;
        //teleported = false;
    }

    Coroutine teleportCorutine = null;

    void CoolDownTeleport()
    {
        //if (runningCoroutine != null)
        //{
        //    StopCoroutine(runningCoroutine);
        //}

        //runningCoroutine = StartCoroutine(transforming());



        if (!teleported)
        {
            if(teleportCorutine == null)
            {
                teleported = true;
                //EventManager.instance.UseTeleport();

                UIManager.instance.teleportCoolDown();

                teleportCorutine = StartCoroutine(coolDowning());
            }

            else
            {
                Debug.Log("쿨타임");
            }

            //teleported = true;
            //coolDowning();

        }


    }


    Coroutine runningCoroutine = null;

    void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        if (StageManager.currentPlayerIdx == 1) return;

        //playerInstance.ChangePrefab();

        Destroy(collidedObject.gameObject);
        //textInstance.cnt++;

        StageManager.instance.currentScore++;

        EventManager.instance.ScoreChanged(StageManager.instance.currentScore);

        //if (textInstance.cnt == gcs.levelConstructSets[gcs.targetIdx].enemyCnt)
        //{
        //    endButton.SetActive(true);
        //    Time.timeScale = 0f;
        //    GameOverEvent?.Invoke();
        //}

    }


    void changeFigure()
    {
        if(StageManager.currentPlayerIdx == 0)
        {
            StageManager.players[0].transform.GetChild(0).gameObject.SetActive(false);
            suffleFigure(0, 1);
        }

        else
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }

            runningCoroutine = StartCoroutine(transforming());
        }
    }

    void suffleFigure(int a, int b)
    {
        StageManager.players[b].SetActive(true);
        StageManager.players[b].transform.position = StageManager.players[a].transform.position;
        StageManager.players[b].transform.rotation = StageManager.players[a].transform.rotation;
        StageManager.players[a].SetActive(false);
        StageManager.currentPlayerIdx = b;
    }
    

    private IEnumerator transforming()
    {
        yield return new WaitForSecondsRealtime(3);

        Debug.Log("호출코루틴");
        suffleFigure(1, 0);

    }






        //public GameObject player;
        //public float moveSpeed;
        //public Rigidbody rb;
        //private Vector3 movement;

        //void Start()
        //{
        //    movement = Vector3.zero;
        //    createPlayer();
        //}

        //public void createPlayer()
        //{
        //    player = Instantiate(player, transform.position, transform.rotation);
        //    rb = player.GetComponent<Rigidbody>();
        //}

        //void Update()
        //{
        //    float moveHorizontal = Input.GetAxis("Horizontal");
        //    float moveVertical = Input.GetAxis("Vertical");
        //    movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;
        //}

        //void FixedUpdate()
        //{
        //    Move();
        //    //CameraController.moveCamera();

        //    CameraController.Vector3 = rb.transform.position;
        //}

        //public void Move()
        //{
        //    if (movement != Vector3.zero)
        //    {
        //        Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;

        //        Transform transform = StageManager.transform;

        //        float width = transform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
        //        float height = transform.localScale.z * 10f; 
        //        newPos.x = Mathf.Clamp(newPos.x, transform.position.x - width / 2, transform.position.x + width / 2);
        //        newPos.z = Mathf.Clamp(newPos.z, transform.position.z - height / 2, transform.position.z + height / 2);

        //        rb.MovePosition(newPos); // 물리적 이동 처리
        //    }

        //    else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
        //}


    }


//public class Player : MonoBehaviour
//{
//    public bool changeBotton;

//    public GameObject before;
//    public GameObject after;

//    public GameObject currentObject;

//    public GameObject map;

//    public int curShape;

//    public static CameraController camearInstance;


//    //private void Awake()
//    //{
//    //    playerInstance = this;
//    //    changeBotton = false;
//    //}


//    //오브젝트 이동
//    //public bool isSpawn = false;
//    //transform

//    //private void Awake()
//    //{
//    //    if (GameSet.playerInstance == null) GameSet.playerInstance = this;
//    //    changeBotton = false;
//    //}

//    void Start()
//    {
//        createPlayer();
//    }

//    public float moveSpeed;
//    private Rigidbody rb;
//    private Vector3 movement = Vector3.zero;

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Space))
//        {
//            changeBotton = !changeBotton;
//            ChangePrefab();
//        }


//        // 키보드 입력 처리
//        float moveHorizontal = Input.GetAxis("Horizontal");
//        float moveVertical = Input.GetAxis("Vertical");

//        //movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized * moveSpeed;
//        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;



//    }

//    void FixedUpdate()
//    {
//        Move();
//        camearInstance.moveCamera(rb.position);

//    }

//    public void Move()
//    {
//        if (movement != Vector3.zero)
//        {
//            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;

//            float width = map.transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
//            float height = map.transform.localScale.z * 10f; // Plane의 높이



//            newPos.x = Mathf.Clamp(newPos.x, map.transform.position.x - width / 2, map.transform.position.x + width / 2);
//            newPos.z = Mathf.Clamp(newPos.z, map.transform.position.z - height / 2, map.transform.position.z + height / 2);



//            rb.MovePosition(newPos); // 물리적 이동 처리
//        }

//        else
//        {
//            rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
//        }
//    }

//    public void createPlayer()
//    {
//        before = Instantiate(before, transform.position, transform.rotation);
//        //before.SetActive(false);


//        after = Instantiate(after, transform.position, transform.rotation);
//        after.SetActive(false);

//        currentObject = before;

//        //currentObject = Instantiate(before, transform.position, transform.rotation);
//        rb = currentObject.GetComponent<Rigidbody>();

//        //currentObject.active = false;

//        //rb.interpolation = RigidbodyInterpolation.Interpolate; // 부드러운 이동을 위한 설정
//        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 연속 충돌 감지 모드

//        curShape = 0;
//    }



//    public void ChangePrefab()
//    {
//        Transform curTransform = currentObject.transform;



//        //Transform curTransform = currentObject.transform;
//        //changeBotton = !changeBotton;

//        //// 현재 오브젝트를 삭제합니다.
//        //Destroy(currentObject);

//        //// 새로운 프리팹을 인스턴스화 합니다.
//        ////if (currentObject.name.Contains("Sphere"))
//        //if(!changeBotton)
//        //{
//        //    currentObject = Instantiate(before, curTransform.position, curTransform.rotation);
//        //}
//        //else
//        //{
//        //    currentObject = Instantiate(after, curTransform.position, curTransform.rotation);
//        //}
//        //rb = currentObject.GetComponent<Rigidbody>();

//    }

//    Coroutine runningCoroutine = null;


//    private IEnumerator transforming()
//    {
//        yield return new WaitForSecondsRealtime(3);

//        Transform curTransform = currentObject.transform;
//        curTransform = currentObject.transform;
//        before.SetActive(true);
//        after.SetActive(false);
//        currentObject = before;
//        curShape = 0;

//        currentObject.transform.position = curTransform.position;
//        currentObject.transform.rotation = curTransform.rotation;
//        rb = currentObject.GetComponent<Rigidbody>();
//    }



//    void OnEnable()
//    {
//        LightSet.inPlayerShotLight += HandleCollisionResult; // CollisionHandler의 이벤트에 대한 구독
//    }

//    void OnDisable()
//    {
//        LightSet.inPlayerShotLight -= HandleCollisionResult; // 구독 해제
//    }

//    void HandleCollisionResult(bool b)
//    {
//        Transform curTransform = currentObject.transform;

//        if (b)
//        {
//            if (curShape == 0)
//            {
//                after.SetActive(true);
//                before.SetActive(false);
//                currentObject = after;
//                curShape = 1;

//                currentObject.transform.position = curTransform.position;
//                currentObject.transform.rotation = curTransform.rotation;
//                rb = currentObject.GetComponent<Rigidbody>();
//            }
//        }

//        else
//        {
//            if (runningCoroutine != null)
//            {
//                StopCoroutine(runningCoroutine);
//            }

//            runningCoroutine = StartCoroutine(transforming());


//        }


//        //Transform curTransform = currentObject.transform;

//        //if (b)
//        //{
//        //    if(curShape == 0)
//        //    {
//        //        after.SetActive(true);
//        //        before.SetActive(false);
//        //        currentObject = after;
//        //        curShape = 1;
//        //    }

//        //    //Debug.Log("1");
//        //}

//        //else 
//        //{
//        //    if (curShape == 1)
//        //    {
//        //        before.SetActive(true);
//        //        after.SetActive(false);
//        //        currentObject = before;
//        //        curShape = 0;
//        //    }
//        //}

//        //currentObject.transform.position = curTransform.position;
//        //currentObject.transform.rotation = curTransform.rotation;

//        ////currentObject.transform.Translate(curTransform.position);
//        //rb = currentObject.GetComponent<Rigidbody>();
//    }

//}



////public void movePos()
////{
////    //Vector3 vec = new Vector3(0, 0.1f, 0);
////    Vector3 vec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
////    vec *= speed;
////    vec *= Time.deltaTime;
////    //벡터값을 현재위치에 더해줌
////    currentObject.transform.Translate(vec);
////}

