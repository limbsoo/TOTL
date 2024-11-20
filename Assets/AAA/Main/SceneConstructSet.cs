using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "SceneConstructSet", menuName = "ConstructSet/SceneConstructSet", order = 3)]

public class SceneConstructSet : ScriptableObject
{
    [System.Serializable]
    public class PopupData
    {
        public PopupType type;
        public GameObject prefab;
    }

    public PopupData[] popups;


    //public List<GameObject> PopUps;


    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}
}
