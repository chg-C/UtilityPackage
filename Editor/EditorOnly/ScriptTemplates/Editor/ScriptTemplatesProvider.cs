using System;
using System.Collections.Generic;
using CHG.Editor.Texts;
using CHG.Editor.Utilities;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UIElements;

namespace CHG.Editor.ScriptTemplator
{
    using Editor = UnityEditor.Editor;

    /// <summary>
    /// Script Template 관리의 메인 Provider 윈도우
    /// </summary>
    public class ScriptTemplatesProvider : SettingsProvider
    {
        public ScriptTemplatesProvider(string path, SettingsScope scopes, ScriptTemplates templateSetting, IEnumerable<string> keywords = null) : base(path, scopes, keywords) 
        {
            this.templateSetting = templateSetting;
        }

        ScriptTemplates templateSetting;
        Editor templatesEditor;
        Editor templateTextEditor;
        TextAsset[] templateAssets = new TextAsset[0];
        ReorderableList templateList;


        string prevPath = "";

        void OnChanged()
        {
            templateAssets = LoadTXTAssets(templateSetting.templatesPath);
            prevPath = templateSetting.templatesPath;
            templateList.list = templateAssets;
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            base.OnActivate(searchContext, rootElement);
            templateList = new ReorderableList(templateAssets, typeof(TextAsset), true, false, false, false);
            templateList.drawHeaderCallback = (rect) => {
                EditorGUI.LabelField(rect, "Script Templates");
            };
            templateList.drawElementCallback = (rect, index, isActivated, isFocused) =>  {
                var element = templateAssets[index];                
                GUI.Label(rect, element.name);
            };
            templateList.drawHeaderCallback = (rect) => {
                EditorGUI.LabelField(rect, "ScriptTemplate Text File List");
            };
            templateList.drawFooterCallback = (rect) => {                
                Rect rectBake = new Rect(rect)
                {
                    x = rect.width-200,
                    width = 200
                };
                if(GUI.Button(rectBake, "Bake Template"))
                {
                    RecursiveFolderGenerator.CreateFolderRecursively(ScriptTemplates.SettingPath + "/Editor");
                    TemplateBaker.CreateTemplatorScript(ScriptTemplates.SettingPath + "/Editor", templateAssets);
                }
            };

            EditorApplication.projectChanged += OnChanged;
            OnChanged();

            templatesEditor = Editor.CreateEditor(templateSetting);
        }
        public override void OnGUI(string searchContext)
        {
            EditorGUI.BeginChangeCheck();
            templatesEditor.OnInspectorGUI();
            if(EditorGUI.EndChangeCheck())
            {
                if(prevPath != templateSetting.templatesPath)
                {
                    ///Template Target Folder Changed
                    OnChanged();
                }
            }
            
            EditorGUILayout.Space(10);
            templateList.DoLayoutList();

        }

        public override void OnDeactivate()
        {
            EditorApplication.projectChanged -= OnChanged;
            base.OnDeactivate();
        }
        // private void OnGUI() {
            
        // }
        private TextAsset[] LoadTXTAssets(string templatesPath)
        {
            string[] guids = AssetDatabase.FindAssets("t:TextAsset", new[] { templatesPath });
            TextAsset[] textAssets = new TextAsset[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                textAssets[i] = AssetDatabase.LoadAssetAtPath<TextAsset>(assetPath);
            }

            return Array.FindAll(textAssets, x => AssetUtility.GetAssetExtension(x) == ".txt");
        }

        

        #region Provider Regist
        [SettingsProvider]
        public static SettingsProvider RegisterProvider()
        {
            ScriptTemplates templates = AssetDatabase.LoadAssetAtPath<ScriptTemplates>(ScriptTemplates.FullName);
            if(templates == null)
            {
                templates = InitScriptTempates();
            }
            
            return CreateProvider("Project/CHG/Script Templates", templates);
        }
        static SettingsProvider CreateProvider(string settingsWindowPath, ScriptTemplates templates)
        {
            var provider = new ScriptTemplatesProvider(settingsWindowPath, SettingsScope.Project, templates);
            return provider;
        }

        private static ScriptTemplates InitScriptTempates()
        {
            Debug.LogWarning("Generate Script Templates Setting On" + ScriptTemplates.SettingPath);
            RecursiveFolderGenerator.CreateFolderRecursively(ScriptTemplates.SettingPath);

            ScriptTemplates templates = ScriptableObject.CreateInstance<ScriptTemplates>();
            templates.Init();
            AssetDatabase.CreateAsset(templates, ScriptTemplates.FullName);
            AssetDatabase.SaveAssets();

            return templates;
        }

        [MenuItem("Tools/CHG/Script Templates", priority = 21)]
        static void OpenProvider()
        {
            SettingsService.OpenProjectSettings("Project/CHG/Script Templates");
        }
        #endregion
    }
}