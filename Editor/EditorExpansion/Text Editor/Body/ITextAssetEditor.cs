using UnityEngine;

namespace CHG.Utilities.Texts
{
    public interface ITextAssetEditor
    {
        bool IsTarget(TextAsset textAsset);
        string DrawEditor(string text);
    }
}