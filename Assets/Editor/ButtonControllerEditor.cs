
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
//    //    DrawDefaultInspector(); //�⺻ �ν����� GUI
//    //    ButtonController buttonController = (ButtonController)target; //����

//    //    // enum ���� ���� ����Ʈ �ʵ� ǥ��
//    //    if (buttonController.buttonAction == ButtonController.ButtonAction.CheckNRun)
//    //    {
//    //        // ����Ʈ�� �ν����Ϳ� ǥ��
//    //        EditorGUILayout.LabelField("Options for CheckNRun:");
//    //        SerializedProperty optionList = serializedObject.FindProperty("optionList");
//    //        EditorGUILayout.PropertyField(optionList);
//    //        serializedObject.ApplyModifiedProperties(); // ����Ʈ ���� ����

//    //    }


//    //}

//    public override void OnInspectorGUI()
//    {
//        DrawDefaultInspector(); //�⺻ �ν����� GUI
//        ButtonController buttonController = (ButtonController)target; //����

//        // enum ���� ���� ����Ʈ �ʵ� ǥ��
//        if (buttonController.buttonAction == ButtonController.ButtonAction.CheckNRun)
//        {
//            // ����Ʈ�� �ν����Ϳ� ǥ��
//            EditorGUILayout.LabelField("Options for CheckNRun:");
//            SerializedProperty optionList = serializedObject.FindProperty("optionList");
//            EditorGUILayout.PropertyField(optionList, true);


//        }

//        serializedObject.ApplyModifiedProperties(); // ����Ʈ ���� ����
//    }
}