using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{

    private void OnTriggerStay(Collider other)
    {
        //if (StageManager.Sstate == StageState.Play)
        //{
        //    switch (gameObject.tag)
        //    {
        //        case ("Player"):
        //            Enemy enemy = transform.parent.GetComponent<Enemy>();
        //            enemy.chasingPlayer();


        //            break;
        //    }




        //    //switch (other.gameObject.tag)
        //    //{
        //    //    if (gameObject.tag == "Player")
        //    //    {




        //    //case ("EnemySensor"):
        //    //    if (other.transform.parent != null)
        //    //    {
        //    //        if (gameObject.tag == "Player")
        //    //        {
        //    //            chasingPlayer();

        //    //        }

        //    //        if (Player.instance.summonDecoy)
        //    //        {
        //    //            if (gameObject.tag == "Decoy")
        //    //            {
        //    //                chasingPlayer();
        //    //                //EventManager.instance.playerDetectedMonster(other.gameObject);
        //    //            }


        //    //        }

        //    //        //else
        //    //        //{


        //    //        //    chasingPlayer(); //EventManager.instance.playerDetectedMonster(other.gameObject); chasingPlayer();
        //    //        //}


        //    //    }
        //    //    break;
        //    //}
        //}







        //// Start is called before the first frame update
        //void Start()
        //{

        //}

        //// Update is called once per frame
        //void Update()
        //{

        //}
    }
}