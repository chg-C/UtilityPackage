using System.IO;
using UnityEditor;
using UnityEngine;

namespace CHG.Editor.Texts
{
    public class DirectTextEditFooter : MonoBehaviour
    {
        public FileInfo DrawFoot(FileInfo info)
        {
            info.IsReadOnly = EditorGUILayout.Toggle("읽기 전용", info.IsReadOnly);
            
            return info;
        }
    }
}
