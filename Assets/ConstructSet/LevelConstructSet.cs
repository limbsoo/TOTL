using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConstructSet", menuName = "ConstructSet/LevelConstructSet", order = 2)]

public class LevelConstructSet : ScriptableObject
{
    public int targetScore;

    public int enemyCnt;

    public int increaseEnemyPerWave;



    public List<GameObject> map;
    public List<GameObject> player;
    public List<GameObject> enemy;
    public List<GameObject> FieldEffectLight;
    public List<GameObject> Goal;


    public List<GameObject> FieldEffect;
}





















////public class LevelConstructSet : ScriptableObject
////{
////    public GameConstructSet GameConstructSet;  // 참조할 ItemContainer
////    //public ScriptableItem selectedItem;  // 선택된 ScriptableItem

////    public void SelectItem(int index)
////    {
////        if (GameConstructSet != null && index >= 0 && index < GameConstructSet.items.Length)
////        {
////            selectedItem = itemContainer.items[index];
////        }
////    }



////    public int targetScore;
////    //public int timeConstrain;



////}



//public class LevelConstructSet : ScriptableObject
//{

//    //    public GameConstructSet gameConstructSet;
//    //public List<GameObject> selectedMaps = new List<GameObject>();
//    //public List<GameObject> selectedPlayers = new List<GameObject>();
//    //public List<GameObject> selectedEnemies = new List<GameObject>();
//    //public List<GameObject> selectedStatusEffects = new List<GameObject>();






//    //public GameObject map;
//    //public GameObject prePlayer;
//    //public GameObject player;

//    public List<ObjectPool> map;
//    public List<ObjectPool> prePlayer;
//    public List<ObjectPool> player;
//    public List<ObjectPool> preEnemies;
//    public List<ObjectPool> enemies;
//    public List<ObjectPool> lightObstacles;

//    public int targetScore;
//    public int timeConstrain;



//    //public List<List<int>> lightObstacleGrid;

//    public List<GridSet> lightObstacleGrid;


//    //public List<GameObject> player;
//    //public List<GameObject> enemy;
//    //public List<GameObject> lightObstacle;

//    ////[SerializeField]
//    //public int stageNumber;
//    //public int difficulty;
//    ////public int mapSize;

//    //public int timeConstrain;
//    //public int targetScore;
//    //public int enemyCnt;
//    //public int obstacleCnt;


//    ////public int obstacleCnt;

//    ////public string monsterName;
//    ////public float maxHealth;
//    ////public float attackDamage;
//}

//[System.Serializable]
//public class ObjectPool
//{
//    public GameObject obj;
//    public int cnt;
//}

//[System.Serializable]
//public class GridSet
//{
//    public int[] cnt;
//}