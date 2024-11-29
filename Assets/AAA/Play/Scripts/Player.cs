using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Events;
//using UnityEngine.Rendering.VirtualTexturing;


public class Player : MonoBehaviour, Spawn
{
    //public UnityEvent OnUseTeleport;
    public UnityEvent OnUseAttack;
    public UnityEvent OnPlayerUnderAttack;

    //도망치는게임
    //디코이 : 제자리를 목표 지점으로 두고 일정 시간이 지난 뒤 플레이어를 다시 추적

    //public float health { get; private set; }
    //public float moveSpeed { get; private set; }

    //public float damamge { get; private set; }
    //public float coolDown { get; private set; }



    PlayerStats playerStats;


    public static bool useAttack = false;


    public float teleportLength = 10;

    public static bool canUseSkill; // 스킬 사용 가능 여부

    public static bool IsplayerDamaged;

    private Vector3 movement;
    
    public static Rigidbody rb;

    public static bool weakning = false;
    

    Coroutine attackCoroutine = null;

    private PlayerState pState;

    


    public float PenealtyTime { get; private set; }


    public Animator animator;

    //Material[] mat = new Material[2];

    //prePlayer꺼

    PlayerSkillState psState;


    public static float inputThreshold = 0.1f; // 임계값 설정


    public GameObject Decoy;


    //public int gold;


