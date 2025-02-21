using System;
using UnityEditor;
using UnityEngine;

namespace CHG.Editor
{
    /// <summary>
    /// TODO: Editor Utility 관리 시스템 만들기
    /// </summary>
    public class Initializer : EditorWindow
    {
        //[MenuItem("Tools/CHG/Initial Setting", priority = 0)]
        public static void InitWindow()
        {
            EditorWindow.GetWindow<Initializer>("Utility Initializer");
        }
        void OnEnable()
        {
            Debug.Log("Opening");
        }
        void OnGUI()
        {
            EditorGUILayout.BeginVertical();

            if(GUILayout.Button("One-Click Initialization"))
            {
                InitializeAll();
            }

            EditorGUILayout.EndVertical();
        }
        #region Init
        private void InitializeAll()
        {
            
        }
        void InitBootstrap()
        {

        }
        void InitDevelopProfile()
        {

        }
        #endregion
        void OnDisable()
        {

            Debug.Log("Closing");
        }
    }
}