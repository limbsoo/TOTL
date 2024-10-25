
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ButtonController))]
[CanEditMultipleObjects]
public class ButtonControllerEditor : Editor
{
//    //SerializedProperty optionList;

//    //void OnEnable()
//    //{
//    //    optionList = serializedObject.FindProperty("optionList");
//    //}

//    //public override void OnInspectorGUI()
//    //{
//    //    DrawDefaultInspector(); //기본 인스펙터 GUI
//    //    ButtonController buttonController = (ButtonController)target; //참조

//    //    // enum 값에 따라 리스트 필드 표시
//    //    if (buttonController.buttonAction == ButtonController.ButtonAction.CheckNRun)
//    //    {
//    //        // 리스트를 인스펙터에 표시
//    //        EditorGUILayout.LabelField("Options for CheckNRun:");
//    //        SerializedProperty optionList = serializedObject.FindProperty("optionList");
//    //        EditorGUILayout.PropertyField(optionList);
//    //        serializedObject.ApplyModifiedProperties(); // 리스트 변동 적용

//    //    }


//    //}

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); //기본 인스펙터 GUI
//        ButtonController buttonController = (ButtonController)target; //참조

//        // enum 값에 따라 리스트 필드 표시
//        if (buttonController.buttonAction == ButtonController.ButtonAction.CheckNRun)
//        {
//            // 리스트를 인스펙터에 표시
//            EditorGUILayout.LabelField("Options for CheckNRun:");
//            SerializedProperty optionList = serializedObject.FindProperty("optionList");
//            EditorGUILayout.PropertyField(optionList, true);


//        }

//        serializedObject.ApplyModifiedProperties(); // 리스트 변동 적용
//    }
}