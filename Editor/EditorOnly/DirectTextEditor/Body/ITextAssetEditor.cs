using UnityEngine;

namespace CHG.Editor.Texts
{
    public interface ITextAssetEditor
    {
        public bool UseCustomHeader
        {
            get;
        }

        bool IsTarget(TextAsset textAsset);
        string DrawEditor(string text);
        void OnTargetChanged();
    }
}