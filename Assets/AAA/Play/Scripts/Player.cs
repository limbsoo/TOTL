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
using UnityEngine.Playables;
//using UnityEngine.Rendering.VirtualTexturing;


public class Player : MonoBehaviour, Spawn
{
    PlayerStats Stats;
    PlayerStats OriginStats;

    Vector3[] _gridCenters;
    Transform _mapTransform;
    float inputThreshold = 0.1f;
    bool isMoving;
    Quaternion lastRotation;

    public Animator animator;
    public static Rigidbody rb;
    public GameObject Decoy;
    GameObject DecoyObject;

    public Action<PlayerEvent, float> OnPlayerEvent;
    public float teleportLength = 10;
    public static bool IsplayerDamaged;
    private Vector3 movement;
    public float stack;

    Material hidingMaterial;

    public Action<PlayerSkillKinds, float, float> OnSkillEffect;

    int curIdle;

    //Coroutine slowEffectedCoroutine;

    public Action<float> OnPlayerDamaged;

    Coroutine SkillCoolDown;
    Coroutine SkillEffectDuration;

    Coroutine DamageCoolDown;
    Coroutine SlowEffectDuration;
    Coroutine SealEffectDuration;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        Stats = DataManager.Instance.saveData.playerStats;
        Stats.playerSkillKind = (PlayerSkillKinds)Enum.GetValues(typeof(PlayerSkillKinds)).GetValue(DataManager.Instance.saveData.playerCharacterIdx + 1);

        OriginStats = new PlayerStats();
        OriginStats.moveSpeed = Stats.moveSpeed;

        StageManager.instance.OnClickSkillButton += SkillChangeState;

        IsplayerDamaged = false;
    }

    void Start()
    {
        GameObject go = transform.Find("Player").gameObject;
        animator = go.GetComponent<Animator>();
        hidingMaterial = go.transform.GetChild(1).GetComponent<Renderer>().materials[0];

        stack = 0;
        curIdle = 0;
    }

    void Update()
    {
        if (StageManager.Sstate == StageState.Play) { HandleInput(); }
    }
    void FixedUpdate()
    {
        Move();
        CameraController.Vector3 = transform.position;
    }


    public PlayerStats GetPlayerStats()
    {
        return Stats;
    }




    public void SkillChangeState()
    {
        if (SkillCoolDown == null)
        {
            SoundManager.instance.Play("Skill", SoundCatecory.Effect, false);

            switch (Stats.playerSkillKind)
            {
                case PlayerSkillKinds.Teleport:
                    Vector3 teleportPosition = rb.position + rb.transform.forward * teleportLength;
                    rb.MovePosition(teleportPosition);
                    break;

                case PlayerSkillKinds.Hide:
                    hidingMaterial.color = new UnityEngine.Color(hidingMaterial.color.r, hidingMaterial.color.g, hidingMaterial.color.b, 0.8f);
                    SkillEffectDuration = StartCoroutine(Function.instance.CountDown(Stats.effectValue, () =>
                    {
                        ResetSkillEffect();
                    }));
                    break;

                case PlayerSkillKinds.Decoy:
                    DecoyObject = Instantiate(Decoy, transform.position, transform.rotation);
                    SkillEffectDuration = StartCoroutine(Function.instance.CountDown(Stats.effectValue, () =>
                    {
                        ResetSkillEffect();
                    }));
                    break;

            }

            OnSkillEffect?.Invoke(Stats.playerSkillKind, Stats.effectValue, Stats.coolDown);
            StageUI.instance.StartCooldown(Stats.coolDown);

            SkillCoolDown = StartCoroutine(Function.instance.CountDown(Stats.coolDown, () =>
            {
                ResetSkillUse();
            }));
        }
    }

    void ResetSkillEffect()
    {
        if (SkillEffectDuration != null) 
        {
            StopCoroutine(SkillEffectDuration);
            SkillEffectDuration = null;
        }

        switch (Stats.playerSkillKind)
        {
            case PlayerSkillKinds.Hide:
                hidingMaterial.color = new UnityEngine.Color(hidingMaterial.color.r, hidingMaterial.color.g, hidingMaterial.color.b, 1f);
                break;

            case PlayerSkillKinds.Decoy:
                Destroy(DecoyObject);
                break;
        }
    }

    void ResetSkillUse()
    {
        if (SkillCoolDown != null)
        {
            StopCoroutine(SkillCoolDown);
            SkillCoolDown = null;
        }

        StageUI.instance.ResetSkillButtonCanUse();
    }





















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

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * Stats.moveSpeed;



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




    public void Damaged(float value)
    {
        if (DamageCoolDown == null)
        {
            SoundManager.instance.Play("Damage", SoundCatecory.Effect, false);

            Stats.health -= value;

            DamageCoolDown = StartCoroutine(Function.instance.CountDown(1, () => {
                DamageCoolDown = null;
            }));

            OnPlayerDamaged.Invoke(Stats.health);
        }
    }



    public void Sealed(float value)
    {
        if(SealEffectDuration != null)
        {
            if (StageUI.instance.CheckCoolDown(value))
            {
                StopCoroutine(SealEffectDuration);

                SealEffectDuration = StartCoroutine(Function.instance.CountDown(value, () =>
                {
                    SealEffectDuration = null;
                }));
            }
        }

        else
        {
            SealEffectDuration = StartCoroutine(Function.instance.CountDown(value, () =>
            {
                SealEffectDuration = null;
            }));

            StageUI.instance.StartCooldown(value);
        }

    }


    public void Slowed(float value)
    {
        if (SlowEffectDuration != null)
        {
            StopCoroutine(SlowEffectDuration);
        }

        SlowEffectDuration = StartCoroutine(Slow(OriginStats.moveSpeed / 2, OriginStats.moveSpeed));

    }



    public IEnumerator Slow(float value, float originValue) //딜레이는 시간도 더 길게 패널티를 주자
    {
        Stats.moveSpeed = OriginStats.moveSpeed / 2;

        Debug.Log("move speed " + Stats.moveSpeed.ToString());


        yield return new WaitForSecondsRealtime(0.1f);
        Stats.moveSpeed = OriginStats.moveSpeed;



        //Stats.moveSpeed = 15f;

        ////playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed  - 15f;
        ////yield return new WaitForSecondsRealtime(1);
        //yield return new WaitForSecondsRealtime(0.1f);

        //Stats.moveSpeed = 30f;

        ////playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed;
    }





    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }

    public void InitState()
    {
        Stats.moveSpeed = OriginStats.moveSpeed;

        ResetSkillEffect();
        ResetSkillUse();

        if (DamageCoolDown != null)
        {
            StopCoroutine(DamageCoolDown);
            DamageCoolDown = null;
        }

        if (SlowEffectDuration != null)
        {
            StopCoroutine(SlowEffectDuration);
            SlowEffectDuration = null;
        }

        if (SealEffectDuration != null)
        {
            StopCoroutine(SealEffectDuration);
            SealEffectDuration = null;
        }
    }

}

