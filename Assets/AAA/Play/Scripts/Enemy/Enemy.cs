using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
//using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;
using static UnityEngine.GraphicsBuffer;


public class Enemy : MonoBehaviour, Spawn
{

    Vector3[] _gridCenters;
    Transform _mapTransform;


    public void Init(Vector3[] gridCenters, Transform mapTransform)
    {
        _gridCenters = gridCenters;
        _mapTransform = mapTransform;
    }



    Material[] mat = new Material[2];




    public bool onceDamaged = false;



    public string curTarget;


    [SerializeField]
    NavMeshAgent nmAgent;
    [SerializeField]
    Transform target;


    public Vector3 originPos;






    public Coroutine chasing;


    public Animator animator;

    private GameObject enemyFBX;


    public Chase chase;


    PlayerSkillKinds _playerUsedSkill;

    private void Start()
    {
        chase = Chase.None;



        nmAgent = GetComponent<NavMeshAgent>();
        //target = Player.instance.transform;

        target = transform;

        transform.GetChild(0).gameObject.GetComponent<CapsuleCollider>().enabled = true;

        mat = this.GetComponent<Renderer>().materials;

        originPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);


        enemyFBX = transform.Find("Enemy").gameObject;
        animator = enemyFBX.GetComponent<Animator>();


        curIdle = 0;

        _playerUsedSkill = PlayerSkillKinds.None;


        StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;
    }



    public void UpdateDetectedList(GameObject go, bool isAdd)
    {
        if(isAdd)
        {
            detectedList.Add(go);


            if (chasingTarget == null)
            {
                chasingTarget = go;
            }

            else
            {
                if(go.CompareTag("Decoy"))
                {
                    chasingTarget = go;
                }
            }

        }

        else
        {
            detectedList.Remove(go);
            detectedList.RemoveAll(gameobject => gameobject == null);


            if (detectedList.Count > 0)
            {
                chasingTarget = detectedList[0];
            }

            else
            {
                chasingTarget = null;
            }
        }


    }



    private float ChaseUpdateInterval = 0.2f; // 재추적 주기 (초)
    private float timeSinceLastUpdate = 0f;

    public List<GameObject> detectedList = new List<GameObject>();
    public GameObject chasingTarget;

    public void Update()
    {
        // 타겟 포지션 업데이트 되면 업데이트?



        timeSinceLastUpdate += Time.deltaTime;

        if (timeSinceLastUpdate >= ChaseUpdateInterval)
        {
            if (chasingTarget != null)
            {
                if (_playerUsedSkill == PlayerSkillKinds.Hide)
                {
                    updateDesitnationPos(originPos);
                }

                else
                {
                    updateDesitnationPos(chasingTarget.transform.position);
                }

                
            }

            else
            {
                if (detectedList.Count > 0)
                {
                    detectedList.RemoveAll(gameobject => gameobject == null);

                    if (detectedList.Count > 0)
                    {
                        chasingTarget = detectedList[0];
                        updateDesitnationPos(chasingTarget.transform.position);
                    }
                }

                else
                {
                    updateDesitnationPos(originPos);
                }
            }

            UpdateViewDirection();

            timeSinceLastUpdate = 0f;
        }

    }

    void UpdateViewDirection()
    {
        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
        Vector2 steeringTarget = new Vector2(nmAgent.steeringTarget.z, nmAgent.steeringTarget.x);

        //방향을 구한 뒤, 역함수로 각을 구한다.
        Vector2 dir = steeringTarget - forward;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        //방향 적용
        transform.eulerAngles = Vector3.up * angle;

    }



    /// <summary>
    /// 0 : Stay
    /// 1 : Run
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>


    void ChagngeMotion(int a, int b)
    {
        if (curIdle == a)
        {
            animator.SetInteger("Idle", b);
            curIdle = b;
        }
    }



    void HandlePlayerUseSkill(PlayerSkillKinds playerSkillKinds, float effectValue, float coolDown)
    {
        switch (playerSkillKinds)
        {
            case (PlayerSkillKinds.Hide):

                _playerUsedSkill = PlayerSkillKinds.Hide;

                StartCoroutine(Function.instance.CountDown(effectValue, () =>
                {
                    _playerUsedSkill = PlayerSkillKinds.None;
                }));

                break;

            //case (PlayerSkillKinds.Decoy):

            //    StartCoroutine(Function.instance.CountDown(effectValue, () =>
            //    {
            //        if (enemy.curTarget == "Decoy" || enemy.curTarget == "")
            //        {
            //            //StageManager.instance.OnPlayerUseSkill -= HandlePlayerUseSkill;
            //            BackOriginPos();

            //        }
            //    }));

            //    break;
        }
    }








































    //private ParticleSystem psystem;
    //public void startEffect()
    //{
    //    psystem = transform.GetChild(2).gameObject.GetComponent<ParticleSystem>();
    //    psystem.Play();
    //}



    //public void setNullNMagent()
    //{
    //    nmAgent = null;
    //    Destroy(this.gameObject);
    //}


    public void updateDesitnationPos(Vector3 pos)
    {
        if (StageManager.Sstate == StageState.Play)
        {
            if (Math.Abs(nmAgent.destination.x - transform.position.x) <= 1 && Math.Abs(nmAgent.destination.z - transform.position.z) <= 1)
            {
                ChagngeMotion(1, 0);
            }

            else
            {
                ChagngeMotion(0, 1);
            }

            if (this.nmAgent == null) return;

            nmAgent.SetDestination(pos);
        }

       

    }

    int curIdle;

    //public void Update()
    //{
    //    if(curTarget == "" && Math.Abs(nmAgent.destination.x - transform.position.x) <= 5 && Math.Abs(nmAgent.destination.z - transform.position.z) <= 1 )
    //    {
    //        if(curIdle == 1)
    //        {
    //            animator.SetInteger("Idle", 0);
    //            curIdle = 0;
    //        }

            
    //    }

    //    else
    //    {
    //        if(curIdle == 0)
    //        {
    //            animator.SetInteger("Idle", 1);
    //            curIdle = 1;
    //        }

            

    //        Vector2 forward = new Vector2(transform.position.z, transform.position.x);
    //        Vector2 steeringTarget = new Vector2(nmAgent.steeringTarget.z, nmAgent.steeringTarget.x);

    //        //방향을 구한 뒤, 역함수로 각을 구한다.
    //        Vector2 dir = steeringTarget - forward;
    //        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

    //        //방향 적용
    //        transform.eulerAngles = Vector3.up * angle;
    //    }

    //}




    public void updateDestination(Transform tf)
    {
        if (StageManager.Sstate == StageState.Play)
        {
            if (this.nmAgent == null) return;

            if (tf.position.y > 1)
            {
                nmAgent.SetDestination(new Vector3(tf.position.x, 1, tf.position.z));
            }

            else nmAgent.SetDestination(tf.position);
        }
    }



    //public void damaged(float damamge, Vector3 forward)
    //{
    //    if(health - damamge <= 0)
    //    {
    //        StageManager.instance.currentScore++;
    //        setNullNMagent();
    //    }

    //    else
    //    {
    //        startEffect();
    //        health -= damamge;
    //        transform.position += new Vector3(forward.x * 2, 0, forward.z * 2);
    //        updateDestination(transform);
    //        onceDamaged = true;

    //        gameObject.GetComponent<MeshRenderer>().material = mat[1];

    //        StartCoroutine(Function.instance.CountDown(1f, () =>
    //        {
    //            onceDamaged = false;
    //            gameObject.GetComponent<MeshRenderer>().material = mat[0];
    //        }));
    //    }
    //}


    //public void chasingPlayer(Transform tr)
    //{
    //    updateDestination(tr);
    //}

}

