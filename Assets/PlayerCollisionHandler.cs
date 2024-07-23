using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("충돌 시작!");
            EventManager.instance.playerCollisionEnemy(collision.gameObject);
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "preEnemy")
        {
            EventManager.instance.prePlayerCollisionPreEnemy(other.gameObject);
        }

        if (gameObject.tag == "AttackRange")
        {
            if(other.gameObject.tag == "Enemy") EventManager.instance.EnemyInAttackRange(other.gameObject);
        }




            //if(other.gameObject.tag == "LightRange")
            //{
            //    if (gameObject.tag == "AttackRange") return;

            //    EventManager.instance.playerEnterTheLightRange();
            //}

            if (other.gameObject.tag == "EnemySensor")
        {
            Debug.Log("적에게 포착");
            EventManager.instance.playerDetectedMonster(other.gameObject);
        }
    }



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