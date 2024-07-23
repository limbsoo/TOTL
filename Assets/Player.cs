using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    //private GameObject obj;
    public int health { get; private set; }
    public string name { get; private set; }

    private Vector3 movement;
    public float moveSpeed;
    private Rigidbody rb;

    Coroutine attackCoroutine = null;

    private PlayerState pState;


    //prePlayer꺼

    public static Player instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            rb = GetComponent<Rigidbody>();
            pState = PlayerState.Original;


            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }




    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    pState = PlayerState.Original;
    //}


    void Update()
    {
        if (StageManager.instance.Sstate == StageState.Preparation) return;

        HandleInput();
        CameraController.Vector3 = transform.position;
    }

    void FixedUpdate()
    {
        if (StageManager.instance.Sstate == StageState.Preparation) return;

        //rb = StageManager.players[StageManager.currentPlayerIdx].GetComponent<Rigidbody>();
        Move();
    }

    public void destoryEnemy(GameObject g)
    {
        Destroy(g.gameObject);
    }

    // 이벤트
    private void OnEnable()
    {
        //EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        EventManager.instance.OnCollisionResult += HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    }

    private void OnDisable()
    {
        //EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        EventManager.instance.OnCollisionResult -= HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    }

    //if(pState == PlayerState.Transformed)

    private void HandleInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (pState == PlayerState.Original)
            {
                if(CanUseSkills())
                {
                    UseSkill();
                    Teleport(teleportLength);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (pState == PlayerState.Original)
            {
                transform.GetChild(0).gameObject.SetActive(true);
                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(attack());
            }
        }
    }


    void Teleport(float length)
    {
        Vector3 teleportPosition = rb.position + rb.transform.forward * length;
        rb.MovePosition(teleportPosition);
    }

    private IEnumerator attack()
    {
        yield return new WaitForSecondsRealtime(1);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void Move()
    {
        if (movement != Vector3.zero)
        {
            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            float width = StageManager.mapTransform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
            float height = StageManager.mapTransform.localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, StageManager.mapTransform.position.x - width / 2, StageManager.mapTransform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, StageManager.mapTransform.position.z - height / 2, StageManager.mapTransform.position.z + height / 2);
            rb.MovePosition(newPos); // 물리적 이동 처리

            // 움직이는 방향으로 회전
            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);
            }
        }

        else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
    }

    void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        Debug.Log("Player Died.");
        EventManager.instance.PlayerDied();
    }




    Coroutine runningCoroutine = null;

    void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        ////if (StageManager.currentPlayerIdx == 1) return;

        ////playerInstance.ChangePrefab();

        //Destroy(collidedObject.gameObject);
        ////textInstance.cnt++;

        //StageManager.instance.currentScore++;

        //EventManager.instance.ScoreChanged(StageManager.instance.currentScore);

        ////if (textInstance.cnt == gcs.levelConstructSets[gcs.targetIdx].enemyCnt)
        ////{
        ////    endButton.SetActive(true);
        ////    Time.timeScale = 0f;
        ////    GameOverEvent?.Invoke();
        ////}

    }

    public float teleportLength = 10;

    public float skillCooldown; // 스킬 쿨타임
    private bool canUseSkill = true; // 스킬 사용 가능 여부

    public bool CanUseSkills()
    {
        return canUseSkill;
    }

    private IEnumerator ResetSkillAvailability(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseSkill = true;
    }

    public void UseSkill()
    {
        Debug.Log("Skill used!");

        canUseSkill = false;
        EventManager.TriggerSkillUsed(skillCooldown);
        StartCoroutine(ResetSkillAvailability(skillCooldown));
    }
















    //void changeFigure()
    //{
    //    if(StageManager.currentPlayerIdx == 0)
    //    {
    //        transform.GetChild(0).gameObject.SetActive(false);
    //        suffleFigure(0, 1);

    //    }

    //    else
    //    {
    //        if (runningCoroutine != null)
    //        {
    //            StopCoroutine(runningCoroutine);
    //        }

    //        runningCoroutine = StartCoroutine(transforming());
    //    }
    //}

    //void suffleFigure(int a, int b)
    //{
    //    StageManager.players[b].SetActive(true);
    //    StageManager.players[b].transform.position = StageManager.players[a].transform.position;
    //    StageManager.players[b].transform.rotation = StageManager.players[a].transform.rotation;

    //    //rb = StageManager.players[a].GetComponent<Rigidbody>();

    //    StageManager.players[a].SetActive(false);
    //    StageManager.currentPlayerIdx = b;

    //    //CameraController.instance.transform.position = new Vector3(StageManager.players[b].transform.position.x, StageManager.players[b].transform.position.y + 50, StageManager.players[b].transform.position.z - 30);

    //}


    //private IEnumerator transforming()
    //{
    //    yield return new WaitForSecondsRealtime(3);

    //    Debug.Log("호출코루틴");
    //    suffleFigure(1, 0);

    //}

}

