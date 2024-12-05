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
    private void Awake()
    {
        IsplayerDamaged = false;
        canUseSkill = true;
        playerStats = DataManager.Instance.saveData.playerStats;

        PenealtyTime = 0;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }


    PlayerStats playerStats;



    public Action<PlayerEvent, float> OnPlayerEvent;





    //public static bool useAttack = false;


    public float teleportLength = 10;

    public static bool canUseSkill; // ��ų ��� ���� ����

    public static bool IsplayerDamaged;

    private Vector3 movement;
    
    public static Rigidbody rb;

    //public static bool weakning = false;
    

    Coroutine attackCoroutine = null;

    private PlayerState pState;

    


    public float PenealtyTime { get; private set; }


    public Animator animator;

    //Material[] mat = new Material[2];

    //prePlayer��

    PlayerSkillState psState;


    public static float inputThreshold = 0.1f; // �Ӱ谪 ����


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

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical) * playerStats.moveSpeed;

        //Debug.Log($"Horizontal: {moveHorizontal}, Vertical: {moveVertical}");

        // �̵� �� ���θ� üũ
        //isMoving = movement != Vector3.zero;

        // �̵� ���� ���� ȸ�� ���� ������Ʈ
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
            float width = StageManager.instance.ReturnMapTransform().localScale.x * 10f; // Unity �⺻ Plane�� ũ��� 10x10 ����
            float height = StageManager.instance.ReturnMapTransform().localScale.z * 10f;
            newPos.x = Mathf.Clamp(newPos.x, StageManager.instance.ReturnMapTransform().position.x - width / 2, StageManager.instance.ReturnMapTransform().position.x + width / 2);
            newPos.z = Mathf.Clamp(newPos.z, StageManager.instance.ReturnMapTransform().position.z - height / 2, StageManager.instance.ReturnMapTransform().position.z + height / 2);
            rb.MovePosition(newPos); // ������ �̵� ó��

            if (movement != Vector3.zero)
            {
                Quaternion newRotation = Quaternion.LookRotation(movement);
                rb.MoveRotation(newRotation);
            }
        }

        else
        {
            rb.velocity = Vector3.zero; // Ű �Է��� ���� �� �ӵ��� 0���� �����Ͽ� �������� ����
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



    public IEnumerator Slow(float value) //�����̴� �ð��� �� ��� �г�Ƽ�� ����
    {
        playerStats.moveSpeed = 15f;

        //playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed  - 15f;
        //yield return new WaitForSecondsRealtime(1);
        yield return new WaitForSecondsRealtime(0.1f);

        playerStats.moveSpeed = 30f;

        //playerStats.moveSpeed = DataManager.Instance.saveData.playerStats.moveSpeed;
    }





}
