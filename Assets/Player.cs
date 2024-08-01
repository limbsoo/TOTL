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
    public float health { get; private set; }
    public float moveSpeed { get; private set; }

    public float damamge { get; private set; }
    public float coolDown { get; private set; }


    public float teleportLength = 10;
    private bool canUseSkill = true; // 스킬 사용 가능 여부


    //public string name { get; private set; }

    private Vector3 movement;
    
    public static Rigidbody rb;

    public static bool weakning = false;
    

    Coroutine attackCoroutine = null;

    private PlayerState pState;

    private bool playerDamaged = false;


    public float PenealtyTime { get; private set; }

   

    //prePlayer꺼

    public static Player instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            PlayerData playerData = GameManager.instance.playerData;
            health = playerData.health;
            moveSpeed = playerData.moveSpeed;
            damamge = playerData.damamge;
            coolDown = playerData.coolDown;
            PenealtyTime = 0;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            pState = PlayerState.Original;
            

            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }


    private Animator animator;

    //private void Awake()
    //{
    //    rb = GetComponent<Rigidbody>();
    //    pState = PlayerState.Original;
    //}


    void Update()
    {
        

        //if (StageManager.instance.Sstate == StageState.Preparation) return;

        HandleInput();
        CameraController.Vector3 = transform.position;
    }

    public static float inputThreshold = 0.1f; // 임계값 설정

    void FixedUpdate()
    {
        //if (StageManager.instance.Sstate == StageState.Preparation) return;

        //rb = GetComponent<Rigidbody>();



        Move();

        Debug.Log($"Movement: {movement}, LastRotation: {lastRotation.eulerAngles}");

        //Move();


        //rb = StageManager.players[StageManager.currentPlayerIdx].GetComponent<Rigidbody>();

    }

    public void destoryEnemy(GameObject g)
    {
        Enemy e = g.gameObject.GetComponent<Enemy>();

        if (e.onceDamaged) return;

        if(e.health - damamge <= 0)
        {
            StageManager.currentScore++;
            Destroy(g.gameObject);
        }
         
        else
        {
            e.health -= damamge;
            e.transform.position += new Vector3(transform.forward.x * 2, 0, transform.forward.z * 2);

            e.updateDestination(e.transform);

            //e.transform.position -= new Vector3(5, 0, 0);

            e.onceDamaged = true;
            StartCoroutine(Dameged(e));
        }

        //if (g.gameObject.GetComponent<Enemy>() != null)


        
    }

    private IEnumerator Dameged(Enemy e)
    {
        //나중에 콤봊ㅁ
        yield return new WaitForSecondsRealtime(1);

        e.onceDamaged = false;

        //transform.GetChild(0).gameObject.SetActive(false);
    }


    // 이벤트
    private void OnEnable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        EventManager.instance.OnCollisionResult += HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    }

    private void OnDisable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        EventManager.instance.OnCollisionResult -= HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;
    }

    //if(pState == PlayerState.Transformed)

    private void HandleInput()
    {
        //float moveHorizontal = Input.GetAxis("Horizontal");
        //float moveVertical = Input.GetAxis("Vertical");
        //movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;
        //movement = new Vector3(moveHorizontal * moveSpeed, 0.0f, moveVertical * moveSpeed).normalized;
        //movement.normalized;

        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 입력 값이 임계값보다 작으면 0으로 처리
        if (Mathf.Abs(moveHorizontal) < inputThreshold)
        {
            moveHorizontal = 0;
        }
        if (Mathf.Abs(moveVertical) < inputThreshold)
        {
            moveVertical = 0;
        }


        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;

        Debug.Log($"Horizontal: {moveHorizontal}, Vertical: {moveVertical}");

        // 이동 중 여부를 체크
        //isMoving = movement != Vector3.zero;

        // 이동 중일 때만 회전 값을 업데이트
        if (movement != Vector3.zero)
        {
            lastRotation = Quaternion.LookRotation(movement);
        }









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

                animator.SetBool("isRotate", true);

                if (attackCoroutine != null) StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(attack());

                Vector3 newPos = rb.position + rb.transform.forward;
                rb.MovePosition(newPos);
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
        //나중에 콤봊ㅁ
        yield return new WaitForSecondsRealtime(0.2f);
        transform.GetChild(0).gameObject.SetActive(false);
        animator.SetBool("isRotate", true);
    }

    private bool isMoving;
    private Quaternion lastRotation;

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
            //rb.MoveRotation(lastRotation);

            // 움직이는 방향으로 회전
            //if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);

            }

            // 움직이는 방향으로 회전
            //lastRotation = Quaternion.LookRotation(movement);


        }

        else
        {
            rb.velocity = Vector3.zero; // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
            //rb.MoveRotation(lastRotation); // 마지막 회전 값 유지
        }

        //rb.MoveRotation(lastRotation);

        //else rb.velocity = Vector3.zero;  // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
    }

    //void TakeDamage(int damage)
    //{
    //    health -= damage;
    //    if (health <= 0) Die();
    //}

    void Die()
    {
        Debug.Log("Player Died.");
        EventManager.instance.PlayerDied();
    }


    private IEnumerator playerDamage()
    {

        yield return new WaitForSecondsRealtime(1f);
        playerDamaged = false;
        //transform.GetChild(0).gameObject.SetActive(false);
    }



    Coroutine runningCoroutine = null;

    void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        if(!playerDamaged)
        {
            //iskientic으로?
            playerDamaged = true;
            health--;
            transform.position += new Vector3(collidedObject.transform.forward.x * 2, 0, collidedObject.transform.forward.z * 2);
            StartCoroutine(playerDamage());
        }

        



       


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
        EventManager.TriggerSkillUsed(coolDown);
        StartCoroutine(ResetSkillAvailability(coolDown));
    }







    private IEnumerator Weakning()
    {
        PenealtyTime = 3;
        yield return new WaitForSecondsRealtime(1);
        PenealtyTime = 2;
        yield return new WaitForSecondsRealtime(1);
        PenealtyTime = 1;
        yield return new WaitForSecondsRealtime(1);
        PenealtyTime = 0;


        weakning = false;
        damamge = 1;
    }









    void changeFigure()
    {
        if (!weakning)
        {
            damamge = LightObstacle.minusDamage;
            weakning = true;
        } 

        else
        {
            if (runningCoroutine != null)
            {
                StopCoroutine(runningCoroutine);
            }

            runningCoroutine = StartCoroutine(Weakning());
        }


    }

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

