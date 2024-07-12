using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
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
        //this.gameObject

        //attack range 종류별

        if(other.gameObject.tag == "LightRange")
        {
            if (gameObject.tag == "AttackRange") return;


            //// 충돌한 Collider의 Transform을 가져옴
            //Transform otherTransform = this.transform;

            //// 충돌한 Collider의 게임 오브젝트가 플레이어 오브젝트인지 검사
            //if (otherTransform.root.gameObject == gameObject)
            //{
            //    // 플레이어 오브젝트와 충돌한 경우
            //    Debug.Log("플레이어 오브젝트와 충돌");
            //}
            //else if (otherTransform.IsChildOf(transform))
            //{
            //    // 플레이어 오브젝트의 하위 오브젝트와 충돌한 경우
            //    Debug.Log("플레이어 오브젝트의 하위 오브젝트와 충돌");
            //}


            //if(this.transform.IsChildOf(other.transform)) 
            //{
            //    int a = 0;
            //}

            //else
            //{
            //    int a = 0;
            //}

            if (gameObject.name == "AttackRange")
            {
                Debug.Log("조명이 공격범위");
            }

            Debug.Log("조명 범위");
            EventManager.instance.playerEnterTheLightRange();
        }

        if(other.gameObject.tag == "EnemySensor")
        {
            Debug.Log("적에게 포착");
            EventManager.instance.playerDetectedMonster(other.gameObject);
        }
    }



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