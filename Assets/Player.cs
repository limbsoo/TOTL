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

        if (Input.GetKeyDown(KeyCode.Space)) // ���Ƿ� �����̽� Ű�� ������ �� ������ �޴� ����
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
        // �÷��̾��� ���� ��ġ�� �ٶ󺸴� ������ ������� �ڷ���Ʈ�� ��ġ ���
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
        // ��� Update()�� �Ϸ�� �� ī�޶� ��ġ ������Ʈ
        if (CameraController.instance.transform != null)
        {
            //new Vector3(Vector3.x, Vector3.y + 50, Vector3.z - 30);
            CameraController.instance.transform.position = new Vector3(rb.position.x, rb.position.y + 50, rb.position.z - 30);

            //CameraController.instance.transform.position = rb.position; // ī�޶� �÷��̾�� ����ȭ
            //CameraController.instance.transform.rotation = rb.rotation; // ī�޶��� ȸ���� ����ȭ (���� ����)
        }
    }


    public void Move()
    {
        Transform transform = StageManager.instance.GCS.levelConstructSets[StageManager.instance.GCS.targetIdx].map.transform;

        if (movement != Vector3.zero)
        {
            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            float width = transform.localScale.x * 10f; // Unity �⺻ Plane�� ũ��� 10x10 ����
            float height = transform.localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, transform.position.x - width / 2, transform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, transform.position.z - height / 2, transform.position.z + height / 2);
            rb.MovePosition(newPos); // ������ �̵� ó��

            // �����̴� �������� ȸ��
            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);
            }
        }

        else rb.velocity = Vector3.zero;  // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����


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
        EventManager.instance.OnCollisionResult += HandleCollisionResult; // CollisionHandler�� �̺�Ʈ�� ���� ����
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
                Debug.Log("��Ÿ��");
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

        Debug.Log("ȣ���ڷ�ƾ");
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

        //        float width = transform.localScale.x * 10f; // Unity �⺻ Plane�� ũ��� 10x10 ����
        //        float height = transform.localScale.z * 10f; 
        //        newPos.x = Mathf.Clamp(newPos.x, transform.position.x - width / 2, transform.position.x + width / 2);
        //        newPos.z = Mathf.Clamp(newPos.z, transform.position.z - height / 2, transform.position.z + height / 2);

        //        rb.MovePosition(newPos); // ������ �̵� ó��
        //    }

        //    else rb.velocity = Vector3.zero;  // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
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


//    //������Ʈ �̵�
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


//        // Ű���� �Է� ó��
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

//            float width = map.transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
//            float height = map.transform.localScale.z * 10f; // Plane�� ����



//            newPos.x = Mathf.Clamp(newPos.x, map.transform.position.x - width / 2, map.transform.position.x + width / 2);
//            newPos.z = Mathf.Clamp(newPos.z, map.transform.position.z - height / 2, map.transform.position.z + height / 2);



//            rb.MovePosition(newPos); // ������ �̵� ó��
//        }

//        else
//        {
//            rb.velocity = Vector3.zero;  // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
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

//        //rb.interpolation = RigidbodyInterpolation.Interpolate; // �ε巯�� �̵��� ���� ����
//        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // ���� �浹 ���� ���

//        curShape = 0;
//    }



//    public void ChangePrefab()
//    {
//        Transform curTransform = currentObject.transform;



//        //Transform curTransform = currentObject.transform;
//        //changeBotton = !changeBotton;

//        //// ���� ������Ʈ�� �����մϴ�.
//        //Destroy(currentObject);

//        //// ���ο� �������� �ν��Ͻ�ȭ �մϴ�.
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
//        LightSet.inPlayerShotLight += HandleCollisionResult; // CollisionHandler�� �̺�Ʈ�� ���� ����
//    }

//    void OnDisable()
//    {
//        LightSet.inPlayerShotLight -= HandleCollisionResult; // ���� ����
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
////    //���Ͱ��� ������ġ�� ������
////    currentObject.transform.Translate(vec);
////}

