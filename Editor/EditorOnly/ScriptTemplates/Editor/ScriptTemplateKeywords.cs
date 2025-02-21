using System.Collections.Generic;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace CHG.Editor.ScriptTemplator
{
    [CreateAssetMenu(menuName = "ScriptTemplates Settings/Keywords", fileName = "ScriptTemplateKeywords", order = 0)]
    public class ScriptTemplateKeywords : ScriptableObject
    {
        [Header("미리 예약된 키워드들은 사용 불가능")]
        [Header("#SCRIPTNAME# - Script Class의 이름, #DATE# - 생성일, #VERSION# - 생성 시점에서의 버전")]
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