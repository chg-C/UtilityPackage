using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace CHG.Utilities.Bundle
{
    
    [System.Serializable]
    public class PackageMetadata
    {
        public string name; // 패키지 이름
        public string displayName; // 패키지 표시 이름
        public string version; // 패키지 버전
        public string description; // 패키지 설명
        public List<string> keywords; // 키워드 목록
        public Author author; // 저자 정보
        public Dictionary<string, string> dependencies; // 의존성

        [System.Serializable]
        public class Author
        {
            public string name; // 저자 이름
            public string email; // 저자 이메일
            public string url; // 저자 웹사이트 URL
        }
    }

    public class PackageCreater : EditorWindow
    {
        private PackageMetadata packageMetadata = new PackageMetadata();

        [MenuItem("Assets/Create/Package")]
        private static void ShowWindow()
        {
            GetWindow<PackageCreater>("Create Package");
        }

        private void OnGUI() {
            packageMetadata.name = EditorGUILayout.TextField("Package Name", packageMetadata.name);
            packageMetadata.displayName = EditorGUILayout.TextField("Display Name", packageMetadata.displayName);
            packageMetadata.version = EditorGUILayout.TextField("Version", packageMetadata.version);
            packageMetadata.description = EditorGUILayout.TextField("Description", packageMetadata.description);
            
            if (packageMetadata.keywords == null)
                packageMetadata.keywords = new List<string>();        
            GUILayout.Label("Keywords", EditorStyles.boldLabel);
            for (int i = 0; i < packageMetadata.keywords.Count; i++)
            {
                packageMetadata.keywords[i] = EditorGUILayout.TextField($"Keyword {i + 1}", packageMetadata.keywords[i]);
            }

            GUILayout.Label("Author", EditorStyles.boldLabel);
            packageMetadata.author = packageMetadata.author ?? new PackageMetadata.Author();
            packageMetadata.author.name = EditorGUILayout.TextField("Author Name", packageMetadata.author.name);
            packageMetadata.author.email = EditorGUILayout.TextField("Author Email", packageMetadata.author.email);
            packageMetadata.author.url = EditorGUILayout.TextField("Author URL", packageMetadata.author.url);
            if (packageMetadata.dependencies == null)
            packageMetadata.dependencies = new Dictionary<string, string>();

            GUILayout.Label("Dependencies", EditorStyles.boldLabel);
            foreach (var key in new List<string>(packageMetadata.dependencies.Keys))
            {
                EditorGUILayout.BeginHorizontal();
                packageMetadata.dependencies[key] = EditorGUILayout.TextField(key, packageMetadata.dependencies[key]);
                if (GUILayout.Button("Remove"))
                {
                    packageMetadata.dependencies.Remove(key);
                }
                EditorGUILayout.EndHorizontal();
            }
            // 패키지 저장 버튼
            if (GUILayout.Button("Save to JSON"))
            {
                string json = JsonUtility.ToJson(packageMetadata, true);
                System.IO.File.WriteAllText("Assets/packageMetadata.json", json);
                Debug.Log("Package metadata saved to Assets/packageMetadata.json");
            }
        }
    }
}