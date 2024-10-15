using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable] // 직렬화

public class GameData
{
    //public PlayerData pd;

    //public List<FieldEffectBlock> febs;

    public int skill;

    //public int maxStage;
    //public int curStage;

    public int gold;

    public string name;
    public int maxWave;
    public int curWave;
    public float health;
    public float moveSpeed;
    public float damamge;
    public float coolDown;


    //public List<SetBlock> setBlocks;



    public List<BlockData> febs;





    //// 각 챕터의 잠금여부를 저장할 배열
    //public bool[] isUnlock = new bool[5];

    //public List<int> slots = new List<int>();

    //public TimeLine[] timeLines = new TimeLine[10];
}

[Serializable]
public class BlockData
{
    public int m_upperIdx;
    public int m_downerIdx;



    public string blockName;
    public int lineNum;
    public int start;
    public int end;



    public Vector3 position;

}





//[Serializable]
//public class PlayerData
//{
//    public string name;
//    public int maxStage;
//    public int curStage;

//    public float health;
//    public float moveSpeed;
//    public float damamge;
//    public float coolDown;


//}


//[Serializable]
//public class BlockData
//{
//    public string blockName;
//    public int lineNum;
//    public int start;
//    public int end;

//    public int upperIdx;
//    public int DownerIdx;


//    public Vector3 position;

//}




































//using System;
//using System.Collections.Generic;
//using UnityEngine;

//[Serializable] // 직렬화

//public class GameData
//{
//    public string name;
//    //public int maxStage;
//    //public int curStage;

//    public int maxWave;
//    public int curWave;

//    public List<SetBlock> setBlocks;


//    public float health;
//    public float moveSpeed;
//    public float damamge;
//    public float coolDown;


//    public int skill;



//    // 각 챕터의 잠금여부를 저장할 배열
//    public bool[] isUnlock = new bool[5];

//    public List<int> slots = new List<int>();

//    public TimeLine[] timeLines = new TimeLine[10];
//}

//[Serializable]
//public class SetBlock
//{
//    public string blockName;
//    public int lineNum;
//    public int start;
//    public int end;

//    public int upperIdx;
//    public int DownerIdx;


//    public Vector3 position;

//}