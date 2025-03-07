using UnityEngine;
using UnityEditor;

namespace CHG.Editor.Bundle
{
    public class ExportAssetBundle : EditorWindow
    {
        static string sceneNameHeader = "AssetBundles";

        string pathToExport = "../AssetBundles";
        BuildAssetBundleOptions options = BuildAssetBundleOptions.None;
        BuildTarget buildTarget = BuildTarget.StandaloneWindows64;

        [MenuItem("Tools/CHG/Bundle/Build Asset Bundle", priority = 99)]
        public static void ShowWindow()
        {
            GetWindow<ExportAssetBundle>("Build Asset Bundle");
        }
        
        private void OnGUI() {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Export To ", GUILayout.Width(80));
            EditorGUILayout.LabelField(pathToExport);
            if(GUILayout.Button("Change"))
            {
                EditorUtility.OpenFolderPanel("Change Bundle Path", null, "");
            }
            GUILayout.EndHorizontal();
            buildTarget = (BuildTarget)EditorGUILayout.EnumPopup("Build Target", buildTarget);
            options = (BuildAssetBundleOptions)EditorGUILayout.EnumFlagsField("Build Options", options);

            if(GUILayout.Button("Build Asset Bundle"))
            {
                BuildAssetBundlesParameters parameter = new BuildAssetBundlesParameters()
                {
                    outputPath = pathToExport,
                    targetPlatform = buildTarget,
                    options = options
                };
                

                BuildAssetBundle(parameter);
            }

            GUILayout.EndVertical();
        }
        private void BuildAssetBundle(BuildAssetBundlesParameters param)
        {
            
            BuildPipeline.BuildAssetBundles(param);
        }
    }
}