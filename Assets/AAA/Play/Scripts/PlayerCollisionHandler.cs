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



    private void OnTriggerStay(Collider other)
    {
        if (StageManager.Sstate == StageState.Play)
        {
            switch (other.gameObject.tag)
            {
                case ("Enemy"):
                    if (gameObject.tag == "Player")
                    {
                        Player p = gameObject.GetComponent<Player>();
                        p.Damaged(1);
                    }
                    break;


                case ("Goal"):
                    StageManager.instance.arriveGoal(other.gameObject);
                    break;

                case ("Gate"):
                    StageManager.instance.arriveGate(other.gameObject);
                    break;
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