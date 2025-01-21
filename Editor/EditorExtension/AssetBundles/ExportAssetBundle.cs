using UnityEngine;
using UnityEditor;

namespace CHG.Editor.Bundle
{
    public class ExportAssetBundle : EditorWindow
    {
        static string sceneNameHeader = "AssetBundles";

        string path;

        [MenuItem("Tools/CHG/Bundle/Build Asset Bundle %&E")]
        public static void ShowWindow()
        {
            GetWindow<ExportAssetBundle>("Build Asset Bundle");
        }
        private void OnGUI() {
            GUILayout.BeginVertical();

            GUILayout.BeginHorizontal();
            GUILayout.Label("경로", GUILayout.Width(50));
            path = GUILayout.TextField(path);
            //EditorGUILayout.DropdownButton()
            GUILayout.EndHorizontal();

            if(GUILayout.Button("어셋 번들 빌드"))
            {
                BuildAssetBundle(BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
            }

            GUILayout.EndVertical();
        }
        private void BuildAssetBundle(BuildAssetBundleOptions options, BuildTarget target)
        {
            BuildPipeline.BuildAssetBundles(sceneNameHeader, options, target);
        }
    }
}