using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelConstructSet", menuName = "ConstructSet/LevelConstructSet", order = 2)]


public class LevelConstructSet : ScriptableObject
{
    public GameObject map;

    public List<GameObject> player;
    public List<GameObject> enemy;
    public List<GameObject> lightObstacle;

    //[SerializeField]
    public int stageNumber;
    public int difficulty;
    //public int mapSize;

    public int timeConstrain;
    public int targetScore;
    public int enemyCnt;
    public int obstacleCnt;

    //public int obstacleCnt;

    //public string monsterName;
    //public float maxHealth;
    //public float attackDamage;
}
