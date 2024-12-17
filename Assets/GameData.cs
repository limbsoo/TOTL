using System;
using System.Collections.Generic;
using UnityEngine;




[Serializable] // 직렬화
public class GameData
{
    public int playerCharacterIdx;

    public int stageModeIdx;
    public int maxWave;
    public int gold;
    public int curWave;



    public List<BlockData> blockdata;

    public PlayerStats playerStats;

    //public PlayerData pd;

    //public List<FieldEffectBlock> febs;

    //public int skill;

    //public int maxStage;
    //public int curStage;



    //public string name;


    //public float health;
    //public float moveSpeed;
    //public float damamge;
    //public float coolDown;


    //public List<SetBlock> setBlocks;






    //public List<FieldEffect> FEs;


    //// 각 챕터의 잠금여부를 저장할 배열
    //public bool[] isUnlock = new bool[5];

    //public List<int> slots = new List<int>();

    //public TimeLine[] timeLines = new TimeLine[10];
}

[Serializable]
public class BlockData
{
    public FieldKinds fieldKinds;
    public EffectKinds effectKinds;
    public WeightKinds weightKinds;

    public float start;
    public float end;
    public float weight;

    public float fieldValue;
    public float fieldDuration;

    public float effectValue;
    public float effectDuration;

    public int lineNum;
    public Vector3 position;
}


[Serializable]
public class BlockInfo
{
    public FieldKinds fieldKinds;
    public EffectKinds effectKinds;

    public float value;
    public float duration;
}

[Serializable]
public class PlayerStats
{
    public float health;
    public float moveSpeed;
    public float coolDown;
    public float effectValue;

    public PlayerSkillKinds playerSkillKind;
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