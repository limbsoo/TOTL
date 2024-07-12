using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionTest : MonoBehaviour
{
    // Collider ������Ʈ�� is Trigger�� false�� ���·� �浹�� �������� ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            Debug.Log("�浹 ����!");
            EventManager.instance.playerCollisionEnemy(collision.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //this.gameObject

        //attack range ������

        if(other.gameObject.tag == "LightRange")
        {
            if (gameObject.tag == "AttackRange") return;


            //// �浹�� Collider�� Transform�� ������
            //Transform otherTransform = this.transform;

            //// �浹�� Collider�� ���� ������Ʈ�� �÷��̾� ������Ʈ���� �˻�
            //if (otherTransform.root.gameObject == gameObject)
            //{
            //    // �÷��̾� ������Ʈ�� �浹�� ���
            //    Debug.Log("�÷��̾� ������Ʈ�� �浹");
            //}
            //else if (otherTransform.IsChildOf(transform))
            //{
            //    // �÷��̾� ������Ʈ�� ���� ������Ʈ�� �浹�� ���
            //    Debug.Log("�÷��̾� ������Ʈ�� ���� ������Ʈ�� �浹");
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
                Debug.Log("������ ���ݹ���");
            }

            Debug.Log("���� ����");
            EventManager.instance.playerEnterTheLightRange();
        }

        if(other.gameObject.tag == "EnemySensor")
        {
            Debug.Log("������ ����");
            EventManager.instance.playerDetectedMonster(other.gameObject);
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




}