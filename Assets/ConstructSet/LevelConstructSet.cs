using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConstructSet", menuName = "ConstructSet/LevelConstructSet", order = 2)]


public class LevelConstructSet : ScriptableObject
{
    //public GameObject map;
    //public GameObject prePlayer;
    //public GameObject player;

    public List<ObjectPool> map;
    public List<ObjectPool> prePlayer;
    public List<ObjectPool> player;
    public List<ObjectPool> preEnemies;
    public List<ObjectPool> enemies;
    public List<ObjectPool> lightObstacles;

    public int targetScore;
    public int timeConstrain;








    //public List<GameObject> player;
    //public List<GameObject> enemy;
    //public List<GameObject> lightObstacle;

    ////[SerializeField]
    //public int stageNumber;
    //public int difficulty;
    ////public int mapSize;

    //public int timeConstrain;
    //public int targetScore;
    //public int enemyCnt;
    //public int obstacleCnt;


    ////public int obstacleCnt;

    ////public string monsterName;
    ////public float maxHealth;
    ////public float attackDamage;
}

[System.Serializable]
public class ObjectPool
{
    public GameObject obj;
    public int cnt;
}