using System;

[Serializable] // 직렬화

public class GameData
{
    public string name;
    public int maxStage;
    public int curStage;

    public float health;
    public float moveSpeed;
    public float damamge;
    public float coolDown;






    // 각 챕터의 잠금여부를 저장할 배열
    public bool[] isUnlock = new bool[5];
}