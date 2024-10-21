using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSensor : MonoBehaviour
{


    private bool isDelayTriggered = false;
    public float triggerTime = 2.0f; // 효과를 발동시키기 위한 시간 (초)
    private float triggerEnterTime = 0.0f; // Trigger가 시작된 시간

    private void OnTriggerEnter(Collider other)
    {

        if (StageManager.Sstate == StageState.Play)
        {
            switch (other.gameObject.tag)
            {
                case ("Player"):

                    FieldEffect fe = gameObject.transform.parent.GetComponent<FieldEffect>();

                    if (fe == null)
                    {
                        isDelayTriggered = false;
                        triggerEnterTime = 0.0f;
                    }



                    else if (fe.m_upperIdx == 1)
                    {
                        isDelayTriggered = true;
                        triggerEnterTime = Time.time;
                    }
                    break;
            }
        }
    }



    private void OnTriggerStay(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if (other.gameObject.tag == "Player")
            {
                FieldEffect fe = gameObject.transform.parent.GetComponent<FieldEffect>();

                if (fe == null)
                {
                    isDelayTriggered = false;
                    triggerEnterTime = 0.0f;
                }

                else if (fe.m_upperIdx == 1)
                {
                    if (isDelayTriggered)
                    {
                        if (Time.time - triggerEnterTime >= triggerTime)
                        {

                            triggerEnterTime = 0.0f;

                            Player P = other.gameObject.GetComponent<Player>();

                            P.ApplyDelayEffect(fe);
                        }
                    }
                }

                else
                {
                    if (fe.IsActivated())
                    //if(other.gameObject.GetComponent<CapsuleCollider>().enabled)
                    {

                        Player P = other.gameObject.GetComponent<Player>();
                        P.setEffected(fe.m_downerIdx);
                    }


                }
            }




            //if (other.gameObject.tag == "Player")
            //{
            //    Player P = other.gameObject.GetComponent<Player>();
            //    P.gold += value;
            //    Destroy(gameObject);
            //}

        }
    }




    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
