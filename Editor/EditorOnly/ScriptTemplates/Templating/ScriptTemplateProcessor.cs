using UnityEngine;
using UnityEditor;
using CHG.Utilities.Datas;
using System.IO;
using System;

namespace CHG.Editor.ScriptTemplator
{
    /// <summary>
    /// Script Template 데이터를 실제 적용하는 클래스
    /// AssetModificationProcessor를 상속해서 Asset이 만들어질 때 Text를 대체
    /// </summary>
    public class ScriptTemplateProcessor : AssetModificationProcessor
    {        
        public static void OnWillCreateAsset(string path)
        {
            ///.cs.meta 파일이 아니라면 무시하고 넘어가기
            if(!path.EndsWith(".cs.meta"))
            {
                return;
            }

            ///수정해야 할 파일은 meta 파일이 아니라 cs 파일이므로 경로 재설정
            path = path.Replace(".cs.meta", ".cs");

            ScriptTemplates templateSetting = AssetDatabase.LoadAssetAtPath<ScriptTemplates>(ScriptTemplates.FullName);
            if(templateSetting == null || templateSetting.keywordsData == null)
            {
                ///Template 사용 설정을 하지 않았다면 return
                //Debug.LogWarning("Script Templating Error: Setting Script Templates First.");
                return;
            }

            string filePath = FileUtility.ConvertToAbsolutePath(path);
            if(!File.Exists(filePath))
            {
                //파일이 존재하지 않음
                Debug.LogWarning("Script Templating Error: No File In " + filePath);
                return;
            }

            string content = File.ReadAllText(filePath);

            content = content.Replace("#DATE#", DateTime.Now.ToString("yyyy - MM - dd"));
            content = content.Replace("#VERSION#", Application.version);

            for(int i = 0; i < templateSetting.keywordsData.keywords.Count; ++i)
            {
                content = content.Replace(templateSetting.keywordsData.keywords[i].keyword, templateSetting.keywordsData.keywords[i].replaceText);
            }

            File.WriteAllText(filePath, content);

            //Editor 준비 대기 후 Reimport
            EditorApplication.delayCall += () =>
            {
                AssetDatabase.ImportAsset(path);
            };
        }
    }
}