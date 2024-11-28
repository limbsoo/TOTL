//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//[CreateAssetMenu(fileName = "GameConstructSet", menuName = "ConstructSet/GameConstructSet", order = 1)]
//[Serializable]
//public class GameConstructSet : ScriptableObject
//{
//    public List<LevelConstructSet> LevelConstructSet;


//    public List<GameObject> Map;
//    public List<GameObject> Player;
//    public List<GameObject> Enemy;
//    public List<GameObject> StatusEffectLight;

//}


////public class GameConstructSet : ScriptableObject
////{
////    //public int idx;
////    public List<LevelConstructSet> levelConstructSets;

////    //public string monsterName;
////    //public float maxHealth;
////    //public float attackDamage;

////    public List<CellPos> lightObstacleGrid;


////}

////[System.Serializable]
////public class CellPos
////{
////    public int[] cnt;
////}
///
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Game Construct Set", menuName = "Game/Game Construct Set")]
public class GameConstructSet : ScriptableObject
{
    public List<LevelConstructSet> LevelConstructSet;
    public int currentLevel;


    //public List<LevelConstructSet> LevelConstructSet;
    //public List<GameObject> Map;
    //public List<GameObject> Player;
    //public List<GameObject> Enemy;
    //public List<GameObject> StatusEffectLight;
}