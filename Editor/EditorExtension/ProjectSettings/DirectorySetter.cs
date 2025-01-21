using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace CHG.Editor.Settings
{
    public class DirectorySetter : EditorWindow
    {
        class Directories
        {
            public Directories(bool available, string name)
            {
                this.available = available;
                this.name = name;

                directories = new List<Directory>();
            }
            public bool available;
            public string name;
            public List<Directory> directories = new List<Directory>();

            public void Add(Directory directory)
            {
                directories.Add(directory);
            }
        }
        [System.Serializable]
        class Directory
        {
            public Directory(string name, string path, bool isEnabled = false, Directory parent = null)
            {
                isExists = false;
                this.isEnabled = isEnabled;
                directoryName = name;
                directoryPath = path;
                this.parent = parent;
            }

            public bool isExists;
            public bool isEnabled;
            public string directoryName;
            public string directoryPath;
            public Directory parent;
        }

        static string rootPath = Application.dataPath;

        List<Directories> directoriesList;
        enum DirectoriesList {
            Reserved, Scripting, Resources, Settings, Etc, END
        }

        bool initialized = false;

        [MenuItem("Tools/CHG/Setting/Directory Setting")]
        public static void ShowWindow()
        {
            GetWindow<DirectorySetter>("Directory Setter");
        }

        private void OnGUI() {
            for(int i = 0; i < directoriesList.Count; ++i)
            {
                directoriesList[i].available = GUILayout.Toggle(directoriesList[i].available, directoriesList[i].name);
                if(directoriesList[i].available)
                {
                    GUILayout.BeginVertical();
                    for(int j = 0; j < directoriesList[i].directories.Count; ++j)
                    {
                        if(directoriesList[i].directories[j].parent == null || directoriesList[i].directories[j].parent.isEnabled)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(20);
                            if(directoriesList[i].directories[j].isExists)
                            {
                                GUILayout.Label(directoriesList[i].directories[j].directoryName + " : 생성됨");
                            }
                            else
                            {
                                directoriesList[i].directories[j].isEnabled = GUILayout.Toggle(directoriesList[i].directories[j].isEnabled, directoriesList[i].directories[j].directoryName);
                            }
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                    EditorGUILayout.Separator();
                }
            }

            if(GUILayout.Button("확인"))
            {
                CreateDirectories();
                CheckDirectories();
                Repaint();
            }
        }
        private void OnEnable() {
            if(!initialized)
            {
                InitializeDirectoryDatas();
            }
            CheckDirectories();
        }

        private void InitializeDirectoryDatas()
        {
            
            directoriesList = new List<Directories>();

            Directories reservedDirectories = new Directories(true, "Unity 예약 폴더");
            Directories defaultDirectories = new Directories(true, "기본");
            Directories resourceDirectories = new Directories(true, "리소스");
            Directories settingDirectories = new Directories(true, "셋팅");
            Directories etcDirectories = new Directories(true, "기타");
            
            directoriesList.Add(reservedDirectories);
            directoriesList.Add(defaultDirectories);
            directoriesList.Add(resourceDirectories);
            directoriesList.Add(settingDirectories);
            directoriesList.Add(etcDirectories);

            //
            Directory plugin = new Directory("플러그인(Plugins)", "Plugins", true);
            Directory scripttemplates = new Directory("스크립트 템플릿(ScriptTemplates)", "ScriptTemplates", false);
            Directory editor_resources = new Directory("에디터 기본 리소스(Editor Default Resources)", "Editor Default Resources", false);
            Directory resources = new Directory("동적 리소스(Resources)", "Resources", false);
            Directory streamingAssets = new Directory("스트리밍 어셋(StreamingAssets)", "StreamingAssets", false);
            
            reservedDirectories.Add(plugin);
            reservedDirectories.Add(scripttemplates);
            reservedDirectories.Add(editor_resources);
            reservedDirectories.Add(resources);
            reservedDirectories.Add(streamingAssets);
            //

            //
            Directory script = new Directory("스크립트(Scripts)", "Scripts", true);
            Directory script_so = new Directory("스크립터블 오브젝트(Scripts/ScriptableObjects)", "Scripts/ScriptableObjects", false, script);
            Directory script_editor = new Directory("에디터 스크립트(Scripts/Editor)", "Scripts/Editor", false, script);
            Directory script_utility = new Directory("유틸리티 스크립트(Scripts/Utilities)", "Scripts/Utilities", false, script);
            Directory prefab = new Directory("프리팹(Prefabs)", "Prefabs", true);
            Directory scene = new Directory("씬(Scenes)", "Scenes", true);

            defaultDirectories.Add(script);
            defaultDirectories.Add(script_so);
            defaultDirectories.Add(script_editor);
            defaultDirectories.Add(script_utility);
            defaultDirectories.Add(scene);
            defaultDirectories.Add(prefab);
            //

            //
            Directory animations = new Directory("애니메이션(Animations)", "Animations", true);
            Directory datas = new Directory("ScriptableObjects 데이터(Data)", "Data", true);
            Directory textures = new Directory("텍스처(Textures)", "Textures", true);
            Directory fonts = new Directory("폰트(Fonts)", "Fonts", true);
            Directory models = new Directory("모델(Models)", "Models", true);
            Directory materials = new Directory("머티리얼(Materials)", "Materials", true);
            Directory shaders = new Directory("셰이더(Shaders)", "Shaders", true);
            Directory sounds = new Directory("사운드(Sounds)", "Sounds", true);
            Directory bgm = new Directory("배경 음악(Sounds/BGM)", "Sounds/BGM", true, sounds);
            Directory sfx = new Directory("효과음(Sounds/SFX)", "Sounds/SFX", true, sounds);

            resourceDirectories.Add(animations);
            resourceDirectories.Add(datas);
            resourceDirectories.Add(fonts);
            resourceDirectories.Add(models);
            resourceDirectories.Add(textures);
            resourceDirectories.Add(materials);
            resourceDirectories.Add(shaders);
            resourceDirectories.Add(sounds);
            resourceDirectories.Add(bgm);
            resourceDirectories.Add(sfx);
            //

            Directory settings = new Directory("설정(Settings)", "Settings", true);
            Directory settings_input = new Directory("입력 설정(Settings/Inputs)", "Settings/Inputs", true, settings);

            settingDirectories.Add(settings);
            settingDirectories.Add(settings_input);
            
            Directory rootAssetbundles = new Directory("루트 폴더 어셋 번들(../AssetBundles)", "../AssetBundles", false);                
            Directory references = new Directory("외부 레퍼런스(References)", "References", false);

            etcDirectories.Add(rootAssetbundles);
            etcDirectories.Add(references);

            initialized = true;
        }

        private void OnDisable() {
        }
        

        private void CheckDirectories()
        {
            for(int i = 0; i < directoriesList.Count; ++i)
            {
                for(int j = 0; j < directoriesList[i].directories.Count; ++j)
                {
                    if(System.IO.Directory.Exists(Path.Combine(rootPath, directoriesList[i].directories[j].directoryPath)))
                    {
                        directoriesList[i].directories[j].isExists = true;
                        directoriesList[i].directories[j].isEnabled = true;
                    }
                }
            }
        }
        private void CreateDirectories()
        {
            for(int i = 0; i < directoriesList.Count; ++i)
            {
                if(!directoriesList[i].available)
                {
                    continue;
                }

                for(int j = 0; j < directoriesList[i].directories.Count; ++j)
                {
                    if(directoriesList[i].directories[j].isExists || !directoriesList[i].directories[j].isEnabled ||
                        (directoriesList[i].directories[j].parent != null && !directoriesList[i].directories[j].parent.isEnabled))
                    {
                        continue;
                    }
                    CreateDirectory(Path.Combine(rootPath, directoriesList[i].directories[j].directoryPath));
                }
            }
            
            AssetDatabase.Refresh();
        }
        void CreateDirectory(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }
    }
}