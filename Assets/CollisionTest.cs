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


    // Collider ������Ʈ�� is Trigger�� false�� ���·� �浹�� �������� ��
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Enemy")
        {
            Debug.Log("�浹 ����!");
            isContact = true;
            OnCollisionResult?.Invoke(collision.gameObject);
        }



        
    }

    // Collider ������Ʈ�� is Trigger�� false�� ���·� �浹���� ��
    //private void OnCollisionStay(Collision collision)
    //{
    //    if (collision.collider.tag == "Enemy")
    //    {
    //        Debug.Log("�浹 ��!");
    //    }
    //}

    // Collider ������Ʈ�� is Trigger�� false�� ���·� �浹�� ������ ��
    //private void OnCollisionExit(Collision collision)
    //{
    //    Debug.Log("�浹 ��!");
    //}

    void Update()
    {
        isContact = false;
    }
}