using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEditorInternal;
using System;
using System.Reflection;
using CHG.Utilities.Datas;
using System.IO;

namespace CHG.Editor.Bundle
{
public class PackageCreater : EditorWindow
    {
        private PackageMetadata packageMetadata = new PackageMetadata();
        private ReorderableList dependenciesList;
        private ReorderableList keywordsList;

        bool showOption = true;

        bool useEditor = false;
        bool generateTests = false;
        bool generateInternalDocuments = true;

        [MenuItem("Assets/Create/Package/Create Custom Package Here")]
        private static void ShowWindow()
        {
            PackageCreater pc = GetWindow<PackageCreater>(true, "Create Package");
        }

        private void OnEnable() {
            List<string> keywords = packageMetadata.keywords;
            keywordsList = new ReorderableList(keywords, typeof(string), true, true, true, true);
            keywordsList.drawHeaderCallback =
                (rect) => {
                    EditorGUI.LabelField(rect, "Keywords", EditorStyles.boldLabel);
                };
            keywordsList.drawElementCallback = 
                (rect, index, activated, focused) => {
                    keywords[index] = EditorGUI.TextField(rect, keywords[index]);
                };
            keywordsList.onAddCallback =
                (list) => {
                    keywords.Add("");
                };
            keywordsList.onRemoveCallback =
                (list) => {
                    if(list.index >= 0 && list.index < keywords.Count)
                    {
                        keywords.RemoveAt(list.index);
                    }
                };
            
            List<PackageMetadata.Dependency> dependencies = packageMetadata.dependencies;
            dependenciesList = new ReorderableList(dependencies, typeof(PackageMetadata.Dependency), true, true, true, true);
            dependenciesList.drawHeaderCallback =
                (rect) => {
                    EditorGUI.LabelField(rect, "Dependencies", EditorStyles.boldLabel);
                };
            dependenciesList.drawElementCallback =
                (rect, index, activated, focused) => {
                    var item = dependencies[index];
                    //rect.height = 50;
                    Rect r = new Rect(20, rect.y+2, rect.width, EditorGUIUtility.singleLineHeight);
                    item.targetPackageName = EditorGUI.TextField(r, "Package Name", item.targetPackageName);
                    r = new Rect(20, rect.y+EditorGUIUtility.singleLineHeight+4, rect.width, EditorGUIUtility.singleLineHeight);
                    item.versionRequired = EditorGUI.TextField(r, "Required Version", item.versionRequired);
                };
            dependenciesList.elementHeightCallback =
                (index) => {
                    return EditorGUIUtility.singleLineHeight*2 + 4;
                };
            dependenciesList.onAddCallback =
                (list) => {
                    dependencies.Add(new PackageMetadata.Dependency());
                };
            dependenciesList.onRemoveCallback = 
                (list) => {
                    if(list.index >= 0 && list.index < dependencies.Count)
                    {
                        dependencies.RemoveAt(list.index);
                    }
                };
        }
        private void OnDisable() {
            
        }

        private void OnGUI() {
            bool validated = true;

            GUILayout.Label("Essential");
            GUILayout.BeginVertical(GUI.skin.box);
            packageMetadata.name = EditorGUILayout.TextField("Package Name", packageMetadata.name);
            if(string.IsNullOrEmpty(packageMetadata.name))
            {
                EditorGUILayout.HelpBox("패키지 이름은 필수 필드입니다.", MessageType.Error);
                validated = false;
            }
            else if(!Validators.IsValidReverseDomain(packageMetadata.name))
            {
                EditorGUILayout.HelpBox("패키지 이름은 역-도메인 표기법을 따라야 합니다(com.example)", MessageType.Error);
                validated = false;
            }
            packageMetadata.packageVersion = EditorGUILayout.TextField("Package Version", packageMetadata.packageVersion);
            if(string.IsNullOrEmpty(packageMetadata.packageVersion))
            {
                EditorGUILayout.HelpBox("패키지 버전은 필수 필드입니다.", MessageType.Error);
                validated = false;
            }
            else if(!Validators.IsValidSemanticVersion(packageMetadata.packageVersion))
            {
                EditorGUILayout.HelpBox("패키지 버전은 시맨틱 버전 표기법을 따라야 합니다(major.minor.patch)", MessageType.Error);
                validated = false;
            }
            GUILayout.EndVertical();

            GUILayout.Label("Recommended");
            GUILayout.BeginVertical(GUI.skin.box);
            packageMetadata.displayName = EditorGUILayout.TextField("Display Name", packageMetadata.displayName);
            packageMetadata.description = EditorGUILayout.TextField("Description", packageMetadata.description);
            packageMetadata.minUnityVersion = EditorGUILayout.TextField("Required Unity Version", packageMetadata.minUnityVersion);
            GUILayout.EndVertical();

            showOption = EditorGUILayout.Foldout(showOption, "Optional");
            if(showOption)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                useEditor = EditorGUILayout.Toggle("Package Uses Editor", useEditor);
                generateTests = EditorGUILayout.Toggle("Generate Tests Folder", generateTests);
                GUILayout.Space(5);
                generateInternalDocuments = EditorGUILayout.Toggle("Generate Internal Documents", generateInternalDocuments);
                GUILayout.Space(5);
                packageMetadata.documentationURL = EditorGUILayout.TextField("External Documents URL", packageMetadata.documentationURL);
                packageMetadata.changelogURL = EditorGUILayout.TextField("Changelog URL", packageMetadata.changelogURL);
                packageMetadata.useCustomLicense = EditorGUILayout.Toggle("Use Custom License", packageMetadata.useCustomLicense);
                if(!packageMetadata.useCustomLicense)
                {
                    packageMetadata.license = (LicenseType)EditorGUILayout.EnumPopup("License Type", packageMetadata.license);
                }
                else if(packageMetadata.license != LicenseType.NONE)
                {
                    packageMetadata.licensesURL = EditorGUILayout.TextField("Licenses URL", packageMetadata.licensesURL);
                }

                dependenciesList.DoLayoutList();
                keywordsList.DoLayoutList();
                GUILayout.Label("Author", EditorStyles.boldLabel);
                packageMetadata.author = packageMetadata.author ?? new PackageMetadata.Author();
                packageMetadata.author.name = EditorGUILayout.TextField("Author Name", packageMetadata.author.name);
                packageMetadata.author.email = EditorGUILayout.TextField("Author Email", packageMetadata.author.email);
                packageMetadata.author.url = EditorGUILayout.TextField("Author URL", packageMetadata.author.url);
                GUILayout.EndVertical();
            }

