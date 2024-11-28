using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStat : MonoBehaviour
{
    public float health;
    public float moveSpeed;
    public float coolDown;

    public PlayerSkillKinds playerSkillKind;
}
