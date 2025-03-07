using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace CHG.Editor.ScriptTemplator
{
    [CreateAssetMenu(menuName = "ScriptTemplates Settings/Keywords", fileName = "ScriptTemplateKeywords", order = 0)]
    public class ScriptTemplateKeywords : ScriptableObject
    {
        [Header("Cannot use pre-defined keywords")]
        [Header("#SCRIPTNAME#, #DATE#, #VERSION#, #NOTRIM#")]
        public List<ScriptTemplateKeyword> keywords;
        
        [System.Serializable]
        public class ScriptTemplateKeyword
        {
            [Tooltip("키워드")]
            public string keyword;
            [Tooltip("대체될 텍스트")]
            public string replaceText;
        }
    }
}