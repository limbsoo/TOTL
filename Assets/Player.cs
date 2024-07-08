using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool changeBotton;

    public GameObject before;
    public GameObject after;

    public GameObject currentObject;

    public GameObject map;

    public int curShape;

    public static CameraController camearInstance;



    //오브젝트 이동
    //public bool isSpawn = false;
    //transform

    private void Awake()
    {
        if (GameSet.playerInstance == null) GameSet.playerInstance = this;
        changeBotton = false;
    }

    void Start()
    {
    }

    public float moveSpeed;
    private Rigidbody rb;
    private Vector3 movement = Vector3.zero;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            changeBotton = !changeBotton;
            ChangePrefab();
        }


        // 키보드 입력 처리
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //movement = new Vector3(moveHorizontal, 0.0f, moveVertical).normalized * moveSpeed;
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;



    }

    void FixedUpdate()
    {
        Move();
        camearInstance.moveCamera(rb.position);

    }

    public void Move()
    {
        if (movement != Vector3.zero)
        {
            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;

            float width = map.transform.localScale.x * 10f; // Plane의 너비 (Unity 기본 Plane의 크기는 10x10 단위)
            float height = map.transform.localScale.z * 10f; // Plane의 높이



            newPos.x = Mathf.Clamp(newPos.x, map.transform.position.x - width / 2, map.transform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, map.transform.position.z - height / 2, map.transform.position.z + height / 2);



            rb.MovePosition(newPos); // 물리적 이동 처리
        }

        else
        {
            rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
        }
    }




    public void createPlayer()
    {
        before = Instantiate(before, transform.position, transform.rotation);
        //before.SetActive(false);


        after = Instantiate(after, transform.position, transform.rotation);
        after.SetActive(false);

        currentObject = before;

        //currentObject = Instantiate(before, transform.position, transform.rotation);
        rb = currentObject.GetComponent<Rigidbody>();

        //currentObject.active = false;

        //rb.interpolation = RigidbodyInterpolation.Interpolate; // 부드러운 이동을 위한 설정
        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // 연속 충돌 감지 모드

        curShape = 0;
    }



    public void ChangePrefab()
    {
        Transform curTransform = currentObject.transform;



        //Transform curTransform = currentObject.transform;
        //changeBotton = !changeBotton;

        //// 현재 오브젝트를 삭제합니다.
        //Destroy(currentObject);

        //// 새로운 프리팹을 인스턴스화 합니다.
        ////if (currentObject.name.Contains("Sphere"))
        //if(!changeBotton)
        //{
        //    currentObject = Instantiate(before, curTransform.position, curTransform.rotation);
        //}
        //else
        //{
        //    currentObject = Instantiate(after, curTransform.position, curTransform.rotation);
        //}
        //rb = currentObject.GetComponent<Rigidbody>();

    }

    Coroutine runningCoroutine = null;


    private IEnumerator transforming()
    {
        yield return new WaitForSecondsRealtime(3);

        Transform curTransform = currentObject.transform;
        curTransform = currentObject.transform;
        before.SetActive(true);
        after.SetActive(false);
        currentObject = before;
        curShape = 0;

        currentObject.transform.position = curTransform.position;
        currentObject.transform.rotation = curTransform.rotation;
        rb = currentObject.GetComponent<Rigidbody>();
    }



    void OnEnable()
    {
        LightSet.inPlayerShotLight += HandleCollisionResult; // CollisionHandler의 이벤트에 대한 구독
    }

    void OnDisable()
    {
        LightSet.inPlayerShotLight -= HandleCollisionResult; // 구독 해제
    }

    void HandleCollisionResult(bool b)
    {
        Transform curTransform = currentObject.transform;

        if (b)
        {
            if (curShape == 0)
            {
                after.SetActive(true);
                before.SetActive(false);
                currentObject = after;
                curShape = 1;

                currentObject.transform.position = curTransform.position;
                currentObject.transform.rotation = curTransform.rotation;
                rb = currentObject.GetComponent<Rigidbody>();
            }
        }

        else
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }

            runningCoroutine = StartCoroutine(transforming());


        }


        //Transform curTransform = currentObject.transform;

        //if (b)
        //{
        //    if(curShape == 0)
        //    {
        //        after.SetActive(true);
        //        before.SetActive(false);
        //        currentObject = after;
        //        curShape = 1;
        //    }

        //    //Debug.Log("1");
        //}

        //else 
        //{
        //    if (curShape == 1)
        //    {
        //        before.SetActive(true);
        //        after.SetActive(false);
        //        currentObject = before;
        //        curShape = 0;
        //    }
        //}

        //currentObject.transform.position = curTransform.position;
        //currentObject.transform.rotation = curTransform.rotation;

        ////currentObject.transform.Translate(curTransform.position);
        //rb = currentObject.GetComponent<Rigidbody>();
    }

}



//public void movePos()
//{
//    //Vector3 vec = new Vector3(0, 0.1f, 0);
//    Vector3 vec = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//    vec *= speed;
//    vec *= Time.deltaTime;
//    //벡터값을 현재위치에 더해줌
//    currentObject.transform.Translate(vec);
//}