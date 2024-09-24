using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // 직렬화

public class GameData
{
    public string name;
    //public int maxStage;
    //public int curStage;

    public int maxWave;
    public int curWave;

    public List<SetBlock> setBlocks;


    public float health;
    public float moveSpeed;
    public float damamge;
    public float coolDown;


    public int skill;



    // 각 챕터의 잠금여부를 저장할 배열
    public bool[] isUnlock = new bool[5];
}

[Serializable]
public class SetBlock
{
    public string blockName;
    public int lineNum;
    public int start;
    public int end;

    public Vector3 position;

}