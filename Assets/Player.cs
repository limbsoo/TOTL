using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Events;
using UnityEngine.Rendering.VirtualTexturing;


public class Player : MonoBehaviour
{
    //public UnityEvent OnUseTeleport;
    public UnityEvent OnUseAttack;
    public UnityEvent OnPlayerUnderAttack;

    //도망치는게임
    //디코이 : 제자리를 목표 지점으로 두고 일정 시간이 지난 뒤 플레이어를 다시 추적

    public float health { get; private set; }
    public float moveSpeed { get; private set; }

    public float damamge { get; private set; }
    public float coolDown { get; private set; }


    public static bool useAttack = false;


    public float teleportLength = 10;

    public static bool canUseSkill = true; // 스킬 사용 가능 여부

    public static bool playerDamaged = false;

    private Vector3 movement;
    
    public static Rigidbody rb;

    public static bool weakning = false;
    

    Coroutine attackCoroutine = null;

    private PlayerState pState;

    


    public float PenealtyTime { get; private set; }


    public Animator animator;

    Material[] mat = new Material[2];

    //prePlayer꺼

    PlayerSkillState psState;


    public static float inputThreshold = 0.1f; // 임계값 설정


    public GameObject Decoy;







    public static Player instance { get; private set; }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            GameData data = DataManager.Instance.data;

            //PlayerData playerData = GameManager.instance.playerData;
            health = data.health;
            moveSpeed = data.moveSpeed;
            damamge = data.damamge;
            coolDown = data.coolDown;


            switch (data.skill)
            {
                case 0:
                    psState = PlayerSkillState.Teleport;
                    break;
                case 1:
                    psState = PlayerSkillState.Hide;
                    break;
                case 2:
                    psState = PlayerSkillState.Decoy;
                    break;
            }

            psState = PlayerSkillState.Decoy;


            PenealtyTime = 0;
            animator = GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            pState = PlayerState.Original;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private float attackDuration;


    private GameObject playerFBX;



    private void Start()
    {


        playerFBX = transform.Find("Player").gameObject;

        animator = playerFBX.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        //clips[0].

        mat = this.GetComponent<Renderer>().materials;

        ////gameObject.GetComponent<MeshRenderer>().material = mat[1];
        //AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;

        //foreach (AnimationClip clip in clips)
        //{
        //    if (clip.name == "Attack")
        //    {
        //        attackDuration = clip.length;
        //        break;
        //    }
        //}


    }

    void Update()
    {
        if(StageManager.Sstate == StageState.Play)
        {

            HandleInput();
            
        }

        CameraController.Vector3 = transform.position;

    }
    void FixedUpdate()
    {
        Move();
        //Debug.Log($"Movement: {movement}, LastRotation: {lastRotation.eulerAngles}");
    }

    public void destoryEnemy(GameObject g)
    {
        if (!useAttack) return;

        Enemy e = g.gameObject.GetComponent<Enemy>();

        if (e.onceDamaged) return;

        e.damaged(damamge, transform.forward);

        //StageManager.instance.psystem.transform.position = rb.position;



        //if (e.health - damamge <= 0)
        //{
        //    //e.startEffect();
        //    StageManager.currentScore++;
        //    Debug.Log("Destroy");

        //    e.setNullNMagent();
        //    //Destroy(g.gameObject);
        //    Debug.Log("적 하나 삭제완료");
        //}

        //else
        //{
        //    e.startEffect();
        //    e.health -= damamge;
        //    e.transform.position += new Vector3(transform.forward.x * 2, 0, transform.forward.z * 2);

        //    e.updateDestination(e.transform);

        //    //e.transform.position -= new Vector3(5, 0, 0);

        //    e.onceDamaged = true;
        //    StartCoroutine(Dameged(e));



        //}



    }

    //private IEnumerator Dameged(Enemy e)
    //{
    //    //나중에 콤봊ㅁ
    //    yield return new WaitForSecondsRealtime(1);

    //    e.onceDamaged = false;

    //    //transform.GetChild(0).gameObject.SetActive(false);
    //}


    // 이벤트
    private void OnEnable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        EventManager.instance.OnCollisionResult += HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;

