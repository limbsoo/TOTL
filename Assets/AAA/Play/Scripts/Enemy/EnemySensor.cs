using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemySensor : MonoBehaviour
{
    Enemy enemy;

    bool playerUseHide;

    private void Start()
    {
        enemy = gameObject.transform.parent.GetComponent<Enemy>();

        //enemy.OnPlayerUseSkill += HandlePlayerUseSkill;
        playerUseHide = false;
    }


    void HandlePlayerUseSkill(PlayerSkillKinds playerSkillKinds)
    {
        switch (playerSkillKinds)
        {
            case (PlayerSkillKinds.Hide):

                playerUseHide = true;

                StartCoroutine(Function.instance.CountDown(2f, () =>
                {
                    playerUseHide = false;

                }));


                break;
        }
    }



    void OnTriggerExit(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if(other.gameObject.tag == "Player")
            {
                StageManager.instance.OnPlayerUseSkill -= HandlePlayerUseSkill;

                BackOriginPos();

                ////Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

                //enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
                //{
                //    enemy.updateDesitnationPos(enemy.originPos); // 적이 플레이어를 쫓는 함수
                //    enemy.curTarget = "";

                //}));
            
            }


        }

    }

    void BackOriginPos()
    {
        enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
        {
            enemy.updateDesitnationPos(enemy.originPos); // 적이 플레이어를 쫓는 함수
            enemy.curTarget = "";

        }));
    }





    private void OnTriggerStay(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Decoy")
            {
                //Player p = other.gameObject.transform.GetComponent<Player>();

                if(playerUseHide)
                {
                    if(enemy.curTarget != "")
                    {
                        enemy.updateDesitnationPos(enemy.transform.position);
                        enemy.curTarget = "";
                        BackOriginPos();
                    }



                    ////enemy.updateDesitnationPos(enemy.transform.position);
                    //enemy.curTarget = "";

                    ////enemy.chasing = null;
                    //enemy.chasingPlayer(enemy.transform);
                }

                else
                {

                    switch (other.gameObject.tag)
                    {
                        case ("Decoy"):// 충돌한 적이 디코이를 감지했을 때
                            enemy.chasingPlayer(other.gameObject.transform); // 적이 플레이어를 쫓는 함수
                            enemy.curTarget = "Decoy"; // 적의 현재 타겟을 디코이로 설정
                            if (enemy.chasing != null) enemy.chasing = null;
                            break;

                        case ("Player"):

                            // 적이 현재 디코이를 추적 중이지 않을 때만 플레이어를 추적하도록 함
                            if (enemy.curTarget == "Decoy") enemy.curTarget = "Player"; // 타겟을 플레이어로 설정

                            else
                            {
                                enemy.curTarget = "Player"; // 타겟을 플레이어로 설정
                                enemy.chasingPlayer(other.gameObject.transform); // 적이 플레이어를 쫓음
                            }

                            if (enemy.chasing != null) enemy.chasing = null;
                            break;
                    }
                }


                //Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

            }
        }
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;


            //// 플레이어와 충돌한 몬스터만 이벤트를 받음
            //StageManager stageManager = FindObjectOfType<StageManager>();
            //if (stageManager != null)
            //{
            //    stageManager.OnPlayerUseSkill += HandlePlayerUseSkill;
            //}
        }
    }







    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Player")
    //    {
    //        Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

    //        enemy.chasingPlayer(collision.collider.gameObject.transform); // 적이 플레이어를 쫓음
    //    }



    //}






    //private void OnTriggerStay(Collider other)
    //{
    //    //if (StageManager.Sstate == StageState.Play)
    //    //{
    //    //    switch (gameObject.tag)
    //    //    {
    //    //        case ("Player"):
    //    //            Enemy enemy = transform.parent.GetComponent<Enemy>();
    //    //            enemy.chasingPlayer();


    //    //            break;
    //    //    }




    //    //    //switch (other.gameObject.tag)
    //    //    //{
    //    //    //    if (gameObject.tag == "Player")
    //    //    //    {




    //    //    //case ("EnemySensor"):
    //    //    //    if (other.transform.parent != null)
    //    //    //    {
    //    //    //        if (gameObject.tag == "Player")
    //    //    //        {
    //    //    //            chasingPlayer();

    //    //    //        }

    //    //    //        if (Player.instance.summonDecoy)
    //    //    //        {
    //    //    //            if (gameObject.tag == "Decoy")
    //    //    //            {
    //    //    //                chasingPlayer();
    //    //    //                //EventManager.instance.playerDetectedMonster(other.gameObject);
    //    //    //            }


    //    //    //        }

    //    //    //        //else
    //    //    //        //{


    //    //    //        //    chasingPlayer(); //EventManager.instance.playerDetectedMonster(other.gameObject); chasingPlayer();
    //    //    //        //}


    //    //    //    }
    //    //    //    break;
    //    //    //}
    //    //}







    //    //// Start is called before the first frame update
    //    //void Start()
    //    //{

    //    //}

    //    //// Update is called once per frame
    //    //void Update()
    //    //{

    //    //}
    //}
}