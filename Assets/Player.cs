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



    //������Ʈ �̵�
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


        // Ű���� �Է� ó��
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

            float width = map.transform.localScale.x * 10f; // Plane�� �ʺ� (Unity �⺻ Plane�� ũ��� 10x10 ����)
            float height = map.transform.localScale.z * 10f; // Plane�� ����



            newPos.x = Mathf.Clamp(newPos.x, map.transform.position.x - width / 2, map.transform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, map.transform.position.z - height / 2, map.transform.position.z + height / 2);



            rb.MovePosition(newPos); // ������ �̵� ó��
        }

        else
        {
            rb.velocity = Vector3.zero;  // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
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

        //rb.interpolation = RigidbodyInterpolation.Interpolate; // �ε巯�� �̵��� ���� ����
        //rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // ���� �浹 ���� ���

        curShape = 0;
    }



    public void ChangePrefab()
    {
        Transform curTransform = currentObject.transform;



        //Transform curTransform = currentObject.transform;
        //changeBotton = !changeBotton;

        //// ���� ������Ʈ�� �����մϴ�.
        //Destroy(currentObject);

        //// ���ο� �������� �ν��Ͻ�ȭ �մϴ�.
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
        LightSet.inPlayerShotLight += HandleCollisionResult; // CollisionHandler�� �̺�Ʈ�� ���� ����
    }

    void OnDisable()
    {
        LightSet.inPlayerShotLight -= HandleCollisionResult; // ���� ����
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
//    //���Ͱ��� ������ġ�� ������
//    currentObject.transform.Translate(vec);
//}