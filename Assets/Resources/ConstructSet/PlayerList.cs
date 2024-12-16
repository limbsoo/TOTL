using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "PlayerList", menuName = "Scriptable Object/PlayerList", order = int.MaxValue)]
public class PlayerList : ScriptableObject
{
    [System.Serializable]
    public class PlayerGroup
    {
        public Sprite sprite;
        public GameObject player;
        public Sprite SkillSprite;

        public PlayerStats Stats;
    }

    //public PopupType popupType;
    //public GameObject popup;

    //public class ScenePopup
    //{
    //    public PopupType popupType;
    //    public GameObject popup;
    //}


    public List<PlayerGroup> lists;

    //public List<GameObject> popupsList;
}