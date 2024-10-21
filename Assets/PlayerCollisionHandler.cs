using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class PlayerCollisionHandler : MonoBehaviour
{
    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때
    private void OnCollisionEnter(Collision collision)
    {
        //switch (collision.collider.tag)
        //{
        //    case ("Enemy"):
        //        EventManager.instance.playerCollisionEnemy(collision.gameObject);
        //        break;

        //    //case ("Goal"):
        //    //    StageManager.instance.arriveGoal(collision.gameObject);
        //    //    break;
        //}



        //if (collision.collider.tag == "Enemy")
        //{
        //    //Debug.Log("충돌 시작!");
        //    EventManager.instance.playerCollisionEnemy(collision.gameObject);
        //}
    }


    private Coroutine decoyCheckCoroutine;



    private Coroutine swampEffectedCoroutine;


    private Coroutine DelayEffectCoroutine;


    //private bool isDelayTriggered = false;
    //public float triggerTime = 2.0f; // 효과를 발동시키기 위한 시간 (초)
    //private float triggerEnterTime = 0.0f; // Trigger가 시작된 시간

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (StageManager.Sstate == StageState.Play)
    //    {
    //        switch (other.gameObject.tag)
    //        {
    //            case ("EffectRange"):
    //                if (gameObject.tag == "Player")
    //                {


    //                    FieldEffect fe = other.gameObject.transform.parent.GetComponent<FieldEffect>();

    //                    if(fe == null)
    //                    {
    //                        isDelayTriggered = false;
    //                        triggerEnterTime = 0.0f;
    //                    }



    //                    else if (fe.m_upperIdx == 1)
    //                    {
    //                        isDelayTriggered = true;
    //                        triggerEnterTime = Time.time;
    //                    }
    //                }
    //                break;


    //            //break;
    //        }

            
    //    }
    //}

    //void OnTriggerExit(Collider other)
    //{
    //    if (StageManager.Sstate == StageState.Play)
    //    {
    //        switch (other.gameObject.tag)
    //        {
    //            //case ("EffectRange"):
    //            //    if (gameObject.tag == "Player")
    //            //    {
    //            //        isDelayTriggered = false; // Trigger 영역을 벗어남
    //            //        triggerEnterTime = 0.0f; // 타이머 초기화
    //            //    }
    //            //    break;

    //            case ("EnemySensor"):
    //                if (gameObject.tag == "Player")
    //                {
    //                    Enemy enemy = other.gameObject.transform.parent.GetComponent<Enemy>();

    //                    enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
    //                    {
    //                        enemy.updateDesitnationPos(enemy.originPos); // 적이 플레이어를 쫓는 함수
    //                        enemy.curTarget = "";

    //                    }));





    //                    //isDelayTriggered = false; // Trigger 영역을 벗어남
    //                    //triggerEnterTime = 0.0f; // 타이머 초기화
    //                }
    //                break;
    //        }


    //    }

    //}




    private void OnTriggerStay(Collider other)
    {
        if (StageManager.Sstate == StageState.Play)
        {
            switch (other.gameObject.tag)
            {
                case ("Enemy"):
                    if(gameObject.tag == "Player")
                    {
                        EventManager.instance.playerCollisionEnemy(gameObject);
                    }


                    break;


                case ("Goal"):
                    StageManager.instance.arriveGoal(other.gameObject);
                    break;

                case ("Gate"):
                    StageManager.instance.arriveGate(other.gameObject);
                    break;


                    //case ("preEnemy"):
                    //    EventManager.instance.prePlayerCollisionPreEnemy(other.gameObject);
                    //    break;


                    //case ("AttackRange"):
                    //    if (other.gameObject.tag == "Enemy")
                    //    {
                    //        if (StageManager.Sstate == StageState.Play) EventManager.instance.EnemyInAttackRange(other.gameObject);
                    //    }
                    //    break;

                    //case ("LightRange"):
                    //    if (gameObject.tag == "AttackRange") return;
                    //    EventManager.instance.playerEnterTheLightRange();
                    //    break;

                    //case ("EnemySensor"):
                    //    if (other.transform.parent != null)
                    //    {





                    //        if (gameObject.tag == "Decoy")
                    //        {
                    //            // 충돌한 적이 디코이를 감지했을 때
                    //            Enemy enemy = other.gameObject.transform.parent.GetComponent<Enemy>();
                    //            enemy.chasingPlayer(gameObject.transform); // 적이 플레이어를 쫓는 함수
                    //            enemy.curTarget = "Decoy"; // 적의 현재 타겟을 디코이로 설정

                    //            if (enemy.chasing != null) enemy.chasing = null;
                    //        }
                    //        else if (gameObject.tag == "Player")
                    //        {
                    //            // 충돌한 적이 플레이어를 감지했을 때
                    //            Enemy enemy = other.gameObject.transform.parent.GetComponent<Enemy>();

                    //            // 적이 현재 디코이를 추적 중이지 않을 때만 플레이어를 추적하도록 함
                    //            if (enemy.curTarget == "Decoy")
                    //            {

                    //                enemy.curTarget = "Player"; // 타겟을 플레이어로 설정
                    //            }

                    //            else
                    //            {
                    //                enemy.curTarget = "Player"; // 타겟을 플레이어로 설정
                    //                enemy.chasingPlayer(gameObject.transform); // 적이 플레이어를 쫓음
                    //            }

                    //            if (enemy.chasing != null) enemy.chasing = null;

                    //        }

                    //    }
                    //    break;


                    //case ("EffectRange"):
                    //    if (gameObject.tag == "Player")
                    //    {
                    //        FieldEffect fe = other.gameObject.transform.parent.GetComponent<FieldEffect>();



                    //        if (fe == null)
                    //        {
                    //            isDelayTriggered = false;
                    //            triggerEnterTime = 0.0f;
                    //        }

                    //        else if (fe.m_upperIdx == 1)
                    //        {
                    //            if (isDelayTriggered)
                    //            {
                    //                if (Time.time - triggerEnterTime >= triggerTime)
                    //                {

                    //                    triggerEnterTime = 0.0f;

                    //                    Player P = gameObject.GetComponent<Player>();

                    //                    P.ApplyDelayEffect(fe);
                    //                }
                    //            }
                    //        }

                    //        else
                    //        {
                    //            if(fe.IsActivated())
                    //            //if(other.gameObject.GetComponent<CapsuleCollider>().enabled)
                    //            {

                    //                Player P = gameObject.GetComponent<Player>();
                    //                P.setEffected(fe.m_downerIdx);
                    //            }


                    //        }

                    //    }
                    //    break;


                    //case ("Item"):
                    //    if (gameObject.tag == "Player")
                    //    {
                    //        Player P = gameObject.GetComponent<Player>();
                    //        P.gold += 100;

                    //        //other.gameObject.SetActive(false);

                    //        Destroy(other.gameObject);
                    //    }

                    //    break;

                    //if (other.gameObject.tag == "Enemy")
                    //{
                    //    if (StageManager.Sstate == StageState.Play) EventManager.instance.EnemyInAttackRange(other.gameObject);
                    //}
                    //break;
            }
        }



















        //switch (other.gameObject.tag)
        //{
        //    case ("Goal"):
        //        StageManager.instance.arriveGoal(other.gameObject);
        //        break;
        //    case ("preEnemy"):
        //        EventManager.instance.prePlayerCollisionPreEnemy(other.gameObject);
        //        break;


        //    case ("AttackRange"):
        //        if (other.gameObject.tag == "Enemy")
        //        {
        //            if (StageManager.Sstate == StageState.Play) EventManager.instance.EnemyInAttackRange(other.gameObject);
        //        }
        //        break;

        //    case ("LightRange"):
        //        if (gameObject.tag == "AttackRange") return;
        //        EventManager.instance.playerEnterTheLightRange();
        //        break;

        //    case ("EnemySensor"):
        //        if (other.transform.parent != null)
        //        {
        //            if (StageManager.Sstate == StageState.Play)
        //            {
        //                if (gameObject.tag == "AttackRange") return;
        //                EventManager.instance.playerDetectedMonster(other.gameObject);
        //            }
        //        }
        //        break;

        //}















        //if (other.gameObject.tag == "preEnemy")
        //{
        //    EventManager.instance.prePlayerCollisionPreEnemy(other.gameObject);
        //}


        ////일정 시간 이후에 목표지점 초기화해서 쫒아가다가 범위 벗어나면 멈추게
        //if (gameObject.tag == "AttackRange")
        //{
        //    //Debug.Log("player attack");

        //    if (other.gameObject.tag == "Enemy")
        //    {
        //        if (StageManager.Sstate == StageState.Play)
        //        {
        //            EventManager.instance.EnemyInAttackRange(other.gameObject);
        //        }


        //    }

        //}




        //if (other.gameObject.tag == "LightRange")
        //{
        //    if (gameObject.tag == "AttackRange") return;

        //    EventManager.instance.playerEnterTheLightRange();
        //}

        //if (other.gameObject.tag == "EnemySensor")
        //{
        //    if(other.transform.parent != null) 
        //    {

        //        //일정시간 후에 멈추게 수정
        //        if (StageManager.Sstate == StageState.Play)
        //        {
        //            //Debug.Log("적에게 포착");

        //            if (gameObject.tag == "AttackRange") return;

        //            EventManager.instance.playerDetectedMonster(other.gameObject);
        //        }


        //    }



        //}
    }



    IEnumerator CheckDecoyStatus(Enemy enemy)
    {
        //while (true)
        {
            // 디코이가 일정 시간 동안 감지되지 않으면 타겟 변경
            float timeoutDuration = 1.0f; // 3초 동안 디코이 감지가 없으면 플레이어로 타겟 변경
            yield return new WaitForSeconds(timeoutDuration);

            // 3초 동안 OnTriggerStay가 호출되지 않으면 타겟을 플레이어로 변경
            if (enemy.curTarget == "Decoy")
            {
                enemy.curTarget = "";

                //enemy.curTarget = "Player"; // 타겟을 플레이어로 변경
                //enemy.chasingPlayer(player); // 적이 플레이어를 추적하도록 설정
                //Debug.Log("Decoy not detected for 3 seconds, switching target to Player.");
            }

            decoyCheckCoroutine = null; // 코루틴 종료 후 초기화
            //}
        }
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (gameObject.tag == "Decoy")
    //    {
    //        // 디코이가 사라졌을 때 타겟을 초기화하거나 플레이어로 변경
    //        Enemy enemy = other.gameObject.transform.parent.GetComponent<Enemy>();

    //        // 디코이가 더 이상 존재하지 않으면 플레이어를 다시 타겟으로 설정
    //        if (enemy.curTarget == "Decoy")
    //        {
    //            enemy.curTarget = ""; // 디코이가 사라지면 타겟 초기화
    //        }
    //    }
    //}







    //if (otherTransform.root.gameObject == gameObject)
    //if (otherTransform.IsChildOf(transform))


    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌중일 때
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.collider.tag == "Enemy")
    //    {
    //        Debug.Log("충돌 중!");
    //    }
    //}

    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌이 끝났을 때
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("충돌 끝!");
    //}




}