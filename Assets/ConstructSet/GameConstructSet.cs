using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConstructSet", menuName = "ConstructSet/GameConstructSet", order = 1)]

public class GameConstructSet : ScriptableObject
{
    public int targetIdx;
    public List<LevelConstructSet> levelConstructSets;

    //public string monsterName;
    //public float maxHealth;
    //public float attackDamage;
}
