using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSensor : MonoBehaviour
{

    public void SetEffectPerField(Player p, EffectKinds effectKinds, float value)
    {
        switch (effectKinds)
        {
            case EffectKinds.Damage:
                p.Damaged(value);
                break;

            case EffectKinds.Slow:
                p.Slowed(value);
                break;

            case EffectKinds.Seal:
                p.Sealed(value);
                break;
        }

    }




    private void OnTriggerStay(Collider other)
    {
        if (gameObject != null && StageManager.Sstate == StageState.Play)
        {
            if (other.CompareTag("Player"))
            //if (other.gameObject.tag == "Player")
            {
                FieldEffect fe = gameObject.transform.parent.GetComponent<FieldEffect>();

                if (fe.IsActivated())
                {
                    Player p = other.gameObject.GetComponent<Player>();
                    BlockData blockdata = fe.GetBlockData();

                    if(blockdata.fieldKinds == FieldKinds.Stack)
                    {
                        if(fe.stack > blockdata.fieldValue)
                        {
                            SetEffectPerField(p, blockdata.effectKinds, (blockdata.fieldValue * blockdata.effectValue) + blockdata.weight);
                            //SetEffectPerField(p, blockdata.effectKinds, blockdata.fieldValue * (blockdata.effectValue + blockdata.weight));
                            //fe.stack = 0;
                        }

                        else { fe.stack += Time.deltaTime; }
                    }

                    else { SetEffectPerField(p, blockdata.effectKinds, blockdata.effectValue + blockdata.weight); }
                }

            }

        }
    }





}
