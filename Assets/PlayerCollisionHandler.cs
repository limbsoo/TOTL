using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    // Collider ������Ʈ�� is Trigger�� false�� ���·� �浹�� �������� ��
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Enemy")
        {
            //Debug.Log("�浹 ����!");
            EventManager.instance.playerCollisionEnemy(collision.gameObject);
            


        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "preEnemy")
        {
            EventManager.instance.prePlayerCollisionPreEnemy(other.gameObject);
        }


        //���� �ð� ���Ŀ� ��ǥ���� �ʱ�ȭ�ؼ� �i�ư��ٰ� ���� ����� ���߰�
        if (gameObject.tag == "AttackRange")
        {
            //Debug.Log("player attack");

            if (other.gameObject.tag == "Enemy")
            {
                if (StageManager.Sstate == StageState.Play)
                {
                    EventManager.instance.EnemyInAttackRange(other.gameObject);
                }

                  
            }
            
        }




        if (other.gameObject.tag == "LightRange")
        {
            if (gameObject.tag == "AttackRange") return;

            EventManager.instance.playerEnterTheLightRange();
        }

        if (other.gameObject.tag == "EnemySensor")
        {
            if(other.transform.parent != null) 
            {

                //�����ð� �Ŀ� ���߰� ����
                if (StageManager.Sstate == StageState.Play)
                {
                    //Debug.Log("������ ����");

                    if (gameObject.tag == "AttackRange") return;

                    EventManager.instance.playerDetectedMonster(other.gameObject);
                }


            }



        }
    }



    //if (otherTransform.root.gameObject == gameObject)
    //if (otherTransform.IsChildOf(transform))


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