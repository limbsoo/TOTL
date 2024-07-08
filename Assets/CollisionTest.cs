using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    //public event Action<GameObject> OnPlayerCollision;
    public static event Action<GameObject> OnCollisionResult;



    public bool isContact;

    private void Awake()
    {
        if (GameSet.playerCollision == null) GameSet.playerCollision = this;
    }


    // Collider 컴포넌트의 is Trigger가 false인 상태로 충돌을 시작했을 때
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Debug.Log("충돌 시작!");
            isContact = true;
            OnCollisionResult?.Invoke(collision.gameObject);
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

    void Update()
    {
        isContact = false;
    }
}