using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PopUPs", menuName = "Scriptable Object/PopUPs", order = int.MaxValue)]

public class ScenePopups : ScriptableObject 
{
    [System.Serializable]
    public class ScenePopup
    {
        public PopupType popupType;
        public GameObject popup;
    }

    //public PopupType popupType;
    //public GameObject popup;

    //public class ScenePopup
    //{
    //    public PopupType popupType;
    //    public GameObject popup;
    //}


    public List<ScenePopup> popups;

    //public List<GameObject> popupsList;
}