        UIManager.instance.OnUseSkill += useSkill;
        //JoyStickController.instance.JoyStickMove += Move;
    }

    private void OnDisable()
    {
        EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        EventManager.instance.OnCollisionResult -= HandleCollisionResult;
        EventManager.instance.OnEnemyInAttackRange += destoryEnemy;

        UIManager.instance.OnUseSkill -= useSkill;
    }

    public void useSkill()
    {
        if (canUseSkill)
        {
            switch (psState)
            {
                case PlayerSkillState.Teleport:
                    UseTeleport();
                    break;
                case PlayerSkillState.Hide:

                    break;
                case PlayerSkillState.Decoy:
                    UseDecoy();
                    break;
            }

            UIManager.instance.StartCooldown(coolDown);
        }

    }


    //if(pState == PlayerState.Transformed)

    private void HandleInput()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");


        if(JoyStickController.instance.inputVector.x != 0 || JoyStickController.instance.inputVector.y != 0 )
        {
            moveHorizontal = JoyStickController.instance.inputVector.x;
            moveVertical = JoyStickController.instance.inputVector.y;
        }

        if (Mathf.Abs(moveHorizontal) < inputThreshold) moveHorizontal = 0;
        if (Mathf.Abs(moveVertical) < inputThreshold) moveVertical = 0;

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * moveSpeed;

        //Debug.Log($"Horizontal: {moveHorizontal}, Vertical: {moveVertical}");

        // 이동 중 여부를 체크
        //isMoving = movement != Vector3.zero;

        // 이동 중일 때만 회전 값을 업데이트
        if (movement != Vector3.zero)
        {
            lastRotation = Quaternion.LookRotation(movement);
            animator.SetInteger("Idle", 1);
        }

        else
        {
            animator.SetInteger("Idle", 0);
        }
        


        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (canUseSkill)
            {
                switch (psState)
                {
                    case PlayerSkillState.Teleport:
                        UseTeleport();
                        break;
                    case PlayerSkillState.Hide:
                        
                        break;
                    case PlayerSkillState.Decoy:
                        UseDecoy();
                        break;
                }

                UIManager.instance.StartCooldown(coolDown);
            }






            //if (pState == PlayerState.Original)
            {
                //if(canUseSkill) OnUseTeleport?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!useAttack)
            {
                useAttack = true;
                OnUseAttack?.Invoke();
            }
        }
    }

    public void UseAttack()
    {
        animator.SetBool("isRotate", true);
        animator.Play("rotatePlayer");
        //transform.GetChild(0).gameObject.SetActive(true);

        //animator.SetTrigger("Attack");


        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(Function.instance.CountDown(attackDuration, () => { 
            animator.SetBool("isRotate", false);
            useAttack = false;
        }));
    }


    private bool isMoving;
    private Quaternion lastRotation;

    public void Move()
    {
        if (StageManager.Sstate == StageState.Edit)
        {
            rb.velocity = Vector3.zero;
            return;
        }


        if (movement != Vector3.zero)
        {
            Debug.Log($"movement: {movement}");


            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            float width = StageManager.instance.mapTransform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
            float height = StageManager.instance.mapTransform.localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, StageManager.instance.mapTransform.position.x - width / 2, StageManager.instance.mapTransform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, StageManager.instance.mapTransform.position.z - height / 2, StageManager.instance.mapTransform.position.z + height / 2);
            rb.MovePosition(newPos); // 물리적 이동 처리

            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);
            }


        }

        else
        {
            rb.velocity = Vector3.zero; // 키 입력이 없을 때 속도를 0으로 설정하여 움직임을 멈춤
            movement = Vector3.zero;
        } 
    }

    //void TakeDamage(int damage)
    //{
    //    health -= damage;
    //    if (health <= 0) Die();
    //}

    //void Die()
    //{
    //    Debug.Log("Player Died.");
    //    EventManager.instance.PlayerDied();
    //}



    Coroutine runningCoroutine = null;

    void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        if(!playerDamaged)
        {
            OnPlayerUnderAttack?.Invoke();
        }
    }



    public bool summonDecoy = false;


    public void UseDecoy()
    {
        GameObject go = Instantiate(Decoy, transform.position, transform.rotation);

        canUseSkill = false;
        summonDecoy = true;

        UIManager.instance.IdleSkillButton(false);

        StartCoroutine(Function.instance.CountDown(coolDown, () => {
            UIManager.instance.IdleSkillButton(true);
            canUseSkill = !canUseSkill;
            summonDecoy = !summonDecoy;
            Destroy(go);
        }));
    }

    public void UseTeleport()
    {
        Vector3 teleportPosition = rb.position + rb.transform.forward * teleportLength;
        rb.MovePosition(teleportPosition);

        Debug.Log("Teleport used!");

        canUseSkill = false;
        StartCoroutine(Function.instance.CountDown(coolDown, () => { canUseSkill = !canUseSkill; }));
        //Function.instance.ChangeStateAndDelay(ref canUseSkill, coolDown, () => { canUseSkill = !canUseSkill;});
    }




