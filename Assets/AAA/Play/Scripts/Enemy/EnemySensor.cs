using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class EnemySensor : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if(other.gameObject.tag == "Player")
            {
                Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

                enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
                {
                    enemy.updateDesitnationPos(enemy.originPos); // ���� �÷��̾ �Ѵ� �Լ�
                    enemy.curTarget = "";

                }));
            
            }


        }

    }


    private void OnTriggerStay(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if (other.gameObject.tag == "Player" || other.gameObject.tag == "Decoy")
            {
                Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

                switch (other.gameObject.tag)
                {
                    case ("Decoy"):// �浹�� ���� �����̸� �������� ��
                        enemy.chasingPlayer(other.gameObject.transform); // ���� �÷��̾ �Ѵ� �Լ�
                        enemy.curTarget = "Decoy"; // ���� ���� Ÿ���� �����̷� ����
                        if (enemy.chasing != null) enemy.chasing = null;
                        break;

                    case ("Player"):

                        // ���� ���� �����̸� ���� ������ ���� ���� �÷��̾ �����ϵ��� ��
                        if (enemy.curTarget == "Decoy") enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����

                        else
                        {
                            enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����
                            enemy.chasingPlayer(other.gameObject.transform); // ���� �÷��̾ ����
                        }

                        if (enemy.chasing != null) enemy.chasing = null;
                        break;
                }
            }
        }
    }












    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.collider.tag == "Player")
    //    {
    //        Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

    //        enemy.chasingPlayer(collision.collider.gameObject.transform); // ���� �÷��̾ ����
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