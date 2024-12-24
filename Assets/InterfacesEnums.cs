//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class InterfacesEnums : MonoBehaviour
//{
//    //namespace State
//}


using UnityEngine;



public enum SceneState { Preparation, Play }
public enum PlayerState { Original, Transformed }

//public enum PlayState { Wait, Playing }

public enum GameResult {Proceeding, Win, Lose}

public enum LightCondition { Usual, Blink, Shadow, Disable, LessPenealty }



//public enum StageState { Wait, Play }

public enum StageState { Edit, Play }

public enum PlayerSkillState { Teleport, Hide, Decoy}

//public enum FieldEffectState { Damage, Seal, Slow}




public enum FieldKinds { None = 0, Long = 1, Stack = 2, Move = 3 }
public enum EffectKinds { None = 0, Damage = 1, Slow = 2, Seal = 3 }
public enum WeightKinds { None = 0, ActiveTime = 1, Value = 2 }


public enum PlayerSkillKinds { None = 0, Teleport = 1, Decoy = 2, Hide = 3 }


public enum PlayerEvent
{
    Damaged,
    GetGold,
    GetKey,
    ArriveGoal,
    SkillCoolDownUpdate,
    Die


}



public enum PopupType
{
    SelectCharacter,
    DataIsExist,
    DataIsNotExist,
    Options,
    Pause,
    GameOver

    //Warning,
    //Confirmation,
    //Information,
    //Error
}



//public enum ButtonType
//{
//    ShowPopUP,
//    Confirm,
//    Close
//}

//public enum CondtionType
//{
//    None,
//    HaveSaveData
//}



public enum PopupButtonType
{
    ShowPopUP,
    Confirm,
    Close
}

public enum PopupButtonConditionType
{
    None,
    HaveSaveData
}

public enum ButtonType
{
    Close,
    StartOrWarn,
    ContinueOrWarn,
    Open,
    ResetData,
    Left,
    Right,
    LoadStage,
    LoadMain,
    Reroll,
    Quit,
    UseSkill
}


public enum CharacterUnlock
{
    None,
    Clear10Wave,
    Clear20Wave
}


public enum Chase
{
    None,
    Player,
    Decoy
}