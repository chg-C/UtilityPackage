using UnityEditor;
using UnityEngine;

namespace CHG.Utilities.Texts
{
    public class DirectTextEditHeader
    {
        private static HeaderInfo selectedHeaderInfo = new HeaderInfo()
        {
            UseCustomEdit = false,
            IsReadOnly = true
        };

        private readonly string[] TABNAMES = new string[] {
            "Custom Editor",
            "Original Text"
        };

        public HeaderInfo DrawHeader(bool useCustomEdit)
        {
            bool wasCustomEdit = selectedHeaderInfo.UseCustomEdit;
            bool wasReadOnly = selectedHeaderInfo.IsReadOnly;

            if(useCustomEdit)
            {
                var tabIDX = GUILayout.Toolbar(selectedHeaderInfo.UseCustomEdit ? 0 : 1, TABNAMES, GUILayout.MinWidth(200));
                selectedHeaderInfo.UseCustomEdit = tabIDX == 0;
            }

            GUILayout.FlexibleSpace();
            
            bool isReadOnly = GUILayout.Toggle(selectedHeaderInfo.IsReadOnly, "Read Only");
            selectedHeaderInfo.IsReadOnly = isReadOnly;

            bool isDirty = (wasCustomEdit != selectedHeaderInfo.UseCustomEdit) || (wasReadOnly != selectedHeaderInfo.IsReadOnly);
            if(isDirty)
            {
                GUI.FocusControl(null);
            }

            return selectedHeaderInfo;
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
        bool isReadOnly;
        public bool IsReadOnly
        {
            get {return isReadOnly;}
            set {isReadOnly = value;}
        }
    }
}