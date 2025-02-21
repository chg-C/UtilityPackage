using UnityEditor;
using UnityEngine;

namespace CHG.Editor.Texts
{
    public class DirectTextEditHeader
    {
        private static HeaderInfo selectedHeaderInfo = new HeaderInfo()
        {
            UseCustomEdit = false
        };

        private readonly string[] TABNAMES = new string[] {
            "전용 에디터",
            "RAW 텍스트 에디터"
        };

        public HeaderInfo DrawHeader(bool useCustomEdit)
        {
            bool wasCustomEdit = selectedHeaderInfo.UseCustomEdit;

            if(useCustomEdit)
            {
                var tabIDX = GUILayout.Toolbar(selectedHeaderInfo.UseCustomEdit ? 0 : 1, TABNAMES, GUILayout.MinWidth(200));
                selectedHeaderInfo.UseCustomEdit = tabIDX == 0;
            }

            GUILayout.FlexibleSpace();

            bool isDirty = wasCustomEdit != selectedHeaderInfo.UseCustomEdit;
            if(isDirty)
            {
                GUI.FocusControl(null);
            }

            return selectedHeaderInfo;
        }
        public void OnTargetChanged()
        {

        }
    }
    
    public struct HeaderInfo
    {
        bool useCustomEdit;
        public bool UseCustomEdit
        {
            get {return useCustomEdit;}
            set {useCustomEdit = value;}
        }
    }
}