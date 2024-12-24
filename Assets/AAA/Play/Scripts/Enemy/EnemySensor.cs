using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class EnemySensor : MonoBehaviour
{
    Enemy enemy;
    PlayerSkillKinds _playerSkillKinds;

    private void Start()
    {
        enemy = gameObject.transform.parent.GetComponent<Enemy>();
    }



    void OnTriggerEnter(Collider other)
    {
        enemy.UpdateDetectedList(other.gameObject, true);
    }


    void OnTriggerExit(Collider other)
    {
        enemy.UpdateDetectedList(other.gameObject, false);
    }







    //void HandlePlayerUseSkill(PlayerSkillKinds playerSkillKinds, float effectValue, float coolDown)
    //{
    //    switch (playerSkillKinds)
    //    {
    //        case (PlayerSkillKinds.Hide):

    //            _playerSkillKinds = PlayerSkillKinds.Hide;

    //            //playerUseHide = true;

    //            StartCoroutine(Function.instance.CountDown(effectValue, () =>
    //            {
    //                _playerSkillKinds = PlayerSkillKinds.None;
    //            }));

    //            break;


    //        case (PlayerSkillKinds.Decoy):

    //            StartCoroutine(Function.instance.CountDown(effectValue, () =>
    //            {
    //                if (enemy.curTarget == "Decoy" || enemy.curTarget == "")
    //                {
    //                    //StageManager.instance.OnPlayerUseSkill -= HandlePlayerUseSkill;
    //                    BackOriginPos();

    //                }
    //            }));

    //            break;
    //    }
    //}



    //void BackOriginPos()
    //{
    //    enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
    //    {
    //        enemy.updateDesitnationPos(enemy.originPos); // ���� �÷��̾ �Ѵ� �Լ�
    //        enemy.curTarget = "";

    //    }));
    //}





    //private void OnTriggerStay(Collider other)
    //{
    //    if (gameObject != null && StageManager.Sstate == StageState.Play)
    //    {

    //        //if (other.CompareTag("Player") || other.CompareTag("Decoy"))
    //        //if (other.gameObject.tag == "Player" || other.gameObject.tag == "Decoy")
    //        {
    //            //Player p = other.gameObject.transform.GetComponent<Player>();

    //            if (_playerSkillKinds == PlayerSkillKinds.Hide)
    //            {
    //                if (enemy.curTarget != "" && enemy.chasing == null)
    //                {
    //                    enemy.updateDesitnationPos(enemy.transform.position);
    //                    enemy.curTarget = "";
    //                    BackOriginPos();
    //                }

    //            }

    //            else
    //            {

    //                if (enemy.chasing != null)
    //                {
    //                    StopCoroutine(enemy.chasing);
    //                }

    //                switch (other.gameObject.tag)
    //                {
    //                    case ("Decoy"):// �浹�� ���� �����̸� �������� ��
    //                        enemy.updateDestination(other.gameObject.transform); // ���� �÷��̾ �Ѵ� �Լ�
    //                        enemy.curTarget = "Decoy"; // ���� ���� Ÿ���� �����̷� ����
    //                        if (enemy.chasing != null) enemy.chasing = null;
    //                        break;

    //                    case ("Player"):

    //                        // ���� ���� �����̸� ���� ������ ���� ���� �÷��̾ �����ϵ��� ��
    //                        if (enemy.curTarget == "Decoy") enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����

    //                        else
    //                        {
    //                            enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����
    //                            enemy.updateDestination(other.gameObject.transform); // ���� �÷��̾ ����
    //                        }

    //                        if (enemy.chasing != null) enemy.chasing = null;
    //                        break;
    //                }
    //            }

    //        }
    //    }
    //}
























    //Enemy enemy;
    //PlayerSkillKinds _playerSkillKinds;


    ////bool playerUseHide;

    //private void Start()
    //{



    //    enemy = gameObject.transform.parent.GetComponent<Enemy>();
    //    //playerUseHide = false;


    //    _playerSkillKinds = PlayerSkillKinds.None;


    //    //StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;
    //}



    //void OnTriggerEnter(Collider other)
    //{
    //    //StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;

    //    //if (other.CompareTag("Player"))
    //    //{
    //    //    //StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;

    //    //}
    //}


    //void OnTriggerExit(Collider other)
    //{
    //    if (gameObject != null && StageManager.Sstate == StageState.Play)
    //    {

    //        //if (other.CompareTag("Player") || other.CompareTag("Decoy"))
    //        //if(other.CompareTag("Player"))
    //        {
    //            //StageManager.instance.OnPlayerUseSkill -= HandlePlayerUseSkill;

    //            BackOriginPos();
    //        }

    //        if (other.CompareTag("Decoy"))
    //        {
    //            Debug.Log("Decoy Out");
    //        }



    //    }

    //}







    //void HandlePlayerUseSkill(PlayerSkillKinds playerSkillKinds, float effectValue, float coolDown)
    //{
    //    switch (playerSkillKinds)
    //    {
    //        case (PlayerSkillKinds.Hide):

    //            _playerSkillKinds = PlayerSkillKinds.Hide;

    //            //playerUseHide = true;

    //            StartCoroutine(Function.instance.CountDown(effectValue, () =>
    //            {
    //                _playerSkillKinds = PlayerSkillKinds.None;
    //            }));

    //            break;


    //        case (PlayerSkillKinds.Decoy):

    //            StartCoroutine(Function.instance.CountDown(effectValue, () =>
    //            {
    //                if (enemy.curTarget == "Decoy" || enemy.curTarget == "")
    //                {
    //                    //StageManager.instance.OnPlayerUseSkill -= HandlePlayerUseSkill;
    //                    BackOriginPos();

    //                }
    //            }));

    //            break;
    //    }
    //}



    //void BackOriginPos()
    //{
    //    enemy.chasing = StartCoroutine(Function.instance.CountDown(2f, () =>
    //    {
    //        enemy.updateDesitnationPos(enemy.originPos); // ���� �÷��̾ �Ѵ� �Լ�
    //        enemy.curTarget = "";

    //    }));
    //}





    //private void OnTriggerStay(Collider other)
    //{
    //    if (gameObject != null && StageManager.Sstate == StageState.Play)
    //    {

    //        //if (other.CompareTag("Player") || other.CompareTag("Decoy"))
    //        //if (other.gameObject.tag == "Player" || other.gameObject.tag == "Decoy")
    //        {
    //            //Player p = other.gameObject.transform.GetComponent<Player>();

    //            if (_playerSkillKinds == PlayerSkillKinds.Hide)
    //            {
    //                if (enemy.curTarget != "" && enemy.chasing == null)
    //                {
    //                    enemy.updateDesitnationPos(enemy.transform.position);
    //                    enemy.curTarget = "";
    //                    BackOriginPos();
    //                }

    //            }

    //            else
    //            {

    //                if (enemy.chasing != null)
    //                {
    //                    StopCoroutine(enemy.chasing);
    //                }

    //                switch (other.gameObject.tag)
    //                {
    //                    case ("Decoy"):// �浹�� ���� �����̸� �������� ��
    //                        enemy.updateDestination(other.gameObject.transform); // ���� �÷��̾ �Ѵ� �Լ�
    //                        enemy.curTarget = "Decoy"; // ���� ���� Ÿ���� �����̷� ����
    //                        if (enemy.chasing != null) enemy.chasing = null;
    //                        break;

    //                    case ("Player"):

    //                        // ���� ���� �����̸� ���� ������ ���� ���� �÷��̾ �����ϵ��� ��
    //                        if (enemy.curTarget == "Decoy") enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����

    //                        else
    //                        {
    //                            enemy.curTarget = "Player"; // Ÿ���� �÷��̾�� ����
    //                            enemy.updateDestination(other.gameObject.transform); // ���� �÷��̾ ����
    //                        }

    //                        if (enemy.chasing != null) enemy.chasing = null;
    //                        break;
    //                }
    //            }


    //            //Enemy enemy = gameObject.transform.parent.GetComponent<Enemy>();

    //        }
    //    }
    //}




    ////private void OnTriggerEnter(Collider other)
    ////{
    ////    //StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;

    ////    //if (other.CompareTag("Player"))
    ////    //{
    ////    //    //StageManager.instance.OnPlayerUseSkill += HandlePlayerUseSkill;

    ////    //}
    ////}

}

