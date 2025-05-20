using UnityEngine;
using UnityEditor; // Needed for MenuItem, Selection, AssetDatabase
using System.IO;
using UnityEditor.VersionControl; // Needed for Path and File

namespace CHG.Editor.Settings
{
    /// <summary>
    /// Unity Editor에서 ReadMe 파일을 만드는 유틸리티 클래스
    /// </summary>
    public static class ReadMeGenerator
    {
        /// <summary>
        /// 기본 Text File 이름
        /// </summary>
        private const string kFileName = "README";

        /// <summary>
        /// 기본 Text File Content
        /// </summary>
        private const string kFallbackDefaultContent = 
        @"#README
        설명을 위한 Text File입니다.
        ";

        /// <summary>
        /// ReadMe File을 생성
        /// </summary>
        [MenuItem("Assets/Create/ReadMe File", priority = 90)]
        public static void CreateReadMeFile()
        {
            string path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (string.IsNullOrEmpty(path))
            {
                path = "Assets";
            }

            if (File.Exists(path))
            {
                path = Path.GetDirectoryName(path);
            }

            string filePath = Path.Combine(path, kFileName + ".txt");

            // Prevent overwriting existing file
            if (File.Exists(filePath))
            {
                Debug.LogWarning($"ReadMe 파일이 이미 {filePath}에 존재합니다.");
                UnityEngine.Object existingFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath);
                if (existingFile != null)
                {
                    Selection.activeObject = existingFile;
                    EditorGUIUtility.PingObject(existingFile);
                }

                return;
            }

            try
            {
                File.WriteAllText(filePath, kFallbackDefaultContent);
                AssetDatabase.Refresh(); // Refresh to show the new file in the editor
                Debug.Log($"ReadMe 파일이 {filePath}에 생성되었습니다.");
                
                UnityEngine.Object existingFile = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(filePath);
                if (existingFile != null)
                {
                    Selection.activeObject = existingFile;
                    EditorGUIUtility.PingObject(existingFile);
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create ReadMe file: {e.Message}");
            }
        }
    }
}