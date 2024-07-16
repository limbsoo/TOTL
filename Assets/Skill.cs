using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public static Skill instance { get; private set; }

    public float teleportLength;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else Destroy(gameObject);
    }


    public float skillCooldown; // 스킬 쿨타임
    private bool canUseSkill = true; // 스킬 사용 가능 여부

    public bool CanUseSkills()
    {
        return canUseSkill;
    }

    public void UseSkill()
    {
        Debug.Log("Skill used!");

        canUseSkill = false;
        EventManager.TriggerSkillUsed(skillCooldown);
        StartCoroutine(ResetSkillAvailability(skillCooldown));
     


        //if (canUseSkill)
        //{
        //    Debug.Log("Skill used!");
        //    Teleport();
        //    canUseSkill = false;
        //    EventManager.TriggerSkillUsed(skillCooldown);
        //    StartCoroutine(ResetSkillAvailability(skillCooldown));
        //}

        //else Debug.Log("Skill is on cooldown.");
    }

    private IEnumerator ResetSkillAvailability(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canUseSkill = true;
    }



}