    public float stack;

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }


    //public static Player instance { get; private set; }
    private void Awake()
    {
        IsplayerDamaged = false;
        canUseSkill = true;
        playerStats = DataManager.Instance.saveData.playerStats;

        PenealtyTime = 0;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private float attackDuration;


    private GameObject playerFBX;




    Vector3[] _gridCenters;
    Transform _mapTransform;




    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }













    private void Start()
    {
        stack = 0;

        playerFBX = transform.Find("Player").gameObject;

        animator = playerFBX.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;
        //clips[0].

        //mat = this.GetComponent<Renderer>().materials;

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
        //EventManager.instance.OnPlayerEnterTheLightRange += changeFigure;
        //EventManager.instance.OnCollisionResult += HandleCollisionResult;
        //EventManager.instance.OnEnemyInAttackRange += destoryEnemy;

        StageUI.instance.OnUseSkill += useSkill;
        //JoyStickController.instance.JoyStickMove += Move;
    }

    private void OnDisable()
    {
        //EventManager.instance.OnPlayerEnterTheLightRange -= changeFigure;
        //EventManager.instance.OnCollisionResult -= HandleCollisionResult;
        //EventManager.instance.OnEnemyInAttackRange += destoryEnemy;

        StageUI.instance.OnUseSkill -= useSkill;
    }


    private int DecoyLifeCycle = 3;

    public void useSkill()
    {
        if(SkillCoolDownCoroutine == null)
        {
            switch (psState)
            {
                case PlayerSkillState.Teleport:
                    UseTeleport();
                    break;
                case PlayerSkillState.Hide:

                    break;
                case PlayerSkillState.Decoy:
                    //SkillCoolDownCoroutine = StartCoroutine()

                    GameObject go = Instantiate(Decoy, transform.position, transform.rotation);
                    summonDecoy = true;
                    StartCoroutine(Function.instance.CountDown(DecoyLifeCycle, () => {
                        summonDecoy = !summonDecoy;
                        Destroy(go);
                    }));
                    break;

            }

            SkillCoolDownCoroutine = StartCoroutine(Function.instance.CountDown(playerStats.coolDown, () =>
            {
                SkillCoolDownCoroutine = null;

                ////UIManager.instance.IdleSkillButton(true);
                ////canUseSkill = !canUseSkill;
                //summonDecoy = !summonDecoy;
                //Destroy(go);
            }));

            StageUI.instance.StartCooldown(playerStats.coolDown);
        }




        //if (canUseSkill)
        //{
        //    switch (psState)
        //    {
        //        case PlayerSkillState.Teleport:
        //            UseTeleport();
        //            break;
        //        case PlayerSkillState.Hide:

        //            break;
        //        case PlayerSkillState.Decoy:
        //            UseDecoy();
        //            break;
        //    }

        //    UIManager.instance.StartCooldown(coolDown);
        //}

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

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * playerStats.moveSpeed;

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

                //UIManager.instance.StartCooldown(coolDown);
            }






            ////if (pState == PlayerState.Original)
            //{
            //    //if(canUseSkill) OnUseTeleport?.Invoke();
            //}
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
            //Debug.Log($"movement: {movement}");


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

    public void HandleCollisionResult(GameObject collidedObject)
    {
        Debug.Log("Received collision with: " + collidedObject.name);

        if(!IsplayerDamaged)
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

        StageUI.instance.IdleSkillButton(false);

        StartCoroutine(Function.instance.CountDown(playerStats.coolDown, () => {
            StageUI.instance.IdleSkillButton(true);
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
        StartCoroutine(Function.instance.CountDown(playerStats.coolDown, () => { canUseSkill = !canUseSkill; }));
        //Function.instance.ChangeStateAndDelay(ref canUseSkill, coolDown, () => { canUseSkill = !canUseSkill;});
    }



    public Coroutine SkillCoolDownCoroutine;















    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


    //public void ApplyFieldEffect(FieldEffect fe)
    //{
    //    //switch (fe.m_downerIdx)
    //    //{
    //    //    case 0: // Damage

    //    //        if (!playerDamaged) underAttack();


    //    //        //health -= 1;
    //    //        break;
    //    //    case 1: // Seal
    //    //        StartCoroutine(Weakning());
    //    //        break;
    //    //    case 2: // Slow

    //    //        if (slowEffectedCoroutine != null)
    //    //        {
    //    //            StopCoroutine(slowEffectedCoroutine);
    //    //        }

    //    //        slowEffectedCoroutine = StartCoroutine(ApplySlow(5));



    //    //        break;
    //    //}
    //}


    public bool haveDamaged()
    {
        if(IsplayerDamaged) return true;
        else return false;
    }

/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    //public void underAttack()
    //{
    //    playerDamaged = true;

    //    gameObject.GetComponent<MeshRenderer>().material = mat[1];

    //    StageUI.instance.playerDamaged.SetActive(true);

    //    //gameObject.GetComponent<BoxCollider>().enabled = false;


    //    StartCoroutine(Function.instance.CountDown(0.5f, () => {

    //        StageUI.instance.playerDamaged.SetActive(false);
    //    }));


    //    playerStats.health--;

    //    TextManager.instance.health.text = playerStats.health.ToString();

    //    StartCoroutine(Function.instance.CountDown(1f, () => {

    //        playerDamaged = false;
    //        gameObject.GetComponent<MeshRenderer>().material = mat[0];
    //        //gameObject.GetComponent<BoxCollider>().enabled = true;
    //    }));

    //    //iskientic으로?
    //    //transform.position += new Vector3(collidedObject.transform.forward.x * 2, 0, collidedObject.transform.forward.z * 2);
    //}



    private Coroutine slowEffectedCoroutine;


    public Action<float> OnPlayerDamaged;



    public void Damaged(float value)
    {
        if(IsplayerDamaged == false)
        {
            IsplayerDamaged = true;
            playerStats.health -= value;

            OnPlayerDamaged.Invoke(playerStats.health);

            StartCoroutine(Function.instance.CountDown(1f, () => {
                IsplayerDamaged = false;
            }));

        }
    }



    public void Sealed(float value)
    {
        if (SkillCoolDownCoroutine != null)
        {
            if (StageUI.instance.CheckCoolDown(value))
            {
                StopCoroutine(SkillCoolDownCoroutine);

                SkillCoolDownCoroutine = StartCoroutine(Function.instance.CountDown(value, () =>
                {
                    SkillCoolDownCoroutine = null;
                }));
            }
        }

        else
        {
            SkillCoolDownCoroutine = StartCoroutine(Function.instance.CountDown(value, () =>
            {
                SkillCoolDownCoroutine = null;
            }));

            StageUI.instance.StartCooldown(value);
        }
    }


    public void Slowed(float value)
    {
        if (slowEffectedCoroutine != null)
        {
            StopCoroutine(slowEffectedCoroutine);
        }

        slowEffectedCoroutine = StartCoroutine(Slow(value));

    }



    public IEnumerator Slow(float value) //딜레이는 시간도 더 길게 패널티를 주자
    {
        playerStats.moveSpeed = 15f;

        //playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed  - 15f;
        //yield return new WaitForSecondsRealtime(1);
        yield return new WaitForSecondsRealtime(0.1f);

        playerStats.moveSpeed = 30f;

        //playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed;
    }





}