            GUI.enabled = validated;
            // 패키지 저장 버튼
            if (GUILayout.Button("Generate Package"))
            {
                // 현재 프로젝트 폴더 경로 얻기
                Type projectWindowUtilType = typeof(ProjectWindowUtil);
                MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath", BindingFlags.Static | BindingFlags.NonPublic);
                object obj = getActiveFolderPath.Invoke(null, new object[0]);
                string currentPath = obj.ToString();

                GeneratePackage(currentPath);
            }
            GUI.enabled = true;
        }
        void GeneratePackage(string path)
        {
            string directoryName = string.IsNullOrEmpty(packageMetadata.displayName) ? packageMetadata.name.ToLower() : packageMetadata.displayName;
            string fullPath = Path.Combine(path, directoryName);
            Directory.CreateDirectory(fullPath);

            string jsonText = packageMetadata.ToJson();
            //Build Package Manifest
            File.WriteAllText(fullPath+"/package.json", jsonText);
            //Create Empty Markdown Documents
            File.Create(fullPath+"/README.md").Close();
            File.Create(fullPath+"/CHANGELOG.md").Close();
            File.Create(fullPath+"/Third Party Notices.md").Close();
            if(!packageMetadata.useCustomLicense)
            {

                string license = LicenseGenerator.GenerateLicense(packageMetadata.license,
                    string.IsNullOrEmpty(packageMetadata.author.name) ? "" : packageMetadata.author.name,
                    DateTime.Now.Year.ToString()
                );
                if(!string.IsNullOrEmpty(license))
                {
                    File.WriteAllText(fullPath+"/LICENSE.md", license);
                }
            }

            //Create SubFolders
            if(useEditor)
            {
                Directory.CreateDirectory(fullPath + "/Editor");
            }
            Directory.CreateDirectory(fullPath + "/Runtime");
            //Test SubFolders
            if(generateTests)
            {
                if(useEditor)
                {
                    Directory.CreateDirectory(fullPath + "/Tests/Editor");
                }
                Directory.CreateDirectory(fullPath + "/Tests/Runtime");
            }
            //Non-Standard Folders
            Directory.CreateDirectory(fullPath + "/Samples~");
            if(generateInternalDocuments)
            {
                Directory.CreateDirectory(fullPath + "/Documentation~");
            }

            AssetDatabase.Refresh();
        }
    }

    #region Utilities
    
    public enum LicenseType {
        NONE, MIT, APACHE2, GPL2, GPL3, LGPL, BSD2, BSD3, CC, MPL, Unlicense
    }
    public static class LicenseGenerator
    {
        public static string GenerateLicense(LicenseType type, string author, string year, string locale = "en")
        {
            switch(type)
            {   
                case LicenseType.MIT:
                    return GenerateMIT(author, year, locale);
                case LicenseType.APACHE2:
                    return GenerateApache2(author, year, locale);
                default:
                case LicenseType.NONE:
                    return "";                
            }
        }
        static string GenerateMIT(string author, string year, string locale)
        {
            return $@"# MIT License

Copyright (c) {year} {author}

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the ""Software""), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

...

THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.";
        }
        static string GenerateApache2(string author, string year, string locale)
        {
            return $@"# Apache License 2.0

Copyright {year} {author}

Licensed under the Apache License, Version 2.0 (the ""License"");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

    http://www.apache.org/licenses/LICENSE-2.0

...

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an ""AS IS"" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.";
        }
    }

    public class JsonBuilder
    {
        public string jsonText;

        int depth = 0;


        bool isFirst = true;

        public void SetDepth()
        {
            for(int i = 0; i < depth; ++i)
            {
                jsonText += "\t";
            }
        }

        public void OpenBrace(string brace = "{")
        {
            SetDepth();
            jsonText += brace + "\n";
            depth += 1;
            isFirst = true;
        }
        public void OpenBrace(string key, string brace = "{")
        {
            if(!isFirst)
            {
                jsonText += ",\n";
            }
            SetDepth();
            jsonText += $"\"{key}\": ";
            jsonText += brace+"\n";
            depth += 1;
            isFirst = true;
        }
        public void CloseBrace(string brace = "}")
        {
            depth -= 1;
            jsonText += "\n";
            SetDepth();
            jsonText += brace;
        }
        public void Push(string value)
        {
            
            if(!isFirst)
            {
                jsonText += ",\n";
            }

            SetDepth();
            jsonText += $"\"{value}\"";
            isFirst = false;
        }
        public void Push(string key, string value)
        {
            if(!isFirst)
            {
                jsonText += ",\n";
            }

            SetDepth();
            jsonText += $"\"{key}\": \"{value}\"";
            isFirst = false;
        }
    }
    #endregion
    #region Data Classes
    [System.Serializable]
    public class PackageMetadata
    {
        //필수Essential
        public string name; // 패키지 이름, 역-도메인 표기법 사용
        public string packageVersion; // 패키지 버전, 시맨틱 버전 표기법 사용
        //권장Recommended
        public string minUnityVersion; //패키지 사용을 위한 Unity 최소 버전
        public string displayName; // 패키지 표시 이름
        public string description; // 패키지 설명

        //옵션Optional
        public List<string> keywords = new List<string>(); // 키워드 목록
        public List<Dependency> dependencies = new List<Dependency>(); // 의존성
        public Author author; // 저자 정보
        
        public bool useCustomLicense = false;
        public LicenseType license;
        public string licenseStr;
        public string licensesURL; //웹 라이선스 경로

        public string changelogURL;
        public string documentationURL;


        [System.Serializable]
        public class Author
        {
            public string name; // 저자 이름(필수)
            public string email; // 저자 이메일
            public string url; // 저자 웹사이트 URL
        }
        [System.Serializable]
        public class Dependency
        {
            public string targetPackageName; //필요 패키지 이름
            public string versionRequired; //필요 패키지 버전(앞에 ^가 붙으면 최소 버전)
        }
        [System.Serializable]
        public class Samples
        {
            public string displayName; //샘플 이름
            public string description; //샘플 설명
            public string path; //샘플 경로
        }

        public string ToJson()
        {
            JsonBuilder builder = new JsonBuilder();
            builder.OpenBrace();
            //Essential
            builder.Push("name", name.ToLower());
            builder.Push("version", packageVersion);
            //Recommended
            if(!string.IsNullOrEmpty(displayName))
                builder.Push("displayName", displayName);
            if(!string.IsNullOrEmpty(description))
                builder.Push("description", description);
            if(!string.IsNullOrEmpty(minUnityVersion))
                builder.Push("unity", minUnityVersion);
            
            //Options            
            if(!string.IsNullOrEmpty(documentationURL))
                builder.Push("documentationUrl", documentationURL);
            if(!string.IsNullOrEmpty(changelogURL))
                builder.Push("changelogUrl", changelogURL);

            if(dependencies.Count > 0)
            {
                builder.OpenBrace("dependencies", "{");
                for(int i = 0; i < dependencies.Count; ++i)
                {
                    if(!Validators.IsValidReverseDomain(dependencies[i].targetPackageName))
                    {
                        Debug.LogError("Invalid Package Name In " + i + ", " + dependencies[i].targetPackageName.ToLower());
                    }
                    else if(!Validators.IsValidSemanticVersion(dependencies[i].versionRequired))
                    {
                        Debug.LogError("Invalid Package Version In " + i + ", " + dependencies[i].targetPackageName);
                    }
                    else
                    {
                        builder.Push(dependencies[i].targetPackageName, dependencies[i].versionRequired);
                    }
                }
                builder.CloseBrace();
            }
            if(keywords.Count > 0)
            {
                builder.OpenBrace("keywords", "[");
                for(int i = 0; i < keywords.Count; ++i)
                {
                    builder.Push(keywords[i]);
                }
                builder.CloseBrace("]");
            }
            if(author != null)
            {
                builder.OpenBrace("author", "{");
                builder.Push("name", author.name);
                builder.Push("email", author.email);
                builder.Push("url", author.url);
                builder.CloseBrace();
            }
            builder.CloseBrace();

            return builder.jsonText;
        }
    }
    #endregion
    
}