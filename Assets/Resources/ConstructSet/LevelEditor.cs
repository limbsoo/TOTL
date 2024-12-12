//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;

//[CustomEditor(typeof(LevelConstructSet))]
//public class LevelEditor : Editor
//{
//    //private GameObject selectedMap;
//    //private GameObject selectedPlayer;
//    //private GameObject selectedEnemy;
//    //private GameObject selectedStatusEffect;

//    //public override void OnInspectorGUI()
//    //{
//    //    DrawDefaultInspector();

//    //    LevelConstructSet levelConstructSet = (LevelConstructSet)target;

//    //    if (levelConstructSet.gameConstructSet == null)
//    //    {
//    //        EditorGUILayout.HelpBox("Please assign a GameConstructSet.", MessageType.Warning);
//    //        return;
//    //    }

//    //    // Map 선택 드롭다운
//    //    DrawSelectionDropdown(
//    //        "Map",
//    //        levelConstructSet.gameConstructSet.Map,
//    //        ref selectedMap,
//    //        levelConstructSet.selectedMaps
//    //    );

//    //    // Player 선택 드롭다운
//    //    DrawSelectionDropdown(
//    //        "Player",
//    //        levelConstructSet.gameConstructSet.Player,
//    //        ref selectedPlayer,
//    //        levelConstructSet.selectedPlayers
//    //    );

//    //    // Enemy 선택 드롭다운
//    //    DrawSelectionDropdown(
//    //        "Enemy",
//    //        levelConstructSet.gameConstructSet.Enemy,
//    //        ref selectedEnemy,
//    //        levelConstructSet.selectedEnemies
//    //    );

//    //    // Status Effect 선택 드롭다운
//    //    DrawSelectionDropdown(
//    //        "Status Effect",
//    //        levelConstructSet.gameConstructSet.StatusEffectLight,
//    //        ref selectedStatusEffect,
//    //        levelConstructSet.selectedStatusEffects
//    //    );

//    //    if (GUI.changed)
//    //    {
//    //        EditorUtility.SetDirty(levelConstructSet);
//    //    }
//    //}

//    //private void DrawSelectionDropdown(string label, List<GameObject> options, ref GameObject selected, List<GameObject> targetList)
//    //{
//    //    if (options == null || options.Count == 0)
//    //    {
//    //        EditorGUILayout.HelpBox($"No {label} available in the selected GameConstructSet.", MessageType.Info);
//    //        return;
//    //    }

//    //    string[] optionNames = options.ConvertAll(o => o.name).ToArray();
//    //    int selectedIndex = options.IndexOf(selected);
//    //    selectedIndex = EditorGUILayout.Popup($"Select {label}", selectedIndex, optionNames);
//    //    selected = options[selectedIndex];

//    //    if (GUILayout.Button($"Add Selected {label}"))
//    //    {
//    //        if (selected != null && !targetList.Contains(selected))
//    //        {
//    //            targetList.Add(selected);
//    //        }
//    //    }
//    //}
//}








////using UnityEngine;
////using UnityEditor;
////using System.Collections.Generic;
////using static UnityEditor.Progress;
////using System.Linq;
////using Unity.VisualScripting;

////[CreateAssetMenu(fileName = "LevelEditor", menuName = "ConstructSet/LevelEditor", order = 3)]

////[CustomEditor(typeof(LevelConstructSet))]
////public class LevelEditor : Editor
////{
////    private GameObject selectedMap;
////    private GameObject selectedPlayer;
////    private GameObject selectedEnemy;
////    private GameObject selectedStatusEffect;

////    public override void OnInspectorGUI()
////    {
////        DrawDefaultInspector();

////        LevelConstructSet levelConstructSet = (LevelConstructSet)target;

////        if (levelConstructSet.gameConstructSet == null)
////        {
////            EditorGUILayout.HelpBox("Please assign a GameConstructSet.", MessageType.Warning);
////            return;
////        }

////        // Map 선택 드롭다운
////        DrawSelectionDropdown(
////            "Map",
////            levelConstructSet.gameConstructSet.Map,
////            ref selectedMap,
////            levelConstructSet.selectedMaps
////        );

////        // Player 선택 드롭다운
////        DrawSelectionDropdown(
////            "Player",
////            levelConstructSet.gameConstructSet.Player,
////            ref selectedPlayer,
////            levelConstructSet.selectedPlayers
////        );

////        // Enemy 선택 드롭다운
////        DrawSelectionDropdown(
////            "Enemy",
////            levelConstructSet.gameConstructSet.Enemy,
////            ref selectedEnemy,
////            levelConstructSet.selectedEnemies
////        );

////        // Status Effect 선택 드롭다운
////        DrawSelectionDropdown(
////            "Status Effect",
////            levelConstructSet.gameConstructSet.StatusEffectLight,
////            ref selectedStatusEffect,
////            levelConstructSet.selectedStatusEffects
////        );

////        if (GUI.changed)
////        {
////            EditorUtility.SetDirty(levelConstructSet);
////        }
////    }

////    private void DrawSelectionDropdown(string label, List<GameObject> options, ref GameObject selected, List<GameObject> targetList)
////    {
////        if (options == null || options.Count == 0)
////        {
////            EditorGUILayout.HelpBox($"No {label} available in the selected GameConstructSet.", MessageType.Info);
////            return;
////        }

////        string[] optionNames = options.ConvertAll(o => o.name).ToArray();
////        int selectedIndex = options.IndexOf(selected);
////        selectedIndex = EditorGUILayout.Popup($"Select {label}", selectedIndex, optionNames);
////        selected = options[selectedIndex];

////        if (GUILayout.Button($"Add Selected {label}"))
////        {
////            if (selected != null && !targetList.Contains(selected))
////            {
////                targetList.Add(selected);
////            }
////        }
////    }
////}