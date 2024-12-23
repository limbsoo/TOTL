using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.Events;
using System.Drawing;
using Unity.Mathematics;
//using UnityEngine.Rendering.VirtualTexturing;


public class Player : MonoBehaviour, Spawn
{
    private void Awake()
    {
        IsplayerDamaged = false;
        canUseSkill = true;
        playerStats = DataManager.Instance.saveData.playerStats;
        playerStats.playerSkillKind = (PlayerSkillKinds)Enum.GetValues(typeof(PlayerSkillKinds)).GetValue(DataManager.Instance.saveData.playerCharacterIdx + 1);

        PenealtyTime = 0;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    PlayerStats playerStats;



    public Action<PlayerEvent, float> OnPlayerEvent;





    //public static bool useAttack = false;


    public float teleportLength = 10;

    public static bool canUseSkill; // 스킬 사용 가능 여부

    public static bool IsplayerDamaged;

    private Vector3 movement;
    
    public static Rigidbody rb;

    //public static bool weakning = false;
    

    Coroutine attackCoroutine = null;

    private PlayerState pState;

    


    public float PenealtyTime { get; private set; }


    public Animator animator;

    //Material[] mat = new Material[2];

    //prePlayer꺼

    PlayerSkillState psState;


    public static float inputThreshold = 0.1f; // 임계값 설정


    public GameObject Decoy;



    public float stack;

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }



    private GameObject playerFBX;
    Vector3[] _gridCenters;
    Transform _mapTransform;




    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }







    Material hidingMaterial;





    private void Start()
    {
        stack = 0;

        playerFBX = transform.Find("Player").gameObject;

        animator = playerFBX.GetComponent<Animator>();

        AnimationClip[] clips = animator.runtimeAnimatorController.animationClips;


        hidingMaterial = playerFBX.transform.GetChild(1).GetComponent<Renderer>().materials[0];


        curIdle = 0;

        //Renderer renderer = playerFBX.transform.GetChild(1).GetComponent<Renderer>();

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

        //CameraController.Vector3 = transform.position;

    }
    void FixedUpdate()
    {
        Move();
        CameraController.Vector3 = transform.position;

        //Debug.Log($"Movement: {movement}, LastRotation: {lastRotation.eulerAngles}");
    }


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






    public void InitPlayerState()
    {
        if (SkillCoolDownCoroutine != null)
        {
            //switch (playerStats.playerSkillKind)
            //{
            //    case PlayerSkillKinds.Hide:

            //        hidingMaterial.color = new UnityEngine.Color(hidingMaterial.color.r, hidingMaterial.color.g, hidingMaterial.color.b, 1f);
            //        break;

            //    case PlayerSkillKinds.Decoy:

            //        summonDecoy = !summonDecoy;
            //        //Destroy(go);
            //        break;

            //}

            SkillCoolDownCoroutine = null;

        }
    }











    private int DecoyLifeCycle = 3;


    public bool tmpUseHide = false;




    public Action<PlayerSkillKinds, float, float> OnSkillEffect;

    public void useSkill()
    {
        if(SkillCoolDownCoroutine == null)
        {
            SoundManager.instance.Play("Skill", SoundCatecory.Effect, false);

            //playerStats

            switch (playerStats.playerSkillKind)
            {
                case PlayerSkillKinds.Teleport:
                    UseTeleport();
                    break;

                case PlayerSkillKinds.Hide:
                    hidingMaterial.color = new UnityEngine.Color(hidingMaterial.color.r, hidingMaterial.color.g, hidingMaterial.color.b, 0.8f);

                    OnSkillEffect?.Invoke(PlayerSkillKinds.Hide, playerStats.effectValue, playerStats.coolDown);

                    StartCoroutine(Function.instance.CountDown(playerStats.effectValue, () => {
                        hidingMaterial.color = new UnityEngine.Color(hidingMaterial.color.r, hidingMaterial.color.g, hidingMaterial.color.b, 1f);

                        //hidingMaterial.color = new UnityEngine.Color(0, 0, 0, 1);
                    }));
                    break;

                case PlayerSkillKinds.Decoy:
                    OnSkillEffect?.Invoke(PlayerSkillKinds.Decoy, playerStats.effectValue, playerStats.coolDown);

                    GameObject go = Instantiate(Decoy, transform.position, transform.rotation);
                    summonDecoy = true;

                    StartCoroutine(Function.instance.CountDown(playerStats.effectValue, () => {
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


    }

    int curIdle;

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

            if(curIdle == 0)
            {
                animator.SetInteger("Idle", 1);
                curIdle = 1;
            }

            
        }

        else
        {
            if(curIdle == 1)
            {
                animator.SetInteger("Idle", 0);
                curIdle = 0;
            }

            
        }
        
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
            //_mapTransform

            //Debug.Log($"movement: {movement}");
            Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            float width = _mapTransform.localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
            float height = _mapTransform.localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, _mapTransform.position.x - width / 2, _mapTransform.position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, _mapTransform.position.z - height / 2, _mapTransform.position.z + height / 2);
            rb.MovePosition(newPos); // 물리적 이동 처리



            ////Debug.Log($"movement: {movement}");
            //Vector3 newPos = rb.position + movement * Time.fixedDeltaTime;
            //float width = StageManager.instance.ReturnMapTransform().localScale.x * 10f; // Unity 기본 Plane의 크기는 10x10 단위
            //float height = StageManager.instance.ReturnMapTransform().localScale.z * 10f;
            //newPos.x = Mathf.Clamp(newPos.x, StageManager.instance.ReturnMapTransform().position.x - width / 2, StageManager.instance.ReturnMapTransform().position.x + width / 2);
            //newPos.z = Mathf.Clamp(newPos.z, StageManager.instance.ReturnMapTransform().position.z - height / 2, StageManager.instance.ReturnMapTransform().position.z + height / 2);
            //rb.MovePosition(newPos); // 물리적 이동 처리

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


    Coroutine runningCoroutine = null;



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









    public bool haveDamaged()
    {
        if(IsplayerDamaged) return true;
        else return false;
    }


    private Coroutine slowEffectedCoroutine;


















    public Action<float> OnPlayerDamaged;



    public void Damaged(float value)
    {
        if(IsplayerDamaged == false)
        {
            IsplayerDamaged = true;
            playerStats.health -= value;

            OnPlayerDamaged.Invoke(playerStats.health);

            SoundManager.instance.Play("Damage", SoundCatecory.Effect, false);

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

