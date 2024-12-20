using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{

    [SerializeField] private int value;
    [SerializeField] private int lifeCycle;

    void Update()
    {
        if(StageManager.instance.lifeCycle >= lifeCycle)
        {
            Destroy(gameObject);
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if (other.CompareTag("Player"))
            {
                StageManager.instance.UpdateGold(value);
                //TextManager.instance.UpdateTexts();
                //Player P = other.gameObject.GetComponent<Player>();
                //P.gold += value;
                Destroy(gameObject);
            }

        }
    }


}