/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    public void ApplyFieldEffect(FieldEffect fe)
    {
        switch (fe.m_downerIdx)
        {
            case 0: // Damage

                if (!playerDamaged) underAttack();


                //health -= 1;
                break;
            case 1: // Seal
                StartCoroutine(Weakning());
                break;
            case 2: // Slow

                if (slowEffectedCoroutine != null)
                {
                    StopCoroutine(slowEffectedCoroutine);
                }

                slowEffectedCoroutine = StartCoroutine(ApplySlow(5));



                break;
        }
    }


/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public void underAttack()
    {
        playerDamaged = true;

        gameObject.GetComponent<MeshRenderer>().material = mat[1];

        UIManager.instance.playerDamaged.SetActive(true);

        //gameObject.GetComponent<BoxCollider>().enabled = false;


        StartCoroutine(Function.instance.CountDown(0.5f, () => {

            UIManager.instance.playerDamaged.SetActive(false);
        }));


        health--;
        StartCoroutine(Function.instance.CountDown(1f, () => {

            playerDamaged = false;
            gameObject.GetComponent<MeshRenderer>().material = mat[0];
            //gameObject.GetComponent<BoxCollider>().enabled = true;
        }));

        //iskientic으로?
        //transform.position += new Vector3(collidedObject.transform.forward.x * 2, 0, collidedObject.transform.forward.z * 2);
    }



    public IEnumerator Weakning()
    {
        //while(PenealtyTime > 0)
        //{

        //}

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


    public IEnumerator slowliy() //딜레이는 시간도 더 길게 패널티를 주자
    {
        moveSpeed = 10;
        //yield return new WaitForSecondsRealtime(1);
        yield return new WaitForSecondsRealtime(0.1f);
        moveSpeed = 20;
    }

    private Coroutine slowEffectedCoroutine;


    public IEnumerator ApplySlow(float delay) //딜레이는 시간도 더 길게 패널티를 주자
    {
        moveSpeed = 10;
        //yield return new WaitForSecondsRealtime(1);
        yield return new WaitForSecondsRealtime(delay);
        moveSpeed = 20;
    }


    private Coroutine SealSkillCoroutine;



    public void ApplyDelayEffect(FieldEffect fe)
    {
        switch (fe.m_downerIdx)
        {
            case 0: // Damage

                if (!playerDamaged) underAttack();


                //health -= 1;
                break;
            case 1: // Seal
                    //StartCoroutine(Weakning());

                if (SealSkillCoroutine != null)
                {
                    StopCoroutine(SealSkillCoroutine);
                }

                SealSkillCoroutine = StartCoroutine(UIManager.instance.SEalCoroutine(5));



                break;
            case 2: // Slow

                if (slowEffectedCoroutine != null)
                {
                    StopCoroutine(slowEffectedCoroutine);
                }

                slowEffectedCoroutine = StartCoroutine(ApplySlow(5));



                break;
        }
    }



    public void setEffected(int idx)
    {
        switch (idx)
        {
            case 0: // Damage

                if(!playerDamaged) underAttack();


                //health -= 1;
                break;
            case 1: // Seal
                    //StartCoroutine(Weakning());
                if (SealSkillCoroutine != null)
                {
                    StopCoroutine(SealSkillCoroutine);
                }

                SealSkillCoroutine = StartCoroutine(UIManager.instance.SEalCoroutine(5));



                break;
            case 2: // Slow

                if (slowEffectedCoroutine != null)
                {
                    StopCoroutine(slowEffectedCoroutine);
                }

                slowEffectedCoroutine = StartCoroutine(slowliy());


                
                break;
        }
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
    //    StageManager.instance.players[b].SetActive(true);
    //    StageManager.instance.players[b].transform.position = StageManager.instance.players[a].transform.position;
    //    StageManager.instance.players[b].transform.rotation = StageManager.instance.players[a].transform.rotation;

    //    //rb = StageManager.instance.players[a].GetComponent<Rigidbody>();

    //    StageManager.instance.players[a].SetActive(false);
    //    StageManager.currentPlayerIdx = b;

    //    //CameraController.instance.transform.position = new Vector3(StageManager.instance.players[b].transform.position.x, StageManager.instance.players[b].transform.position.y + 50, StageManager.instance.players[b].transform.position.z - 30);

    //}


    //private IEnumerator transforming()
    //{
    //    yield return new WaitForSecondsRealtime(3);

    //    Debug.Log("호출코루틴");
    //    suffleFigure(1, 0);

    //}

}

